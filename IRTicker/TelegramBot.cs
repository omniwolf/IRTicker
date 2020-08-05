using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using System.Diagnostics;
using System.Windows.Controls;
using IndependentReserve.DotNetClientApi.Data;
using System.Xml.XPath;
using System.Runtime.Remoting;
using System.Collections.Concurrent;
using Telegram.Bot.Requests;
using System.IO.Ports;
using System.Reactive.Linq;
using Telegram.Bot.Types.ReplyMarkups;

namespace IRTicker
{
    public class TelegramBot
    {

        PrivateIR pIR;
        DCE DCE_IR;
        IRTicker IRT;

        static TelegramBotClient botClient;
        TelegramState TGstate;

        public ConcurrentDictionary<string, bool> closedOrdersFirstRun = new ConcurrentDictionary<string, bool>();
        private ConcurrentDictionary<string, List<Guid>> notifiedOrders = new ConcurrentDictionary<string, List<Guid>>();

        public TelegramBot(PrivateIR _pIR, DCE _DCE_IR, IRTicker _IRT) {

            DCE_IR = _DCE_IR;
            pIR = _pIR;
            IRT = _IRT;

            TGstate = new TelegramState(this);
            Debug.Print("starting tgbot with: " + Properties.Settings.Default.TelegramAPIToken);
            botClient = new TelegramBotClient(Properties.Settings.Default.TelegramAPIToken);

            botClient.OnMessage += Bot_OnMessage;
            botClient.StartReceiving();

            /*botClient.SendTextMessageAsync("aoeu", "hi", Telegram.Bot.Types.Enums.ParseMode.Default, false, false, 0, Buttons(), );
            botClient.EditMessageReplyMarkupAsync()*/

            if (Properties.Settings.Default.TelegramChatID != 0) {
                TGstate.authStage = 2;
                TGstate.ResetMenu();
            }
        }

        public void NewClient() {
            ResetBot();
            botClient = new TelegramBotClient(Properties.Settings.Default.TelegramAPIToken);
            botClient.OnMessage += Bot_OnMessage;
            botClient.StartReceiving();
        }

        public void ResetBot() {
            TGstate.authStage = 0;
            Properties.Settings.Default.TelegramChatID = 0;
            TGstate.commandChosen = CommandChosen.Nothing;
            Properties.Settings.Default.Save();
        }

        public void StopBot() {
            botClient.StopReceiving();
        }

        public static InlineKeyboardMarkup Buttons() {
            return new InlineKeyboardMarkup(
                new InlineKeyboardButton[][] {
                    new InlineKeyboardButton[] {
                        InlineKeyboardButton.WithCallbackData(""),
                        InlineKeyboardButton.WithCallbackData("")
                    },
                    new InlineKeyboardButton[] {
                        InlineKeyboardButton.WithCallbackData(""),
                        InlineKeyboardButton.WithCallbackData("")},
                    }
                );
         }

        async void Bot_OnMessage(object sender, MessageEventArgs e) {

            if (pIR == null) return;  // don't try anything if we don't have a legit account to login to

            if (e.Message.Text != null) {
                Console.WriteLine($"Received a text message in chat {e.Message.Chat.Id}. - " + e.Message.Text);

                // before we go through too much, let's just check if they're quitting
                if (TGstate.commandChosen != CommandChosen.Nothing) {
                    if (e.Message.Text.ToLower() == "quit") {
                        TGstate.ResetMenu();
                        return;
                    }
                }

                switch (TGstate.commandChosen) {

                    case CommandChosen.Nothing:
                        if (TGstate.authStage == 0) {
                            Properties.Settings.Default.TelegramChatID = 0;
                            await botClient.SendTextMessageAsync(e.Message.Chat.Id, "Hello " + e.Message.Chat.FirstName + ", let's authenticate you.  Please enter your secret code. 🔐", Telegram.Bot.Types.Enums.ParseMode.Default);
                            TGstate.authStage = 1;
                        }
                        else if (TGstate.authStage == 1) {
                            if (e.Message.Text == Properties.Settings.Default.TelegramCode) {
                                Properties.Settings.Default.TelegramChatID = e.Message.Chat.Id;
                                Properties.Settings.Default.Save();
                                TGstate.authStage = 2;
                                await SendMessage("Code accepted, you are now authenticated! ✅");
                                TGstate.ResetMenu();
                            }
                            else {
                                await botClient.SendTextMessageAsync(e.Message.Chat.Id, "⚠️ Code NOT accepted ⚠️");
                                TGstate.authStage = 0;
                            }
                        }
                        else if (TGstate.authStage == 2) {

                            if (e.Message.Chat.Id != Properties.Settings.Default.TelegramChatID) {
                                Debug.Print("TGBot: intercepted a message from an unrecognised chat!  ID: " + e.Message.Chat.Id);
                                await botClient.SendTextMessageAsync(e.Message.Chat.Id, "Hi, someone else has already authenticated with this bot.  If you're in control of IR Ticker, reset the Telegram settings in IR Ticker to re-autheticate.");
                                return;
                            }

                            switch (e.Message.Text.ToLower()) {
                                case "help":
                                    SendMessage("ℹ️ *Command List*:" + Environment.NewLine + Environment.NewLine +
                                        "*Forget*             => Will unauthenticate the bot" + Environment.NewLine +
                                        "*Summary <fiat>*   => Market price summary (eg 'summary aud')" + Environment.NewLine + 
                                        "*Market order*    => Place a market order" + Environment.NewLine +
                                        "*Cancel*            => Cancel an open order" + Environment.NewLine +
                                        "*Baiter*             => Market baiter menu" + Environment.NewLine +
                                        "*Account*         => Currency balances" + Environment.NewLine + Environment.NewLine +
                                        "💡 If unique, the first word can be used instead of the whole command, eg 'market'" + Environment.NewLine +
                                        "💡 When entering a pair, you can be lazy and enter 'btc aud' or 'btcaud' rather than 'BTC-AUD'" + Environment.NewLine +
                                        "💡 At any time type 'quit' to return to the top menu");
                                    break;

                                case "forget":
                                    await SendMessage("I have forgotten you.  You will need to reauthenticate.  To do so send any message.");
                                    ResetBot();
                                    break;

                                case "market":
                                case "market order":
                                    SendMessage("`Market Order` :: ❓ Specify which pair (eg BTC-AUD):");
                                    TGstate.commandChosen = CommandChosen.MarketOrder;
                                    TGstate.commandSubStage = 1;
                                    break;

                                case "cancel":
                                    TGstate.commandChosen = CommandChosen.CancelOrder;
                                    SendMessage("`Cancel Order` :: ❓ Specify which pair (eg BTC-AUD):");
                                    TGstate.commandSubStage = 1;
                                    break;

                                case "summary":
                                case "summary aud":
                                    MarketSummary_Sub("AUD");
                                    break;

                                case "summary usd":
                                    MarketSummary_Sub("USD");
                                    break;

                                case "summary nzd":
                                    MarketSummary_Sub("NZD");
                                    break;

                                case "bait":
                                case "baiter":
                                    if (pIR.marketBaiterActive) {
                                        SendMessage("`Market Baiter` :: ⬆️ Market baiter is currently *active*" + Environment.NewLine +
                                            "  ❓ Do you wish to stop it? (y/n)");
                                        TGstate.commandSubStage = 1;
                                        TGstate.commandChosen = CommandChosen.StopMarketBaiter;
                                    }
                                    else {
                                        await SendMessage("`Market Baiter` :: ⬇️ Market baiter is currently *inactive*.");
                                        TGstate.ResetMenu();
                                    }
                                    break;

                                case "account":
                                    Dictionary<string, Account> accounts;
                                    try {
                                        accounts = pIR.GetAccounts();
                                    }
                                    catch (Exception ex) {
                                        Debug.Print("TGBot: failed to pull accounts: " + ex.Message);
                                        await SendMessage("`Account Balance` :: ⚠️ Failed to pull account data, the following is the most recent account data.");
                                        accounts = pIR.accounts;
                                    }
                                    if (accounts.Count > 0) {
                                        await SendMessage("`Account Balance` :: Crypto and fiat currencies shown below");
                                        string masterStr = "------------------------------" + Environment.NewLine;
                                        foreach (KeyValuePair<string, Account> account in accounts) {
                                            masterStr += "  *" + account.Key.ToUpper() + "*: " + account.Value.AvailableBalance + Environment.NewLine;
                                        }
                                        masterStr += "------------------------------";
                                        await SendMessage(masterStr);
                                    }
                                    else {
                                        await SendMessage("`Account Balance` :: ⚠️ No account balance data available");
                                    }
                                    TGstate.ResetMenu();
                                    break;

                                default:
                                    TGstate.ResetMenu();  // sends the top menu message
                                    break;
                            }
                        }
                        break;

                    case CommandChosen.MarketOrder:

                        if (TGstate.commandSubStage == 1) { // picking the pair
                            Tuple<string, string> pairTup = verifyChosenPair(e.Message.Text, "Market Order");
                            if (pairTup.Item1 != "") {
                                TGstate.ChosenPair = pairTup;
                                SendMessage("`Market Order` :: ❓ " + pairTup.Item1 + "-" + pairTup.Item2 + " chosen.  Is this a buy or a sell? (b/s)");
                                TGstate.commandSubStage = 2;
                            }  // no need for an else clause here, the verifychosenPair() sub handles it
                        }
                        else if (TGstate.commandSubStage == 2) {
                            if (e.Message.Text.ToLower() == "b") {
                                Dictionary<string, DCE.MarketSummary> mSummaries = DCE_IR.GetCryptoPairs();
                                decimal bestOffer = -1;
                                // if they have chosen a fiat we're not currently pulling, let's grab it manually.
                                if (!mSummaries.ContainsKey(TGstate.ChosenPair.Item1 + "-" + TGstate.ChosenPair.Item2) ||
                                    DCE_IR.CurrentSecondaryCurrency != TGstate.ChosenPair.Item2) {
                                    IRT.ParseDCE_IR(TGstate.ChosenPair.Item1, TGstate.ChosenPair.Item2, false);
                                    bestOffer = DCE_IR.GetCryptoPairs()[TGstate.ChosenPair.Item1 + "-" + TGstate.ChosenPair.Item2].CurrentLowestOfferPrice;
                                }
                                else {
                                    bestOffer = mSummaries[TGstate.ChosenPair.Item1 + "-" + TGstate.ChosenPair.Item2].CurrentLowestOfferPrice;
                                }

                                SendMessage("`Market Order` :: BUY order chosen. Current best bid is: $" + Utilities.FormatValue(bestOffer) + Environment.NewLine + Environment.NewLine +
                                    "  ❓ How much " + TGstate.ChosenPair.Item1 + " do you want to buy?");

                                TGstate.commandSubStage = 30;
                            }
                            else if (e.Message.Text.ToLower() == "s") {
                                Dictionary<string, DCE.MarketSummary> mSummaries = DCE_IR.GetCryptoPairs();
                                decimal bestBid = -1;
                                // if they have chosen a fiat we're not currently pulling, let's grab it manually.
                                if (!mSummaries.ContainsKey(TGstate.ChosenPair.Item1 + "-" + TGstate.ChosenPair.Item2) ||
                                    DCE_IR.CurrentSecondaryCurrency != TGstate.ChosenPair.Item2) {
                                    IRT.ParseDCE_IR(TGstate.ChosenPair.Item1, TGstate.ChosenPair.Item2, false);
                                    bestBid = DCE_IR.GetCryptoPairs()[TGstate.ChosenPair.Item1 + "-" + TGstate.ChosenPair.Item2].CurrentHighestBidPrice;
                                }
                                else {
                                    bestBid = mSummaries[TGstate.ChosenPair.Item1 + "-" + TGstate.ChosenPair.Item2].CurrentHighestBidPrice;
                                }

                                SendMessage("`Market Order` :: SELL order chosen. Current best bid is: $" + Utilities.FormatValue(bestBid) + Environment.NewLine + Environment.NewLine +
                                    "  ❓ How much " + TGstate.ChosenPair.Item1 + " do you want to sell?");

                                TGstate.commandSubStage = 40;
                            }
                            else {
                                SendMessage("`Market Order` :: ⚠️ Unrecognised command.  Try again or 'quit' to exit");
                            }
                        }
                        else if (TGstate.commandSubStage == 30) {  // 30 is for buy orders, let's parse the vol
                            if (decimal.TryParse(e.Message.Text, out decimal vol)) {

                                // if they have chosen a fiat we're not currently pulling, let's grab it manually.
                                if (DCE_IR.CurrentSecondaryCurrency != TGstate.ChosenPair.Item2) {
                                    IRT.ParseDCE_IR(TGstate.ChosenPair.Item1, TGstate.ChosenPair.Item2, false);
                                }
                                decimal bestOffer = DCE_IR.GetCryptoPairs()[TGstate.ChosenPair.Item1 + "-" + TGstate.ChosenPair.Item2].CurrentLowestOfferPrice;

                                SendMessage("`Market Order` :: You wish to place a market *BUY* order for " + TGstate.ChosenPair.Item1 + "-" + TGstate.ChosenPair.Item2 + " of size " + vol + "." + Environment.NewLine +
                                    "  Current best offer is: $" + Utilities.FormatValue(bestOffer) + Environment.NewLine +
                                    "  Order approx value: $" + Utilities.FormatValue(bestOffer * vol) + Environment.NewLine + Environment.NewLine +
                                    "❓ Do you wish to proceed? (y/n)");
                                TGstate.Volume = vol;
                                TGstate.commandSubStage = 31;
                            }
                            else {
                                SendMessage("`Market Order` :: ⚠️ Invalid volume, please try again or 'quit' to exit.");
                            }
                        }
                        else if (TGstate.commandSubStage == 31) {  // 31 is to place the buy order (or not)
                            if (e.Message.Text.ToLower() == "y") {
                                BankOrder marketO;
                                try {
                                    marketO = pIR.PlaceMarketOrder(TGstate.ChosenPair.Item1, TGstate.ChosenPair.Item2, OrderType.MarketBid, TGstate.Volume);
                                }
                                catch (Exception ex) {
                                    Debug.Print("TGBot: couldn't place the market buy order: " + ex.Message);
                                    await SendMessage("`Market Order` :: ⚠️ Order failed for the following reason:" + Environment.NewLine +
                                        ex.Message + Environment.NewLine + Environment.NewLine +
                                        "Enter 'y' to try again, or anything else to quit");
                                    return;
                                }
                                if ((marketO.Status == OrderStatus.Filled) || (marketO.Status == OrderStatus.Open)) {
                                    await SendMessage("`Market Order` :: ✅ Order placed!");
                                }
                                else {
                                    await SendMessage("`Market Order` :: ⁉️ Order successful, but not filled?  Status: " + marketO.Status);
                                }
                            }
                            else {
                                await SendMessage("`Market Order` :: Order not placed.");
                            }
                            TGstate.ResetMenu();
                        }
                        else if (TGstate.commandSubStage == 40) {  // 40 is for sell orders, let's parse the vol
                            if (decimal.TryParse(e.Message.Text, out decimal vol)) {
                                // if they have chosen a fiat we're not currently pulling, let's grab it manually.
                                if (DCE_IR.CurrentSecondaryCurrency != TGstate.ChosenPair.Item2) {
                                    IRT.ParseDCE_IR(TGstate.ChosenPair.Item1, TGstate.ChosenPair.Item2, false);
                                }
                                decimal bestBid = DCE_IR.GetCryptoPairs()[TGstate.ChosenPair.Item1 + "-" + TGstate.ChosenPair.Item2].CurrentHighestBidPrice;
                                SendMessage("`Market Order` :: You wish to place a market *SELL* order for " + TGstate.ChosenPair.Item1 + "-" + TGstate.ChosenPair.Item2 + " of size " + vol + "." + Environment.NewLine +
                                    "  Current best bid is: $" + Utilities.FormatValue(bestBid) + Environment.NewLine +
                                    "  Order approx value: $" + Utilities.FormatValue(bestBid * vol) + Environment.NewLine + Environment.NewLine +
                                    "❓ Do you wish to proceed? (y/n)");

                                TGstate.Volume = vol;
                                TGstate.commandSubStage = 41;
                            }
                            else {
                                SendMessage("`Market Order` :: ⚠️ Invalid volume, please try again or 'quit' to exit.");
                            }
                        }
                        else if (TGstate.commandSubStage == 41) {  // 41 is to place the sell order (or not)
                            if (e.Message.Text.ToLower() == "y") {
                                BankOrder marketO;
                                try {
                                    marketO = pIR.PlaceMarketOrder(TGstate.ChosenPair.Item1, TGstate.ChosenPair.Item2, OrderType.MarketOffer, TGstate.Volume);
                                }
                                catch (Exception ex) {
                                    Debug.Print("TGBot: couldn't place the market sell order: " + ex.Message);
                                    await SendMessage("`Market Order` :: ⚠️ Order failed for the following reason:" + Environment.NewLine +
                                        ex.Message + Environment.NewLine + Environment.NewLine +
                                        "Enter 'y' to try again, or anything else to quit");
                                    return;
                                }
                                if ((marketO.Status == OrderStatus.Filled) || (marketO.Status == OrderStatus.Open)) {
                                    await SendMessage("`Market Order` :: ✅ Order placed!");
                                }
                                else {
                                    await SendMessage("`Market Order` :: ⁉️ Order successful, but not filled?  Status: " + marketO.Status);
                                }
                            }
                            else {
                                await SendMessage("`Market Order` :: Order not placed.");
                            }
                            TGstate.ResetMenu();
                        }
                        break;

                    case CommandChosen.CancelOrder:

                        if (TGstate.commandSubStage == 1) {
                            Tuple<string, string> pairTup;
                            // if we failed because of nonces, but we have a legit pair, we can retry (if they send 'r')
                            if ((e.Message.Text.ToUpper() == "R") && (!string.IsNullOrEmpty(TGstate.ChosenPair.Item1)) && (!string.IsNullOrEmpty(TGstate.ChosenPair.Item1))) {
                                pairTup = TGstate.ChosenPair;
                            }
                            else {
                                pairTup = verifyChosenPair(e.Message.Text, "Cancel order");
                            }

                            if (!string.IsNullOrEmpty(pairTup.Item1) && !string.IsNullOrEmpty(pairTup.Item2)) {
                                TGstate.ChosenPair = pairTup;
                                // list the open orders
                                Page<BankHistoryOrder> openOs = new Page<BankHistoryOrder>();
                                try {
                                    openOs = pIR.GetOpenOrders(pairTup.Item1, pairTup.Item2);
                                }
                                catch (Exception ex) {
                                    Debug.Print("TGB: failed to pull open orders due to: " + ex.Message);
                                    SendMessage("`Cancel Order` :: ⚠️ Sorry - failed to pull open orders for the following reason:" + Environment.NewLine + 
                                        ex.Message + Environment.NewLine + Environment.NewLine +
                                        "Please enter the pair again, or send 'r' to retry with " + pairTup.Item1 + "-" + pairTup.Item2 + ".");
                                    return;
                                }

                                if (openOs.Data.Count() > 0) {
                                    SendMessage(compileOpenOrders(openOs, pairTup.Item1, pairTup.Item2));
                                    TGstate.commandSubStage = 2;
                                }
                                else {
                                    await SendMessage("`Cancel Order` :: No open orders to cancel for pair " + pairTup.Item1 + "-" + pairTup.Item2 + ".  Exiting cancel order menu.");
                                    TGstate.ResetMenu();
                                }
                            }
                            else {
                                SendMessage("`Cancel Order` :: ⚠️ " + pairTup.Item1 + "-" + pairTup.Item2 + " pair doesn't exist. Try again or 'quit' to exit.");
                            }
                        }

                        else if (TGstate.commandSubStage == 2) {  // 2 is we picking which order to cancel

                            if (int.TryParse(e.Message.Text, out int chosenOrderNumber)) {
                                // check if it's a legit order and cancel it
                                if (TGstate.openOrdersToList.ContainsKey(chosenOrderNumber)) {
                                    // ok, it's legit number
                                    SendMessage("`Cancel Order` :: ❓ Are you sure you wish to cancel order #" + chosenOrderNumber + "? (y/n)");
                                    TGstate.commandSubStage = 3;
                                    TGstate.orderToCancel = chosenOrderNumber;
                                }
                                else {
                                    SendMessage("`Cancel Order` :: ⚠️ Bad choice, try again (or 'quit' to exit).");
                                }
                            }
                            else {
                                SendMessage("`Cancel Order` :: ⚠️ Couldn't parse your answer.  Please try again and just type the number next to the order you want to cancel, nothing else.");
                            }
                        }
                        else if (TGstate.commandSubStage == 3) {  // 3 i confirming the cancel
                            if (e.Message.Text.ToLower() == "y") {
                                BankOrder cancelledOrder;
                                try {
                                    cancelledOrder = pIR.CancelOrder(TGstate.openOrdersToList[TGstate.orderToCancel].OrderGuid.ToString());
                                }
                                catch (Exception ex) {
                                    SendMessage("`Cancel Order` :: ⚠️ Failed to cancel the order.  Enter 'y' to try again, or anything else to quit");
                                    return;
                                }
                                if (cancelledOrder.Status == OrderStatus.Cancelled) {
                                    await SendMessage("`Cancel Order` :: ✅ Order successfully cancelled.");
                                }
                            }
                            else {
                                await SendMessage("`Cancel Order` :: Order NOT cancelled.");
                            }
                            TGstate.ResetMenu();
                        }
                        break;

                    case CommandChosen.StopMarketBaiter:

                         if (TGstate.commandSubStage == 1) {
                            if (e.Message.Text.ToLower() == "y") {
                                pIR.marketBaiterActive = false;
                                await SendMessage("`Market Baiter` :: ✅ Market baiter stopped.");
                            }
                            else {
                                await SendMessage("`Market Baiter` :: Market baiter will continue.");
                            }
                            TGstate.ResetMenu();
                        }

                        break;
                }
            }
        }

        private async void MarketSummary_Sub(string fiat) {

            if (DCE_IR.CurrentSecondaryCurrency != fiat) {
                // need to quick it grab 
                foreach (string crypto in DCE_IR.PrimaryCurrencyList) {
                    IRT.ParseDCE_IR(crypto, fiat, false);
                }
            }

            Dictionary<string, DCE.MarketSummary> mSummaries = DCE_IR.GetCryptoPairs();
            if (mSummaries.Count < 1) {
                await SendMessage("`Market Summary` :: ⁉️ No market data available");
            }
            else {
                await SendMessage("`Market Summary` :: 📊 " + fiat + " market prices:" + Environment.NewLine + Environment.NewLine);
                string masterStr = "------------------------------" + Environment.NewLine;
                foreach (KeyValuePair<string, DCE.MarketSummary> mSummary in mSummaries) {
                    if (mSummary.Value.SecondaryCurrencyCode.ToUpper() == fiat) {
                        masterStr += "  *" + mSummary.Value.PrimaryCurrencyCode + "*: $" + Utilities.FormatValue((mSummary.Value.CurrentLowestOfferPrice + mSummary.Value.CurrentHighestBidPrice) / 2) + Environment.NewLine;
                    }
                }
                masterStr += "------------------------------";
                await SendMessage(masterStr);
                
                TGstate.ResetMenu();
            }
        }

        // verifies and splits a pair sent by the user is legit and exists as a real pair
        private Tuple<string, string> verifyChosenPair(string pair, string subMenu) {
            string crypto = "";
            string fiat = "";

            pair = pair.Trim();

            // let's see if we can massage the string into the right format
            if (pair.IndexOf("-") == -1) {
                if (pair.IndexOf(" ") == -1) {
                    // try and solve for crypto and fiat run together eg BTCAUD
                    if (pair.Length == 6) {  // assuming something like BTCAUD
                        pair = pair.Substring(0, 3) + "-" + pair.Substring(3, 3);
                    }
                    else if (pair.Length == 7) {  // assuming something like USDTAUD
                        pair = pair.Substring(0, 4) + "-" + pair.Substring(4, 3);
                    }
                }
                else {  // ok we have a space, so maybe something liek "BTC AUD"
                    pair = pair.Replace(" ", "-");
                }
            }

            // need to consider whether we sanitise the pair string before sending it to SplitPair(), or we upgrade SplitPair to deal with shitty strings
            Tuple<string, string> pairTup = Utilities.SplitPair(pair.ToUpper());
            if (pairTup.Item1 == "" || pairTup.Item2 == "") {
                SendMessage("`" + subMenu + "` :: ⚠️ Couldn't parse the pair, please try again or 'quit' to exit this menu.");
                return new Tuple<string, string>("", "");
            }

            string pairInternal = pair.ToUpper();
            pairInternal = pairInternal.Replace("BTC", "XBT").ToUpper();

            foreach (string _crypto in DCE_IR.PrimaryCurrencyList) {
                foreach (string _fiat in DCE_IR.SecondaryCurrencyList) {
                    if (pairInternal == _crypto + "-" + _fiat) {
                        // the pair exists
                        crypto = _crypto;
                        fiat = _fiat;
                        break;
                    }
                }
                if (crypto != "") break;
            }
            if (crypto == "" || fiat == "") {
                SendMessage("`" + subMenu + "` :: ⚠️ This pair doesn't exist, please try again or 'quit' to exit this menu.");
                return new Tuple<string, string>("", "");
            }
            return new Tuple<string, string>(crypto, fiat);
        }

        // crypto will be "BTC", not "XBT"
        private string compileOpenOrders(Page<BankHistoryOrder> openOs, string crypto, string fiat) {

            TGstate.openOrdersToList.Clear();
            string masterStr = "`Cancel Order` :: ❓ Please enter the number of the order you wish to cancel." + Environment.NewLine + Environment.NewLine +
                "  *Open " + crypto + "-" + fiat + " orders:*" + Environment.NewLine;

            int count = 1;
            foreach (BankHistoryOrder bho in openOs.Data) {
                masterStr += Environment.NewLine + "  *" + count + "*. " + (bho.OrderType == OrderType.LimitBid ? "Limid bid  " : "Limit offer") +
                    " | Price: $" + Utilities.FormatValue(bho.Price.Value, 2) + Environment.NewLine +
                    "  Original vol: " + crypto + " " + bho.Original.Volume.ToString() +
                    (bho.Outstanding.HasValue ? Environment.NewLine + "  Outstanding vol: " + crypto + " " + bho.Outstanding.Value.ToString() : "") + Environment.NewLine +
                    "  Date created: " + bho.CreatedTimestampUtc.ToLocalTime() + Environment.NewLine;
                TGstate.openOrdersToList.Add(count, bho);
                count++;
            }
            masterStr += Environment.NewLine + "  Type 'quit' to exit this menu.";
            return masterStr;
        }

        public async Task SendMessage(string message, Telegram.Bot.Types.Enums.ParseMode pMode = Telegram.Bot.Types.Enums.ParseMode.MarkdownV2) {
            if ((Properties.Settings.Default.TelegramChatID == 0) || (TGstate.authStage != 2)) {
                Debug.Print("TG: no chat ID or user not authenticated, cannot send message: " + message);
                return;
            }

            if (pMode == Telegram.Bot.Types.Enums.ParseMode.MarkdownV2) {
                message = message.Replace("-", "\\-").Replace(".", "\\.").Replace("(", "\\(").Replace(")", "\\)").Replace("=", "\\=")
                    .Replace(">", "\\>").Replace("!", "\\!").Replace("|", "\\|").Replace("#", "\\#").Replace("XBT", "BTC");
            }

            await botClient.SendTextMessageAsync(Properties.Settings.Default.TelegramChatID, message, pMode);
        }

        public async void closedOrders(Page<BankHistoryOrder> cOrders) {

            if (cOrders.Data.Count() > 0) {
                string pair = cOrders.Data.First().PrimaryCurrencyCode + "-" + cOrders.Data.First().SecondaryCurrencyCode;
                List<BankHistoryOrder> ordersToNotify = new List<BankHistoryOrder>();

                if (!notifiedOrders.ContainsKey(pair)) notifiedOrders.TryAdd(pair, new List<Guid>());
                if (!closedOrdersFirstRun.ContainsKey(pair)) closedOrdersFirstRun.TryAdd(pair, true);

                foreach (BankHistoryOrder cOrder in cOrders.Data) {
                    if (!notifiedOrders[pair].Contains(cOrder.OrderGuid)) {
                        ordersToNotify.Add(cOrder);
                        notifiedOrders[pair].Add(cOrder.OrderGuid);  // persistent for the session
                    }
                }

                if (!closedOrdersFirstRun[pair]) {
                    // send message..
                    foreach (BankHistoryOrder cOrder in ordersToNotify) {
                        if (cOrder.Status == OrderStatus.Filled) {
                            string crypto = cOrder.PrimaryCurrencyCode.ToString().ToUpper();
                            await SendMessage("*Order Filled!* 🤝" + Environment.NewLine +
                                "  Pair: " + crypto + "-" + cOrder.SecondaryCurrencyCode.ToString().ToUpper() + Environment.NewLine +
                                "  Value: $" + Utilities.FormatValue(cOrder.Value.Value, 2) + Environment.NewLine +
                                "  Avg price: $" + Utilities.FormatValue(cOrder.AvgPrice.Value, 2) + Environment.NewLine +
                                "  Volume: " + crypto + ": " + cOrder.Volume.ToString() + Environment.NewLine +
                                "  Order created: " + cOrder.CreatedTimestampUtc.ToLocalTime());
                        }
                    }
                }
                else closedOrdersFirstRun[pair] = false;
            }


        }

        enum CommandChosen {
            Forget, CancelOrder, Nothing, StopMarketBaiter, MarketOrder
        }

        // this class holds the state of the bot between commands
        class TelegramState {

            TelegramBot TGBot;

            public int authStage = 0;  // 0 = not authed, 1 = awaiting code, 2 = code accepted
            public CommandChosen commandChosen = CommandChosen.Nothing;
            public int commandSubStage = 0;
            public Dictionary<int, BankHistoryOrder> openOrdersToList = new Dictionary<int, BankHistoryOrder>();
            public int orderToCancel = 0;
            public Tuple<string, string> ChosenPair { get; set; }
            public decimal Volume { get; set; }

            public TelegramState(TelegramBot _TGBot) {
                TGBot = _TGBot;
            }

            public void ResetMenu() {
                commandChosen = CommandChosen.Nothing;
                commandSubStage = 0;
                orderToCancel = 0;
                openOrdersToList.Clear();
                ChosenPair = new Tuple<string, string>("", "");
                Volume = -1;
                TGBot.SendMessage("*IR Ticker TelegramBot Top Menu*" + Environment.NewLine +
                    "Please enter your command, or 'help' for a list of commands.");
            }
        }
    }
}
