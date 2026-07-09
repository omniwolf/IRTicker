# IRTicker → New Version: Architecture Review & Rebuild Notes

> **What this is:** an autonomous, whole-app architecture review produced by six parallel reader passes over the current C# WinForms codebase, synthesised into one document. It is the *input* for a rebuild plan — **not the plan itself**. The actual staged plan (priorities, what ships first, tech choices) is a decision we should make together in a brainstorming session; see [§9 Open design questions](#9-open-design-questions-for-our-brainstorm).
>
> Produced 2026-07-08. Scope: `IRTicker/` (excluding auto-generated `*.Designer.cs` and `obj/`).

---

## 1. What the app is

A single-operator desktop tool for Independent Reserve staff. Its core job is a **live multi-exchange price/spread ticker** (Independent Reserve, BTC Markets, Bitaroo), driven by websockets with REST fallback. Bolted on around that core are several **money-moving and reporting features**: authenticated trading on IR and Coinbase (incl. an automated "market baiter"), a 6-platform balance dashboard, average-fill-price tools, a full-featured Telegram remote-control bot, Slack status/reporting, and BlinkStick USB LED indicators.

---

## 2. Current architecture map

### Component overview

| Layer | Files | Role |
|---|---|---|
| **God object / orchestrator** | `IRTicker.cs` (~3400 lines), `Program.cs`, `UIControls.cs` | Boots app, owns all shared state, runs the polling loop, marshals every UI update |
| **Data model** | `DCEs.cs`, `CE.cs`, `IRCurrencies.cs` | The `DCE` ("exchange") object: tickers, order books, history, liveness flags |
| **Realtime** | `WebSocketsConnect.cs`, `Coinbase-trade/CBWebSockets.cs` | Socket lifecycle, subscriptions, order-book delta application, reconnect/throttle |
| **REST clients** | `PrivateIR.cs`, `CoinbaseClient.cs`, `BinanceClient.cs`, `Utilities.cs` | Private + public HTTP; auth/signing; seeds & backs up the socket feeds |
| **Accounts/trading** | `IRAccountsForm.cs`, `Coinbase-trade/CBAccountsForm.cs`, `AccAvgPrice.cs`, `Balance.cs`, `BalSettings.cs`, `AccountAPIKeys.cs` | Balances, order placement/cancel, baiter, avg price, reconciliation |
| **Views/integrations** | `OBview.cs`, `SpreadGraph.cs`, `VertLabel.cs`, `TelegramBot.cs`, `Slack.cs` | Order-book/spread display, Telegram bot, Slack presence/reports |
| **Dead code** | `IRTicker.Private.cs` | 100% commented out — **do not port** |

### Data flow (current)

```
                  ┌────────────────────────── IRTicker.cs (UI thread + BackgroundWorker) ──────────────────────────┐
                  │                                                                                                 │
  WSS feeds  ─────┼─►  WebSocketsConnect ──► DCE state (order books, tickers, flags)  ◄── ParseDCE_* (REST poll)   │
 (IR/BTCM/CB)     │         │  socket threads call pollingThread.ReportProgress(code) directly (⚠ unsupported)     │
                  │         ▼                                                                                       │
  REST (public) ──┼──► Utilities.Get (SYNC) ──► DCE state                                                           │
                  │                                                                                                 │
                  │   PollingThread loop ──► ReportProgress(int code) ──► giant switch ──► ~hundreds of Labels      │
                  │                                                                                                 │
  Private REST ───┼──► PrivateIR (SDK, global lock) / CoinbaseClient / BinanceClient ──► Account forms, baiter      │
                  └─────────────────────────────────────────────────────────────────────────────────────────────┘
                             │                         │                        │
                        Telegram bot              Slack status            BlinkStick LEDs
```

### Threading model (four coexisting mechanisms)

1. **One `BackgroundWorker`** runs an infinite polling loop (`PollingThread_MeatAndPotates`). Never returns; only ends on unhandled exception, then `RunWorkerCompleted` recreates it.
2. **`ReportProgress(int)`** is the UI-marshalling bus — a magic-number switchboard (codes 1,2,12,13,14,15,21,23…64). **Socket threads call it directly** (guarded only by `IsBusy`), which is unsupported and races with worker recreation.
3. **`SynchronizationContext.Post`** — a *second*, parallel UI-dispatch idiom used by baiter/avg-price callbacks.
4. **Ad-hoc `Task.Run` + a raw `Thread`** (the `UITimerThread`, stopped with the deprecated `Thread.Abort()`).

---

## 3. Cross-cutting themes (the findings that span subsystems)

These are the systemic issues that showed up in multiple reviews — the things a rebuild most needs to fix:

- **🔴 Plaintext secrets everywhere (highest priority).** All API keys/secrets (IR ×5 keypairs, Coinbase key/secret/passphrase, Binance, B2C2 token, Slack token, ETH wallet) live in `Properties.Settings.Default` → **plaintext XML in `user.config`**. They're shown unmasked in textboxes and even written to `Debug.Print`. No encryption, no DPAPI, no keychain.
- **Unmanaged concurrency.** Liveness flags (`socketsAlive`, `HeartBeat`, `socketsReset`, `NetworkAvailable`, `HasStaticData`) are plain non-volatile fields written by socket threads and read by the worker — torn-read/visibility races that reconnect decisions depend on. Only collections are guarded (`ConcurrentDictionary` / a few `lock`s).
- **The god object + magic report codes.** `IRTicker.cs` owns everything and communicates UI updates via undocumented integer codes with fall-through. Extremely brittle; adding a feature means touching the switchboard.
- **Static, hand-wired UI.** ~60 currencies are hard-coded (three label triples each) and currency lists are hardcoded in several places — the app literally "breaks when new currencies turn up." Adding a coin = editing Designer + multiple maps.
- **Blocking I/O on hot paths.** `Utilities.Get` is synchronous `HttpWebRequest` called from the poll loop; IR private calls are synchronous under a **global lock** that serialises all private calls; `Thread.Sleep` used inside async loops (baiter, Telegram order-book view); a bad refresh-rate parse pops a **modal MessageBox from the worker thread**.
- **Silent error handling.** REST failures widely swallowed (`return ""`/`null` after `Debug.Print`); JSON routing via `message.Contains("Error")` (false-triggers on any GUID containing "Error"); `decimal.Parse` (not `TryParse`) on UI text at order-submit time.
- **Duplicated symbol normalization.** XBT↔BTC (and UST↔USDT, DOG↔DOGE, LNK↔LINK, USC↔USDC) remapped ad-hoc in many files instead of one shared layer.
- **`new HttpClient()` per request** (Coinbase, Binance) — socket-exhaustion anti-pattern.

---

## 4. What to KEEP (proven domain logic worth porting)

This is real, hard-won logic — port the *semantics*, just re-implement cleanly:

- **The IR incremental order-book engine:** dual index (price-level map + `guid→price` reverse index) for O(1) delta application, plus the **nonce-buffer resync** (seed nonce = min-buffered − 1, apply strictly sequential, dump-and-resubscribe on an unfillable gap). This is the core correctness mechanism.
- **REST-snapshot + websocket-delta hybrid**, with REST fallback while a socket is down.
- **The connection/subscription throttle** (cap N connects/hour) — it exists because of *real IP bans*; port it verbatim as a reusable rate limiter. (This is the work we just did.)
- **`CryptoPairsAdd` merge semantics** (only overwrite non-zero/newer fields) and the normalized `MarketSummary` ticker shape + `PRIMARY-SECONDARY` pair convention.
- **Liveness/heartbeat + reconnect concept** (100s no-message ⇒ reconnect).
- **Trading domain rules:** avg-price weighted calc, loan/slush "out-by" reconciliation, spread/limit-vs-market detection (`ValidateLimitOrder`), order-book depth highlighting, confirm-before-order + insufficient-balance safeguards, the Coinbase self-sorting `CB_Orderbook`.
- **Feature surfaces:** Telegram command set + edit-in-place menu UX + "order filled" push; Slack IR-vs-BTCM sentiment mapping; spread-history export; negative-spread detection.

---

## 5. What to REDESIGN

- **Concurrency → single-threaded orchestrator.** Node's single JS thread + async state machines makes most of the current races *structurally impossible*. Run exchange/socket/REST services in the **main process**; stream normalized snapshots/diffs to the renderer via IPC; renderer never blocks.
- **Kill the report-code switchboard and dual UI-dispatch idioms** → one typed event/IPC channel (or a state store the renderer subscribes to).
- **Per-exchange adapter interface** (`connect/subscribe/onMessage/disconnect`, `getBalances/placeOrder/cancelOrder/…`) instead of one `DCE` class + caller-side `switch` on string keys. Model the **IR family explicitly as one connection with three fiat-scoped books**, not three sibling objects sharing a URL.
- **Data-driven UI.** Render the price grid, order book (real DOM table/depth chart, not `RichTextBox` text), and balances from state — eliminating the hand-wired labels and the "breaks on new coins" problem. Pull the coin list + decimal config from the IR endpoint that's already called.
- **Streaming charts.** Replace MS Chart full-redraw-every-tick with an append/stream time-series lib (uPlot / lightweight-charts).
- **HTTP layer:** shared keep-alive client per exchange; genuine async; a token-bucket rate limiter + retry/back-off with **typed errors surfaced to the UI** (not swallowed); per-exchange request queues instead of one global lock.
- **Correct buffered replay:** fix the buffer-overwrite bug (`orderBuffer_IR[pair] = new …` on every pre-snapshot delta drops all but the last), make nonce continuity a first-class invariant.
- **Money-safe numerics:** JS `number` ≠ C# `decimal`; use integer minor-units or big.js.
- **Centralize** symbol normalization, magic-number thresholds/durations, and endpoints into config.
- **The baiter:** redesign around explicit order-state tracking (there's a known "order doesn't go away / shows up in closed orders" bug) rather than porting the `Thread.Sleep` state machine.

---

## 6. What to DROP

- `IRTicker.Private.cs` (dead, fully commented out).
- `Thread.Abort()` UI-timer thread; the blocking `Thread.Sleep` + modal-MessageBox-from-worker patterns.
- Plaintext `Properties.Settings` secret storage (replace, don't port).
- **BlinkStick USB LED integration** — large, device-specific, exception-prone; drop or isolate behind an optional plugin. *(Your call — see open questions.)*

---

## 7. Suggested target shape (tentative — to refine together)

```
main process (Node)                          renderer (web UI)
├─ ExchangeService per exchange              ├─ price grid (data-driven)
│   ├─ SocketClient (ws)  ── normalized ──►  ├─ order book table / depth chart
│   ├─ RestClient (keep-alive)   diffs       ├─ spread charts (streaming)
│   └─ RateLimiter/backoff       via IPC     ├─ accounts / trading panels
├─ OrderBookEngine (nonce/buffer)            └─ balance dashboard
├─ TradingService (baiter, orders)
├─ Secret store (safeStorage / keytar)   ◄── secrets NEVER reach renderer
├─ Integrations: Telegram (telegraf), Slack (@slack/web-api)
└─ Persistence: spread history + config store
```

Node library candidates: `ws` (sockets), `undici`/`axios` (HTTP), `telegraf` or `node-telegram-bot-api`, `@slack/web-api`, `uPlot`/`lightweight-charts` (charts), `keytar`/Electron `safeStorage` (secrets), `zod` (response validation).

---

## 8. Rough staging idea (tentative)

A possible ordering, to be confirmed/reshaped in the brainstorm. Read-only first, money-moving later:

1. **Foundation + read-only ticker:** main/renderer skeleton, IR socket + REST, order-book engine, data-driven price grid + order book view. Proves the hardest part (IR incremental book) early.
2. **More exchanges + charts:** BTCM, Bitaroo; streaming spread charts; liveness/reconnect/throttle.
3. **Accounts & balances (read-only):** secure secret storage, balance dashboard, avg price.
4. **Trading (money-moving):** IR + Coinbase order place/cancel with safeguards, then the baiter (redesigned).
5. **Integrations:** Telegram bot, Slack, (optional) BlinkStick.

---

## 9. Open design questions for our brainstorm

These are the decisions that are genuinely yours — they'll shape the plan more than anything above:

1. **MVP scope** — is v1 the *full* app (trading + baiter + balances + bot + LEDs), or a read-only ticker first with money-moving features later? (Strongly recommend the latter.)
2. **Trading in scope at all for the rewrite**, or is the new app primarily the *monitoring* tool and trading stays in the old one for now?
3. **Electron specifically?** A rebuild is the moment to confirm. Alternatives: **Tauri** (Rust core, much smaller/faster, better for a secrets-holding trading tool), or a plain local web app. You said "probably Electron" — worth a 5-minute discussion.
4. **Single-user still?** Affects auth, secret storage, and whether this ever runs anywhere but your machine.
5. **Distribution** — just your machine, or other IR staff? (Drives auto-update, packaging, secret provisioning.)
6. **Exchange priorities** — IR is clearly core; where do Coinbase, Binance, B2C2, Bitaroo, MetaMask rank? Which can be dropped or deferred?
7. **Keep BlinkStick LED cubes?**
8. **Automated tests this time?** Once domain logic is separated from UI it becomes highly testable — worth deciding up front.
9. **Persistence** — spread history is currently CSV; settings in `user.config`. Want a real local store (SQLite) for history/config?
10. **Telegram bot parity** — needed early, or a later phase?

---

*Next step: when you're back, we run a brainstorming session using §9 as the agenda, then turn the outcome into a concrete staged implementation plan.*
