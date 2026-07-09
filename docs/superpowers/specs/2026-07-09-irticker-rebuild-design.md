# IRTicker Rebuild — Design Spec (living document)

**Started:** 2026-07-09
**Status:** In progress — *Main window designed and approved. Other surfaces still to design.*
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

## Still to design (next sessions)

- [ ] **Trading window** (separate window; most-used surface; full-precision order-book depth; market/limit orders; where average-fill-price lives — integrated vs separate).
- [ ] **Exchanges / volume-comparison panel** (BTCM + Kraken; the sentiment computation).
- [ ] **Nanoleaf integration** (reference the existing C# POC).
- [ ] **Slack status/name integration.**
- [ ] **Architecture & data model** — exchange adapter interface, order-book engine + nonce/buffer resync (rewritten cleanly), main↔renderer IPC shape, secure secret storage.
- [ ] **Phase 2/3 surfaces** — baiter, balance dashboard, Kraken, Telegram, spread charts.

---

*Next step per brainstorming: design the trading window, then the architecture/data-model, then transition to a per-phase implementation plan (writing-plans) once Phase 1 design is complete.*
