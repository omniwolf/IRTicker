# IRTicker Rebuild — Design Spec (living document)

**Started:** 2026-07-09
**Status:** In progress — *Main window, trading window, and average-price tracker designed and approved. Remaining surfaces (Slack/volume-sentiment, Nanoleaf, architecture, Phase 2/3) still to design.*
**Companion review of the old app:** see `REBUILD_NOTES.md` (repo root).

---

## Locked decisions

### Stack
- **Electron + TypeScript.** Rationale: developer is comfortable with Node (TS is a small step); single-threaded event loop structurally eliminates the old app's concurrency races; best-in-class UI/charting ecosystem; OS-backed secret storage via Electron `safeStorage`.
- **Throughput is not the deciding factor** — the workload (hundreds to low-thousands of msgs/sec) is modest for any modern runtime. The old app's lag was architectural (O(n²) UI rebuilds, full chart redraws, blocking I/O), not language.
- **Watch-item:** JS `number` is float64 — no native `decimal`. Use integer minor-units or a decimal lib (big.js/decimal.js) for all prices/volumes/money. This is a day-one design rule.

### Architecture principle
- Exchange sockets, REST clients, order-book engine, and signing live in the **main process**; the **renderer only displays** and receives normalized snapshots/diffs over IPC. Secrets never reach the renderer.
- Existing C# core is treated as a **reference for behaviour, not code to port** — it will be reviewed and largely rewritten.

### Feature scope
**Phase 1 (MVP):** main-window ticker; IR trading window; volume-sentiment → Slack status + Nanoleaf lights; network fees; secure secret storage.
**Phase 2:** market baiter (redesigned with explicit order-state tracking); balance dashboard; Kraken; (maybe Telegram).
**Phase 3:** spread-over-time charts + spread logging; Slack balance reports; Telegram (if not P2).
**Dropped:** Coinbase trading; BlinkStick (→ **Nanoleaf** light panels; a C# POC exists to reference); Bitaroo; the old debug order-book window (`OBview`).

---

## Main window — DESIGNED ✅

### Hard constraints
- **Compact** — roughly the current window size; fits the user's existing screen layout. Not a full-screen dashboard. "Concise" is a hard rule.
- Fast, frictionless currency switching (the old 5-second pane-switch friction is gone).

### Layout (top to bottom)
1. **Header bar**
   - **Secondary-currency buttons:** `AUD · NZD · SGD · USD`. Click to switch the whole table instantly (replaces the old cycle-through-panes model and the 3 separate panes). Switching **collapses** any expanded row.
   - **Volume-sentiment indicator** (e.g. 🥳): the *result* of the IR-vs-BTCM (later +Kraken) 24h-volume comparison. This same signal drives the Slack status and the Nanoleaf lights. Raw BTCM/Kraken figures are **not** shown here — they live on a separate switchable "Exchanges" panel.
2. **Network-fee strip:** BTC (sat/vB) and ETH (gwei), from mempool/etherscan-style sources.
3. **Ticker table:** one row per IR crypto for the selected secondary currency (~42 pairs). Rows are **taller** than the old app (space freed by dropping other exchanges).

### Row anatomy (left → right)
| Col | Content | Source |
|---|---|---|
| **Pair** | Crypto ticker (e.g. XBT) | — |
| **Trend** | Session-accumulated **mid-price sparkline** (flex column) | session price history |
| **Mid** | `(bestBid + bestOffer) / 2`, **coloured by momentum** | live order book |
| **Spread** | In **bps** | live order book |
| **Vol** | 24h volume | IR market summary (REST) |
| **Day range** | Bar with a marker showing where mid sits between today's low & high | `DayLowestPrice`/`DayHighestPrice` (REST) |

### Sparkline behaviour
- Built **over the session** from mid-price points (in-memory in Phase 1; persisting across sessions to seed it faster is a later nicety).
- **Fixed time-scale, left-anchored, grows rightward** as data accumulates. ~1 hour of data fills the column; after that it's a rolling 1-hour window.
- **Not stretched/distorted** — a pair with 15 min of data occupies ~¼ of the column; `<5 min` shows "gathering…".

### Momentum colour logic (replicate old `Utilities.PriceColour`, IRTicker/Utilities.cs:156)
- Compare current mid to the mid **~5 minutes ago**.
- Requires **≥5 points spanning ≥5 min**, else **neutral**.
- `≥ +1%` → **bright green** (pumping) · `+0.5–1%` → **pale green** · within **±0.5%** → **neutral** · `−0.5–1%` → **pale red** · `≤ −1%` → **bright red** (dumping).
- **Neutral = default theme text colour** (not literal black) so it works in light and dark. Most rows are neutral most of the time — colour only signals a real move.

### Row expand (accordion)
- Click a row → expands **inline** with a smooth animation; rows below shuffle down; **one open at a time**; click again (or switch currency) to collapse.
- The open row hides its columns; the detail panel shows:
  - Large mid readout + bid / ask + spread + 24h vol.
  - The **same crypto's price in the other 3 secondary currencies** (IR returns all fiats in one subscription).
  - A **side-by-side bid/ask depth ladder** (bids left, asks right, pyramid shape) — a **preview**; full-precision depth lives in the trading window.
  - (Depth-imbalance bid/ask liquidity can also live here.)

### Secondary "Exchanges" panel
- A switchable/secondary view (not the main page) showing **BTCM** (and later **Kraken**) data. Feeds the volume comparison that produces the sentiment indicator.

### Open sub-decisions (minor, defer)
- Exact colour shades to be tuned for light/dark contrast.
- Depth-imbalance in the row vs only in the expanded detail (leaning: detail).
- Persisting session price history across restarts (later phase).

---

## Trading window — DESIGNED ✅  *(order-entry pixel layout deferred to the baiter, Phase 2)*

Separate window (lives on its own virtual desktop). **Taller, not wider** — tight width (~350px), tall (~800–1000px of height available); the vertical space goes to a deep order book.

**Behaviour (locked; layout of the entry controls to be finalised with the baiter):**
- **Account** selector — **unlimited** stored accounts (secure storage). **Selecting an account clears the fiat**, forcing a re-pick (prevents trading against the wrong fiat after an account switch — a real bug today).
- **Crypto + fiat** selectors. **Order type defaults to Limit** (Market available).
- **BUY / SELL** — large colour-coded toggle (green / red) that recolours the whole order-entry area. Direction must be unmistakable (wrong-direction trade is the scary failure).
- **Price + Volume** — grouped, sized inputs (not full-width). Volume is always the **primary** (crypto).
- **Order book** — shows the side you'd trade against; columns price / size / **cumulative**. Setting the **limit price highlights crossable orders** (better than the limit) in green. **Double-click a level → volume = cumulative depth to that level** (limit price untouched). Tall + scrollable.
- **Sweep** button — sets volume to **all crossable orders inside the limit** and executes; must be **callable programmatically by the baiter**.
- **Volume validation** — field goes **red** on insufficient balance (buy: order fiat value > fiat balance; sell: volume > crypto balance) or insufficient book depth.
- **Max** button — **Sell only** (= crypto balance). **Hidden on Buy** (fiat ÷ price is fragile to a price uptick).
- **Place button** dynamic text: **"Place limit buy/sell"** (rests) vs amber **"Marketable buy/sell — crosses"** (takes liquidity). Market type → "Buy/Sell now"; baiter → "Start baitin'". **Spacebar confirms.**
- **Colour semantics:** red = real error (insufficient balance); **amber = will cross / take liquidity** (usually intentional in the sweep flow, so not alarm-red).
- **Balances** — compact scrollable pane; the **two active-trade currencies pinned to the top**. Deposit address removed. Fee shown as a one-liner.
- **Baiter (Phase 2)** — space reserved below the order book; adds its own controls (price ceiling, size, start/stop). The precise arrangement of the entry controls is deliberately deferred until then.

---

## Average-price tracker — DESIGNED ✅

A **tabbed single window** — multiple trackers as tabs (`＋` to add), each independent. (Supersedes the earlier "multiple separate windows" idea.)

- **Per tab:** a free-text **Comment** (also serves as the tab label); crypto/fiat pair; **Buy/Sell direction**, colour-coded green/red like the trade window.
- **Start / End date-time** — **time is the primary field** (prominent); the date is small/secondary (rarely changed — trackers seldom run more than ~an hour). **Start defaults to now** on open / new tab. End can be **"ongoing"** (keeps counting live fills).
- **Target total** — settable in **either crypto or secondary** (unit toggle); target / remaining / progress all switch currency with the toggle.
- **Results:** Traded shown in **both** crypto and fiat; volume-weighted **avg price**; **progress bar colour-coded** — blue (going, <100%), green (done, =100%), **red (OVER, >100%, shows overshoot)**; remaining in both units.
- **Copy icons** on fiat-traded, avg price, and both remaining values (raw number, for pasting into other systems).
- **Refresh** button (manual re-pull) alongside live auto-update.
- **"Send remaining → trade volume"** — pushes the outstanding amount into the trading window's volume field.
- *Optional later:* a per-fill list to audit which trades are counted.

---

## Still to design (next sessions)

- [ ] **Exchanges / volume-comparison panel** (BTCM + Kraken; the sentiment computation).
- [ ] **Nanoleaf integration** (reference the existing C# POC).
- [ ] **Slack status/name integration.**
- [ ] **Architecture & data model** — exchange adapter interface, order-book engine + nonce/buffer resync (rewritten cleanly), main↔renderer IPC shape, secure secret storage.
- [ ] **Phase 2/3 surfaces** — baiter, balance dashboard, Kraken, Telegram, spread charts.

---

*Next step per brainstorming: design the remaining Phase-1 surfaces (Slack/volume-sentiment, Nanoleaf) and the architecture/data-model, then transition to a per-phase implementation plan (writing-plans) once Phase 1 design is complete.*
