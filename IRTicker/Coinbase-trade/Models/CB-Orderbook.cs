using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static IRTicker.CBWebSockets;

namespace IRTicker.Coinbase_trade.Models {
    internal class CB_Orderbook {
        // Bids stored in descending order by price
        private readonly SortedDictionary<decimal, decimal> _bids = new SortedDictionary<decimal, decimal>(new DescendingComparer<decimal>());
        // Asks stored in ascending order by price
        private readonly SortedDictionary<decimal, decimal> _asks = new SortedDictionary<decimal, decimal>();

        public IEnumerable<CB_OrderBookEntry> Bids => _bids.Select(kv => new CB_OrderBookEntry { Price = kv.Key, Size = kv.Value });//.ToList();
        public IEnumerable<CB_OrderBookEntry> Asks => _asks.Select(kv => new CB_OrderBookEntry { Price = kv.Key, Size = kv.Value });//.ToList();

        /**
         * this is tricky.  the _bids and _asks properties of the OrderBook class are automatically sorted
         * So, by simply adding the price/size object (OrderBookEntry) to the _bids and _offers property,
         * they get sorted in the correct order.
         */
        public void LoadSnapshot(IEnumerable<CB_OrderBookEntry> bids, IEnumerable<CB_OrderBookEntry> asks) {
            _bids.Clear();
            _asks.Clear();
            foreach (var b in bids) _bids[b.Price] = b.Size;
            foreach (var a in asks) _asks[a.Price] = a.Size;
        }

        public void UpdateFromL2Update(string side, decimal price, decimal size) {
            if (side == "buy") {
                if (size == 0m) {
                    //Debug.Print("CB-trade - removing entry from _bids, price: " + price);
                    _bids.Remove(price);
                }
                else
                    _bids[price] = size;
            }
            else if (side == "sell") {
                if (size == 0m) {
                    //Debug.Print("CB-trade - removing entry from _asks, price: " + price);
                    _asks.Remove(price);
                }
                else
                    _asks[price] = size;
            }
        }
    }

    internal class CB_OrderBookEntry {
        public decimal Price { get; set; }
        public decimal Size { get; set; }
    }
}
