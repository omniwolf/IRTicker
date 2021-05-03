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
using Telegram.Bot.Extensions.Polling;
using System.Threading;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;

namespace IRTicker
{
    public class TelegramBot
    {

        PrivateIR pIR;
        DCE DCE_IR;
        IRTicker IRT;

        static ITelegramBotClient botClient;
        TelegramState TGstate;
        int LatestMessageID;
        string LastMessage = "";  // need to track our previous message so we don't try and edit a message with the same message
        bool NextMsgIsNew = false;  // set to true to disable the edit functionality for the next message (eg when an async message has come through like the order filled message)
        private string bTCMemoji = "";
        private string APIKey;  // try to track which APIkey we're using so we know which closed orders we're pulling
        private string mBaitSpinner = "/";  // we spin this in the market baiter tg bot screen on each refresh to show the refresh is working

        public ConcurrentDictionary<string, bool> closedOrdersFirstRun = new ConcurrentDictionary<string, bool>();
        public ConcurrentDictionary<string, List<Guid>> notifiedOrders = new ConcurrentDictionary<string, List<Guid>>();

        public TelegramBot(string TGAPIKey, PrivateIR _pIR, DCE _DCE_IR, IRTicker _IRT) {

            DCE_IR = _DCE_IR;
            pIR = _pIR;
            IRT = _IRT;

            TGstate = new TelegramState(this);
            Debug.Print("starting tgbot with: " + TGAPIKey);

            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;

            botClient = new TelegramBotClient(TGAPIKey);

            //botClient.OnMessage += Bot_OnMessage;
            //botClient.OnCallbackQuery += OnCallbackQueryReceived;
            botClient.StartReceiving(new DefaultUpdateHandler(HandleUpdateAsync, HandleErrorAsync), cancellationToken);

            //botClient.EditMessageReplyMarkupAsync()*/

            if (Properties.Settings.Default.TelegramChatID != 0) {
                TGstate.authStage = 2;
                TGstate.ResetMenu();
            }
        }

        public void NewClient(string newToken) {
            ResetBot();
            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;

            botClient = new TelegramBotClient(newToken);
            //botClient.OnMessage += Bot_OnMessage;
            botClient.StartReceiving(new DefaultUpdateHandler(HandleUpdateAsync, HandleErrorAsync), cancellationToken);
            TGstate.ResetMenu();
        }

        public void ResetBot() {
            TGstate.authStage = 0;
            Properties.Settings.Default.TelegramChatID = 0;
            TGstate.commandChosen = CommandChosen.Nothing;
            Properties.Settings.Default.Save();
        }

        public void StopBot() {
            if (botClient.IsReceiving) {
                try {
                    botClient.StopReceiving();
                }
                catch (Exception ex) {
                    Debug.Print("stopping TG bot didn't work very well.  error: " + ex.Message);
                }
            }
        }

        // this thing runs through all IR pairs and pulls all orders for the pair to the notifiedOrders dictionary
        // so that we have a list of closed orders to compare when a new one comes in
        public async Task populateClosedOrders() {
            // now we pull all closed orders for all pairs to ensure we have all order guids listed in the TG Bot notifiedOrders dictionary

            Page<BankHistoryOrder> cOrders;

            foreach (string primaryCode in DCE_IR.PrimaryCurrencyList) {
                foreach (string secondaryCode in DCE_IR.SecondaryCurrencyList) {
                    try {
                        cOrders = pIR.GetClosedOrders(primaryCode, secondaryCode, true);  // grab the closed orders on a schedule, this way we will know if an order has been filled and can alert.

                        // need to go if the current primary/secondary is what's shown on IRAccounts, then draw it
                        if ((pIR.SelectedCrypto == primaryCode) && (DCE_IR.CurrentSecondaryCurrency == secondaryCode)) {
                            IRT.drawClosedOrders(cOrders.Data);
                        }
                        // IRT.drawClosedOrders(cOrders);
                    }
                    catch (Exception ex) {
                        string errorMsg = ex.Message;
                        if (ex.InnerException != null) {
                            errorMsg = ex.InnerException.Message;
                        }
                        Debug.Print(DateTime.Now + " - PrivateIR_init sub, trying to pull closed orders for " + primaryCode + "-" + secondaryCode + ", but it failed: " + errorMsg);
                    }
                }
            }
            pIR.firstClosedOrdersPullDone = true;
        }

        public string BTCMemoji {
            get {
                return bTCMemoji;
            }
            set {
                bTCMemoji = value;
                BTCMemojiLastSet = DateTime.Now;
            }
        }

        private DateTime BTCMemojiLastSet { get; set; }

        public static InlineKeyboardMarkup MainMenuButtons() {
            return new InlineKeyboardMarkup(
                new InlineKeyboardButton[][] {
                    new InlineKeyboardButton[] {
                        InlineKeyboardButton.WithCallbackData("Summary AUD", "main-summ"),
                        InlineKeyboardButton.WithCallbackData("Account balances", "main-account")
                    },
                    new InlineKeyboardButton[] {
                        InlineKeyboardButton.WithCallbackData("Market buy BTC/AUD", "main-market-buy-btc"),
                        InlineKeyboardButton.WithCallbackData("View closed orders BTC/AUD", "main-closed-btc")
                    } ,
                    new InlineKeyboardButton[] {
                        InlineKeyboardButton.WithCallbackData("Show order book BTC/AUD", "main-orderbook-btc"),
                        InlineKeyboardButton.WithCallbackData("Market baiter status", "bait-refresh")
                    }
                }
                );
         }

        public static InlineKeyboardMarkup CommonPairsButtons() {
            return new InlineKeyboardMarkup(
                new InlineKeyboardButton[][] {
                    new InlineKeyboardButton[] {
                        InlineKeyboardButton.WithCallbackData("BTC/AUD", "pair-btcaud"),
                        InlineKeyboardButton.WithCallbackData("ETH/AUD", "pair-ethaud")
                    },
                    new InlineKeyboardButton[] {
                        InlineKeyboardButton.WithCallbackData("XRP/AUD", "pair-xrpaud"),
                        InlineKeyboardButton.WithCallbackData("USDT/AUD", "pair-usdtaud"),
                    },
                    new InlineKeyboardButton[] {
                        InlineKeyboardButton.WithCallbackData("Quit to main menu", "quit")
                    },
                }
                );
        }
        public static InlineKeyboardMarkup BuySellButtons() {
            return new InlineKeyboardMarkup(
                new InlineKeyboardButton[][] {
                    new InlineKeyboardButton[] {
                        InlineKeyboardButton.WithCallbackData("BUY", "buy-order"),
                        InlineKeyboardButton.WithCallbackData("SELL", "sell-order")
                    },
                    new InlineKeyboardButton[] {
                        InlineKeyboardButton.WithCallbackData("Quit to main menu", "quit")
                    },
                }
                );
        }

        public static InlineKeyboardMarkup BaitinButtons() {
            return new InlineKeyboardMarkup(
                new InlineKeyboardButton[][] {
                    new InlineKeyboardButton[] {
                        InlineKeyboardButton.WithCallbackData("Stop baitin'", "stop-baitin"),
                        InlineKeyboardButton.WithCallbackData("Cancel order menu", "bait-cancel")
                    },
                    new InlineKeyboardButton[] {
                        InlineKeyboardButton.WithCallbackData("Refresh", "bait-refresh"),
                        InlineKeyboardButton.WithCallbackData("Quit to main menu", "quit")
                    },
                }
                );
        }

        public static InlineKeyboardMarkup YesNoButtons() {
            return new InlineKeyboardMarkup(
                new InlineKeyboardButton[][] {
                    new InlineKeyboardButton[] {
                        InlineKeyboardButton.WithCallbackData("YES", "yes"),
                        InlineKeyboardButton.WithCallbackData("NO", "no") }
                    }
                );
        }

        public static InlineKeyboardMarkup QuitToMain() {
            return new InlineKeyboardMarkup(
                new InlineKeyboardButton[][] {
                    new InlineKeyboardButton[] {
                        InlineKeyboardButton.WithCallbackData("Quit to main menu", "quit") }
                    }
                );
        }

        async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken) {
            if ((update.CallbackQuery != null) && (update.CallbackQuery is CallbackQuery cbq)) {
                try {
                    switch (cbq.Data) {
                        case "quit":
                            TGstate.ResetMenu(true);
                            break;
                        case "main-summ":
                            MarketSummary_Sub("AUD", true);
                            break;
                        case "main-account":
                            AccountBalances_Sub(true);
                            break;
                        case "main-market-buy-btc":
                            TGstate.ChosenPair = new Tuple<string, string>("XBT", "AUD");
                            TGstate.commandChosen = CommandChosen.MarketOrder;
                            TGstate.commandSubStage = 2;
                            MarketBuy_Sub(true);
                            break;
                        case "main-cancel":
                            TGstate.commandChosen = CommandChosen.CancelOrder;
                            SendMessage("`Cancel Order` :: ❓ Specify which pair (eg BTC-AUD):", buttons: CommonPairsButtons(), editMessage: false);
                            TGstate.commandSubStage = 1;
                            break;
                        case "main-closed-btc":
                            TGstate.commandChosen = CommandChosen.ViewClosed;
                            TGstate.commandSubStage = 1;
                            ViewClosed_PairSub("BTC-AUD", true);
                            break;
                        case "main-orderbook-btc":
                            ShowOrderBook(new Tuple<string, string>("XBT", "AUD"));
                            break;
                        case "pair-btcaud":
                            if (TGstate.commandChosen == CommandChosen.MarketOrder) MarketOrder_SubStage1("BTC-AUD", true);
                            else if (TGstate.commandChosen == CommandChosen.CancelOrder) CancelOrder_Sub("BTC-AUD", true);
                            else if (TGstate.commandChosen == CommandChosen.ViewClosed) ViewClosed_PairSub("BTC-AUD", true);
                            else if (TGstate.commandChosen == CommandChosen.ShowOrderBook) ShowOrderBook_Sub("BTC-AUD", true);
                            break;
                        case "pair-ethaud":
                            if (TGstate.commandChosen == CommandChosen.MarketOrder) MarketOrder_SubStage1("ETH-AUD", true);
                            else if (TGstate.commandChosen == CommandChosen.CancelOrder) CancelOrder_Sub("ETH-AUD", true);
                            else if (TGstate.commandChosen == CommandChosen.ViewClosed) ViewClosed_PairSub("ETH-AUD", true);
                            else if (TGstate.commandChosen == CommandChosen.ShowOrderBook) ShowOrderBook_Sub("ETH-AUD", true);
                            break;
                        case "pair-xrpaud":
                            if (TGstate.commandChosen == CommandChosen.MarketOrder) MarketOrder_SubStage1("XRP-AUD", true);
                            else if (TGstate.commandChosen == CommandChosen.CancelOrder) CancelOrder_Sub("XRP-AUD", true);
                            else if (TGstate.commandChosen == CommandChosen.ViewClosed) ViewClosed_PairSub("XRP-AUD", true);
                            else if (TGstate.commandChosen == CommandChosen.ShowOrderBook) ShowOrderBook_Sub("XRP-AUD", true);
                            break;
                        case "pair-usdtaud":
                            if (TGstate.commandChosen == CommandChosen.MarketOrder) MarketOrder_SubStage1("USDT-AUD", true);
                            else if (TGstate.commandChosen == CommandChosen.CancelOrder) CancelOrder_Sub("USDT-AUD", true);
                            else if (TGstate.commandChosen == CommandChosen.ViewClosed) ViewClosed_PairSub("USDT-AUD", true);
                            else if (TGstate.commandChosen == CommandChosen.ShowOrderBook) ShowOrderBook_Sub("USDT-AUD", true);
                            break;
                        case "buy-order":
                            MarketBuy_Sub(true);
                            break;
                        case "sell-order":
                            MarketSell_Sub(true);
                            break;
                        case "view-closed":
                            ViewClosed_Sub();
                            break;
                        case "stop-baitin":
                            pIR.marketBaiterActive = false;
                            await SendMessage("`Market Baiter` :: ✅ Market baiter stopped.", editMessage: true);
                            break;
                        case "bait-refresh":
                            MarketBaiter_Sub(true);
                            break;
                        case "bait-cancel":
                            TGstate.commandChosen = CommandChosen.CancelOrder;
                            CancelOrder_Sub(TGstate.ChosenPair.Item1 + "-" + TGstate.ChosenPair.Item2);
                            break;
                        case "yes":
                            switch (TGstate.commandChosen) {
                                case CommandChosen.MarketOrder:
                                    if ((TGstate.commandSubStage == 31) || (TGstate.commandSubStage == 41)) {  // 31 is to place the buy order (or not), 41 is sell
                                        MarketOrderConfirm_Sub();
                                    }
                                    break;

                                case CommandChosen.CancelOrder:
                                    if (TGstate.commandSubStage == 3) {  // 3 i confirming the cancel
                                        CancelOrderConfirm_Sub();
                                    }
                                    break;
                                case CommandChosen.MarketBaiter:
                                    MarketBaiter_Sub(true);  // mbait is true, but there is no open order
                                    break;
                            }
                            break;
                        case "no":
                            switch (TGstate.commandChosen) {
                                case CommandChosen.MarketOrder:
                                    await SendMessage("`Market Order` :: Order not placed.", editMessage: true);
                                    break;

                                case CommandChosen.CancelOrder:
                                    await SendMessage("`Cancel Order` :: Order NOT cancelled.", editMessage: true);
                                    break;
                            }
                            TGstate.ResetMenu();
                            break;
                    }

                }
                catch (Exception ex) {
                    Debug.Print("TGBot: exception in callback buttons: " + ex.Message);
                }
            }

            else if (update.Message is Message message) {
                if (pIR == null) return;  // don't try anything if we don't have a legit account to login to

                if ((message != null) && (message.Text != null)) {
                    Console.WriteLine($"Received a text message in chat {message.Chat}. - " + message.Text);

                    // before we go through too much, let's just check if they're quitting
                    if (TGstate.commandChosen != CommandChosen.Nothing) {
                        if (message.Text.ToLower() == "quit") {
                            TGstate.ResetMenu();
                            return;
                        }
                    }

                    switch (TGstate.commandChosen) {

                        case CommandChosen.Nothing:
                            if (TGstate.authStage == 0) {
                                Properties.Settings.Default.TelegramChatID = 0;
                                await botClient.SendTextMessageAsync(message.Chat, "Hello " + message.Chat.FirstName + ", let's authenticate you.  Please enter your secret code. 🔐", Telegram.Bot.Types.Enums.ParseMode.Default);
                                TGstate.authStage = 1;
                            }
                            else if (TGstate.authStage == 1) {
                                if (message.Text == Properties.Settings.Default.TelegramCode) {
                                    Properties.Settings.Default.TelegramChatID = message.Chat.Id;
                                    Properties.Settings.Default.Save();
                                    TGstate.authStage = 2;
                                    await SendMessage("Code accepted, you are now authenticated! ✅");
                                    TGstate.ResetMenu();
                                }
                                else {
                                    await botClient.SendTextMessageAsync(message.Chat, "⚠️ Code NOT accepted ⚠️");
                                    TGstate.authStage = 0;
                                }
                            }
                            else if (TGstate.authStage == 2) {

                                if (message.Chat.Id != Properties.Settings.Default.TelegramChatID) {
                                    Debug.Print("TGBot: intercepted a message from an unrecognised chat!  ID: " + message.Chat.Id);
                                    await botClient.SendTextMessageAsync(message.Chat, "Hi, someone else has already authenticated with this bot.  If you're in control of IR Ticker, reset the Telegram settings in IR Ticker to re-autheticate.");
                                    return;
                                }

                                switch (message.Text.ToLower()) {
                                    case "help":
                                        SendMessage("ℹ️ *Command List*:" + Environment.NewLine + Environment.NewLine +
                                            "*Forget*             => Will unauthenticate the bot" + Environment.NewLine +
                                            "*Summary <fiat>*   => Market price summary (eg 'summary aud')" + Environment.NewLine +
                                            "*Market order*    => Place a market order" + Environment.NewLine +
                                            "*Cancel*            => Cancel an open order" + Environment.NewLine +
                                            "*Baiter*             => Market baiter menu" + Environment.NewLine +
                                            "*Account*         => Currency balances" + Environment.NewLine + Environment.NewLine +
                                            "*Closed*         => View closed orders" + Environment.NewLine + Environment.NewLine +
                                            "*Order book*         => Quick view of the order book" + Environment.NewLine + Environment.NewLine +
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
                                        await SendMessage("`Market Order` :: ❓ Specify which pair (eg BTC-AUD):", buttons: CommonPairsButtons());
                                        //botClient.SendTextMessageAsync(Properties.Settings.Default.TelegramChatID, "", Telegram.Bot.Types.Enums.ParseMode.Default, false, false, 0, CommonPairsButtons());
                                        TGstate.commandChosen = CommandChosen.MarketOrder;
                                        TGstate.commandSubStage = 1;
                                        break;

                                    case "cancel":
                                        TGstate.commandChosen = CommandChosen.CancelOrder;
                                        TGstate.commandSubStage = 1;
                                        await SendMessage("`Cancel Order` :: ❓ Specify which pair (eg BTC-AUD):", buttons: CommonPairsButtons());
                                        //botClient.SendTextMessageAsync(Properties.Settings.Default.TelegramChatID, "", Telegram.Bot.Types.Enums.ParseMode.Default, false, false, 0, CommonPairsButtons());
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
                                        MarketBaiter_Sub(false);
                                        break;

                                    case "account":
                                        AccountBalances_Sub();
                                        break;

                                    case "closed":
                                        ViewClosed_Sub();
                                        break;

                                    case "order book":
                                    case "show orders":
                                    case "show":
                                        TGstate.commandChosen = CommandChosen.ShowOrderBook;
                                        TGstate.commandSubStage = 1;
                                        await SendMessage("`Show Order Book` :: ❓ Specify which pair (eg BTC-AUD):", buttons: CommonPairsButtons());
                                        break;

                                    default:
                                        TGstate.ResetMenu();  // sends the top menu message
                                        break;
                                }
                            }
                            break;

                        case CommandChosen.MarketOrder:

                            if (TGstate.commandSubStage == 1) { // picking the pair
                                MarketOrder_SubStage1(message.Text);
                            }
                            else if (TGstate.commandSubStage == 2) {
                                if ((message.Text.ToLower() == "b") || (message.Text.ToLower() == "buy")) {
                                    MarketBuy_Sub();  // we have a sub for this because it gets called by a button too.  the sell side has no button, so we can just code directly into the parent sub
                                }
                                else if ((message.Text.ToLower() == "s") || (message.Text.ToLower() == "sell")) {
                                    MarketSell_Sub();
                                }
                                else {
                                    SendMessage("`Market Order` :: ⚠️ Unrecognised command.  Try again or 'quit' to exit", buttons: BuySellButtons());
                                }
                            }
                            else if (TGstate.commandSubStage == 30) {  // 30 is for buy orders, let's parse the vol
                                if (decimal.TryParse(message.Text, out decimal vol)) {
                                    if (vol <= 0) {
                                        MarketOrder_InvalidVolume();
                                        break;
                                    }

                                    // if they have chosen a fiat we're not currently pulling, let's grab it manually.
                                    if (DCE_IR.CurrentSecondaryCurrency != TGstate.ChosenPair.Item2) {
                                        IRT.ParseDCE_IR(TGstate.ChosenPair.Item1, TGstate.ChosenPair.Item2, false);
                                    }
                                    decimal bestOffer = DCE_IR.GetCryptoPairs()[TGstate.ChosenPair.Item1 + "-" + TGstate.ChosenPair.Item2].CurrentLowestOfferPrice;
                                    string accBal = "";
                                    if (pIR.accounts.ContainsKey(TGstate.ChosenPair.Item2)) {
                                        accBal += "  " + TGstate.ChosenPair.Item2.ToUpper() + " account balance: $" + Utilities.FormatValue(pIR.accounts[TGstate.ChosenPair.Item2].AvailableBalance) + Environment.NewLine;
                                    }

                                    SendMessage("`Market Order` :: You wish to place a market *BUY* order for " + TGstate.ChosenPair.Item1 + "-" + TGstate.ChosenPair.Item2 + " of size "  + TGstate.ChosenPair.Item1 + " " + vol + "." + Environment.NewLine +
                                        "  Current best offer is: $" + Utilities.FormatValue(bestOffer) + Environment.NewLine +
                                        accBal + 
                                        "  Order approx value: $" + Utilities.FormatValue(bestOffer * vol) + Environment.NewLine + Environment.NewLine +
                                        "❓ Do you wish to proceed?", buttons: YesNoButtons());
                                    TGstate.Volume = vol;
                                    TGstate.commandSubStage = 31;
                                }
                                else {
                                    SendMessage("`Market Order` :: ⚠️ Invalid volume, please try again or 'quit' to exit.");
                                }
                            }

                            else if (TGstate.commandSubStage == 40) {  // 40 is for sell orders, let's parse the vol
                                if (decimal.TryParse(message.Text, out decimal vol)) {
                                    if (vol <= 0) {
                                        MarketOrder_InvalidVolume();
                                        break;
                                    }
                                    // if they have chosen a fiat we're not currently pulling, let's grab it manually.
                                    if (DCE_IR.CurrentSecondaryCurrency != TGstate.ChosenPair.Item2) {
                                        IRT.ParseDCE_IR(TGstate.ChosenPair.Item1, TGstate.ChosenPair.Item2, false);
                                    }
                                    decimal bestBid = DCE_IR.GetCryptoPairs()[TGstate.ChosenPair.Item1 + "-" + TGstate.ChosenPair.Item2].CurrentHighestBidPrice;
                                    string accBal = "";
                                    if (pIR.accounts.ContainsKey(TGstate.ChosenPair.Item1)) {  // don't bother pulling the balance again, it's probably the same as when we just pulled it a second ago for the previous question
                                        accBal += "  " + TGstate.ChosenPair.Item1.ToUpper() + " account balance: " + TGstate.ChosenPair.Item1.ToUpper() + " " + Utilities.FormatValue(pIR.accounts[TGstate.ChosenPair.Item1].AvailableBalance) + Environment.NewLine;
                                    }
                                    SendMessage("`Market Order` :: You wish to place a market *SELL* order for " + TGstate.ChosenPair.Item1 + "-" + TGstate.ChosenPair.Item2 + " of size " + TGstate.ChosenPair.Item1 + " " + vol + "." + Environment.NewLine +
                                        "  Current best bid is: $" + Utilities.FormatValue(bestBid) + Environment.NewLine +
                                        accBal +
                                        "  Order approx value: $" + Utilities.FormatValue(bestBid * vol) + Environment.NewLine + Environment.NewLine +
                                        "❓ Do you wish to proceed?", buttons: YesNoButtons());

                                    TGstate.Volume = vol;
                                    TGstate.commandSubStage = 41;
                                }
                                else {  
                                    SendMessage("`Market Order` :: ⚠️ Invalid volume, please try again or 'quit' to exit.");
                                }
                            }

                            else if ((TGstate.commandSubStage == 31) || (TGstate.commandSubStage == 41)) {
                                if ((message.Text.ToUpper() == "YES") || (message.Text.ToUpper() == "Y")) {
                                    MarketOrderConfirm_Sub();
                                }
                                else {
                                    await SendMessage("`Market Order` :: Order not placed.", editMessage: true);
                                    TGstate.ResetMenu();
                                }
                            }

                            break;

                        case CommandChosen.CancelOrder:

                            if (TGstate.commandSubStage == 1) {
                                CancelOrder_Sub(message.Text);
                            }

                            else if (TGstate.commandSubStage == 2) {  // 2 is we picking which order to cancel

                                if (int.TryParse(message.Text, out int chosenOrderNumber)) {
                                    // check if it's a legit order and cancel it
                                    if (TGstate.openOrdersToList.ContainsKey(chosenOrderNumber)) {
                                        // ok, it's legit number
                                        SendMessage("`Cancel Order` :: ❓ Are you sure you wish to cancel order #" + chosenOrderNumber + "?", buttons: YesNoButtons());
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

                            else if (TGstate.commandSubStage == 3) {
                                if ((message.Text.ToUpper() == "YES") || (message.Text.ToUpper() == "Y")) {
                                    CancelOrderConfirm_Sub();
                                }
                                else {
                                    await SendMessage("`Cancel Order` :: Order NOT cancelled.", editMessage: true);
                                    TGstate.ResetMenu();
                                }
                            }
                            break;

                        case CommandChosen.ViewClosed:
                            if (TGstate.commandSubStage == 1) ViewClosed_PairSub(message.Text);
                            break;

                        case CommandChosen.ShowOrderBook:
                            if (TGstate.commandSubStage == 1) {
                                Tuple<string, string> pairTup = verifyChosenPair(message.Text, "Show Order Book");

                                if (!string.IsNullOrEmpty(pairTup.Item1) && !string.IsNullOrEmpty(pairTup.Item2)) {
                                    TGstate.ChosenPair = pairTup;
                                    ShowOrderBook(pairTup);
                                }
                            }
                            break;

                        case CommandChosen.MarketBaiter:
                            if ((message.Text.ToUpper() == "YES") || (message.Text.ToUpper() == "Y") || (message.Text.ToUpper() == "REFRESH") || (message.Text.ToUpper() == "REF")) {
                                MarketBaiter_Sub(true);
                            }
                            else if (message.Text.ToUpper() == "CANCEL") {
                                CancelOrder_Sub(TGstate.ChosenPair.Item1 + "-" + TGstate.ChosenPair.Item2);
                            }
                            else {
                                TGstate.ResetMenu();
                            }
                            break;
                    }
                }
            }
        }

        async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken) {
            if (exception is ApiRequestException apiRequestException) {
                SendMessage("Error: " + apiRequestException.ToString() + Environment.NewLine + "Resetting menu...");
                TGstate.ResetMenu();
            }
        }

        private async void MarketBaiter_Sub(bool editMsg) {
            if (pIR.marketBaiterActive) {
                TGstate.commandChosen = CommandChosen.MarketBaiter;

                decimal price = -1;
                decimal volume = -1;

                switch (mBaitSpinner) {
                    case "/":
                        mBaitSpinner = "-";
                        break;
                    case "-":
                        mBaitSpinner = @"\\\";
                        break;
                    case @"\\\":
                        mBaitSpinner = "|";
                        break;
                    case "|":
                        mBaitSpinner = "/";
                        break;
                }

                string masterStr = "`Market Baiter` :: ⬆️ Market baiter is currently *active* " + mBaitSpinner + Environment.NewLine + Environment.NewLine;

                if (pIR.placedOrder != null) {
                    if (pIR.placedOrder.Price.HasValue) {
                        price = pIR.placedOrder.Price.Value;
                    }
                    volume = pIR.placedOrder.VolumeOrdered;
                    if (pIR.placedOrder.VolumeFilled > 0) volume = pIR.placedOrder.VolumeOrdered - pIR.placedOrder.VolumeFilled;

                    string pair = pIR.placedOrder.PrimaryCurrencyCode.ToString().ToUpper() + "-" + pIR.placedOrder.SecondaryCurrencyCode.ToString().ToUpper();
                    // also store this in the state object in case we want to cancel
                    TGstate.ChosenPair = new Tuple<string, string>(pIR.placedOrder.PrimaryCurrencyCode.ToString().ToUpper(), pIR.placedOrder.SecondaryCurrencyCode.ToString().ToUpper());

                    masterStr += "  Original volume: " + (TGstate.ChosenPair.Item1 == "XBT" ? "BTC" : TGstate.ChosenPair.Item1) + " " + Utilities.FormatValue(volume, 8, false) + Environment.NewLine;
                    if (volume != pIR.baiterLiveVol) {
                        masterStr += "  Remaining volume: " + (TGstate.ChosenPair.Item1 == "XBT" ? "BTC" : TGstate.ChosenPair.Item1) + " " + Utilities.FormatValue(pIR.baiterLiveVol, 8, false) + Environment.NewLine;
                    }
                    masterStr += "  Price: $ " + Utilities.FormatValue(price, 5, false) + Environment.NewLine;

                    // now we find the closest order.. what a mish

                    IOrderedEnumerable <KeyValuePair<decimal, ConcurrentDictionary<string, DCE.OrderBook_IR>>> orderedOrders;
                    KeyValuePair<decimal, ConcurrentDictionary<string, DCE.OrderBook_IR>>[] tempArrayBook;

                    if (pIR.placedOrder.Type == OrderType.LimitBid) {
                        tempArrayBook = DCE_IR.IR_OBs[pair].Item1.ToArray();
                        orderedOrders = tempArrayBook.OrderByDescending(k => k.Key);
                    }
                    else {
                        tempArrayBook = DCE_IR.IR_OBs[pair].Item2.ToArray();
                        orderedOrders = tempArrayBook.OrderBy(k => k.Key);
                    }

                    if (pIR.placedOrder.Price == orderedOrders.ElementAt(0).Key) {
                        decimal nextClosestPrice = orderedOrders.ElementAt(1).Key;
                        masterStr += "  Next closest order is: $ " + Utilities.FormatValue(nextClosestPrice, 5, false);
                    }
                    else {  // our order is not at the top at this moment
                        decimal topOrderPrice = orderedOrders.ElementAt(0).Key;
                        masterStr += "  Baitin' order not at the top, top order is: $ " + Utilities.FormatValue(topOrderPrice, 5, false);
                    }

                    SendMessage(masterStr, buttons: BaitinButtons(), editMessage: editMsg);
                }
                else {  // placedOrder is null
                    masterStr += "  No active order at this point in time.  Retry?";
                    SendMessage(masterStr, buttons: YesNoButtons());
                }
            }
            else {
                await SendMessage("`Market Baiter` :: ⬇️ Market baiter is currently *inactive*.");
                TGstate.ResetMenu();
            }
        }

        private async void MarketOrderConfirm_Sub() {
            string oTypeStr;
            OrderType oType;
            if (TGstate.commandSubStage == 31) {
                oTypeStr = "buy";
                oType = OrderType.MarketBid;
            }
            else if (TGstate.commandSubStage == 41) {
                oTypeStr = "sell";
                oType = OrderType.MarketOffer;
            }
            else return;

            BankOrder marketO;
            try {
                marketO = pIR.PlaceMarketOrder(TGstate.ChosenPair.Item1, TGstate.ChosenPair.Item2, oType, TGstate.Volume);
            }
            catch (Exception ex) {
                string errorMsg = ex.Message;
                if (ex.InnerException != null) {
                    errorMsg = ex.InnerException.Message;
                }
                Debug.Print("TGBot: couldn't place the market " + oTypeStr + " order: " + errorMsg);
                await SendMessage("`Market Order` :: ⚠️ Order failed for the following reason:" + Environment.NewLine +
                    errorMsg + Environment.NewLine + Environment.NewLine +
                    "Try again?", buttons: YesNoButtons(), editMessage: true);
                return;

            }
            if ((marketO.Status == OrderStatus.Filled) || (marketO.Status == OrderStatus.Open)) {
                await SendMessage("`Market Order` :: ✅ Order placed!", editMessage: true);
            }
            else {
                await SendMessage("`Market Order` :: ⁉️ Order successful, but not filled?  Status: " + marketO.Status, editMessage: true);
            }
            TGstate.ResetMenu();
        }

        private async void CancelOrderConfirm_Sub() {
            BankOrder cancelledOrder;
            try {
                cancelledOrder = pIR.CancelOrder(TGstate.openOrdersToList[TGstate.orderToCancel].OrderGuid.ToString());
            }
            catch (Exception ex) {
                string errorMsg = ex.Message;
                if (ex.InnerException != null) {
                    errorMsg = ex.InnerException.Message;
                }
                SendMessage("`Cancel Order` :: ⚠️ Failed to cancel the order.  Failure reason:" + Environment.NewLine +
                    errorMsg + Environment.NewLine + Environment.NewLine +
                    "Try again?", buttons: YesNoButtons(), editMessage: true);
                return;
            }
            if (cancelledOrder.Status == OrderStatus.Cancelled) {
                await SendMessage("`Cancel Order` :: ✅ Order successfully cancelled.", editMessage: true);
                TGstate.ResetMenu();
            }
            else {
                await SendMessage("`Cancel Order` :: ⚠️ Order not cancelled, current status: " + Environment.NewLine +
                    cancelledOrder.Status.ToString() + Environment.NewLine +
                    "Try again?", buttons: YesNoButtons(), editMessage: true);
            }
        }

        private void ViewClosed_Sub() {
            TGstate.commandChosen = CommandChosen.ViewClosed;
            TGstate.commandSubStage = 1;
            SendMessage("`View Closed Orders` :: ❓ Specify which pair (eg BTC-AUD):", buttons: CommonPairsButtons(), editMessage: true);
        }

        private async void ViewClosed_PairSub(string message, bool editMsg = false) {

            Tuple<string, string> pairTup;
            // if we failed because of nonces, but we have a legit pair, we can retry (if they send 'r')
            if ((message.ToUpper() == "R") && (!string.IsNullOrEmpty(TGstate.ChosenPair.Item1)) && (!string.IsNullOrEmpty(TGstate.ChosenPair.Item1))) {
                pairTup = TGstate.ChosenPair;
            }
            else {
                pairTup = verifyChosenPair(message, "View Closed Orders");
            }

            if (!string.IsNullOrEmpty(pairTup.Item1) && !string.IsNullOrEmpty(pairTup.Item2)) {
                TGstate.ChosenPair = pairTup;
                // list the closed orders
                Page<BankHistoryOrder> closedOs;
                try {
                    closedOs = pIR.GetClosedOrders(pairTup.Item1, pairTup.Item2);
                }
                catch (Exception ex) {
                    Debug.Print("TGB: failed to pull closed orders due to: " + ex.Message);
                    SendMessage("`View Closed Orders` :: ⚠️ Sorry - failed to pull open closed for the following reason:" + Environment.NewLine +
                        ex.Message + Environment.NewLine + Environment.NewLine +
                        "Please enter the pair again, or send 'r' to retry with " + pairTup.Item1 + "-" + pairTup.Item2 + ".", editMessage: editMsg);
                    return;
                }

                if (closedOs.Data.Count() > 0) {

                    int count = 1;
                    string masterStr = "`View Closed Orders` :: " + pairTup.Item1.ToUpper() + "-" + pairTup.Item2.ToUpper() + Environment.NewLine;
                    foreach (BankHistoryOrder bho in closedOs.Data) {
                        if (bho.Status == OrderStatus.Cancelled) continue;
                        masterStr += Environment.NewLine + "  *" + count + "*. " + (bho.OrderType == OrderType.LimitBid ? "Limit bid  " : "Limit offer") +
                            " | Price: $" + Utilities.FormatValue(bho.AvgPrice.Value, 2) + Environment.NewLine +
                            "  Vol: " + pairTup.Item1.ToUpper() + " " + (bho.Outstanding.HasValue ? (bho.Volume - bho.Outstanding.Value).ToString() : bho.Volume.ToString()) + Environment.NewLine +
                            ((bho.Outstanding.HasValue && (bho.Outstanding.Value > 0)) ? "  Outstanding vol: " + pairTup.Item1.ToUpper() + " " + bho.Outstanding.Value.ToString() + Environment.NewLine : "") +
                            "  Value: $" + Utilities.FormatValue(bho.Value.Value, 2) + Environment.NewLine +
                            "  Date created: " + bho.CreatedTimestampUtc.ToLocalTime() + Environment.NewLine;
                        TGstate.openOrdersToList.Add(count, bho);
                        count++;
                        if (count > 10) break;
                    }

                    await SendMessage(masterStr, editMessage: editMsg);
                }
                else {
                    await SendMessage("`View Closed Orders` :: No open closed orders to view for " + pairTup.Item1 + "-" + pairTup.Item2 + ".  Exiting view closed orders menu.");
                }
                TGstate.ResetMenu();
            }
        }

        private void ShowOrderBook_Sub (string message, bool editMsg = false) {
            Tuple<string, string> pairTup;
            // if we failed because of nonces, but we have a legit pair, we can retry (if they send 'r')
            if ((message.ToUpper() == "R") && (!string.IsNullOrEmpty(TGstate.ChosenPair.Item1)) && (!string.IsNullOrEmpty(TGstate.ChosenPair.Item1))) {
                pairTup = TGstate.ChosenPair;
            }
            else {
                pairTup = verifyChosenPair(message, "Show Order Book");
            }

            if (!string.IsNullOrEmpty(pairTup.Item1) && !string.IsNullOrEmpty(pairTup.Item2)) {
                TGstate.ChosenPair = pairTup;
                ShowOrderBook(pairTup);
            }
        }

        private async void CancelOrder_Sub(string message, bool editMsg = false) {
            Tuple<string, string> pairTup;
            // if we failed because of nonces, but we have a legit pair, we can retry (if they send 'r')
            if ((message.ToUpper() == "R") && (!string.IsNullOrEmpty(TGstate.ChosenPair.Item1)) && (!string.IsNullOrEmpty(TGstate.ChosenPair.Item1))) {
                pairTup = TGstate.ChosenPair;
            }
            else {
                pairTup = verifyChosenPair(message, "Cancel Order");
            }

            if (!string.IsNullOrEmpty(pairTup.Item1) && !string.IsNullOrEmpty(pairTup.Item2)) {
                TGstate.ChosenPair = pairTup;
                // list the open orders
                Page<BankHistoryOrder> openOs;
                try {
                    openOs = pIR.GetOpenOrders(pairTup.Item1, pairTup.Item2);
                }
                catch (Exception ex) {
                    Debug.Print("TGB: failed to pull open orders due to: " + ex.Message);
                    SendMessage("`Cancel Order` :: ⚠️ Sorry - failed to pull open orders for the following reason:" + Environment.NewLine +
                        ex.Message + Environment.NewLine + Environment.NewLine +
                        "Please enter the pair again, or send 'r' to retry with " + pairTup.Item1 + "-" + pairTup.Item2 + ".", editMessage: editMsg);
                    return;
                }

                if (openOs.Data.Count() > 0) {
                    SendMessage(compileOpenOrders(openOs, pairTup.Item1, pairTup.Item2), buttons: QuitToMain(), editMessage: editMsg);
                    TGstate.commandSubStage = 2;
                }
                else {
                    await SendMessage("`Cancel Order` :: No open orders to cancel for pair " + pairTup.Item1 + "-" + pairTup.Item2 + ".  Exiting cancel order menu.");
                    TGstate.ResetMenu();
                }
            }
        }

        private void MarketOrder_InvalidVolume(bool editMsg = false) {
            SendMessage("`Market Order` :: ⚠️ Invalid volume, please try again or enter 'quit' to exit to the main menu.", buttons: QuitToMain(), editMessage: editMsg);
        }

        private void MarketOrder_SubStage1(string message, bool editMsg = false) {
            Tuple<string, string> pairTup = verifyChosenPair(message, "Market Order");
            if (pairTup.Item1 != "") {
                TGstate.ChosenPair = pairTup;
                SendMessage("`Market Order` :: ❓ " + pairTup.Item1 + "-" + pairTup.Item2 + " chosen.  Is this a buy or a sell?", buttons: BuySellButtons(), editMessage: editMsg);
                TGstate.commandSubStage = 2;
            }  // no need for an else clause here, the verifychosenPair() sub handles it
        }

        private void MarketBuy_Sub(bool editMsg = false) {
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

            string msg = "`Market Order` :: BUY order chosen." + Environment.NewLine +
                "  Current best bid is: $" + Utilities.FormatValue(bestOffer) + Environment.NewLine;

            Dictionary<string, Account> accounts;
            try {
                accounts = pIR.GetAccounts();
                if (accounts.ContainsKey(TGstate.ChosenPair.Item2)) {
                    msg += "  " + TGstate.ChosenPair.Item2.ToUpper() + " account balance: $" + Utilities.FormatValue(accounts[TGstate.ChosenPair.Item2].AvailableBalance) + Environment.NewLine;
                }
            }
            catch (Exception ex) {
                Debug.Print("TGBot: couldn't get account balance for buy, will just continue without it.  error: " + ex.Message);
            }

            msg += Environment.NewLine + "  ❓ How much " + TGstate.ChosenPair.Item1 + " do you want to buy?";
            SendMessage(msg, buttons: QuitToMain(), editMessage: editMsg);

            TGstate.commandSubStage = 30;
        }

        private void MarketSell_Sub(bool editMsg = false) {
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

            string msg = "`Market Order` :: SELL order chosen. " + Environment.NewLine +
                "  Current best bid is: $" + Utilities.FormatValue(bestBid) + Environment.NewLine;

            Dictionary<string, Account> accounts;
            try {
                accounts = pIR.GetAccounts();
                if (accounts.ContainsKey(TGstate.ChosenPair.Item1)) {
                    msg += "  " + TGstate.ChosenPair.Item1.ToUpper() + " account balance: " + TGstate.ChosenPair.Item1.ToUpper() + " "  + Utilities.FormatValue(accounts[TGstate.ChosenPair.Item1].AvailableBalance) + Environment.NewLine;
                }
            }
            catch (Exception ex) {
                Debug.Print("TGBot: couldn't get account balance for buy, will just continue without it.  error: " + ex.Message);
            }
            SendMessage(msg + Environment.NewLine + "  ❓ How much " + TGstate.ChosenPair.Item1 + " do you want to sell?", buttons: QuitToMain(), editMessage: editMsg);

            TGstate.commandSubStage = 40;
        }

        private async void AccountBalances_Sub(bool editMsg = false) {
            Dictionary<string, Account> accounts;
            try {
                accounts = pIR.GetAccounts();
            }
            catch (Exception ex) {
                Debug.Print("TGBot: failed to pull accounts: " + ex.Message);
                await SendMessage("`Account Balance` :: ⚠️ Failed to pull account data, the following is the most recent account data.", editMessage: editMsg);
                accounts = pIR.accounts;
            }
            if (accounts.Count > 0) {
                await SendMessage("`Account Balance` :: Crypto and fiat currencies shown below", editMessage: editMsg);
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
        }

        private async void MarketSummary_Sub(string fiat, bool editMsg = false) {

            if (DCE_IR.CurrentSecondaryCurrency != fiat) {
                // need to quick it grab 
                foreach (string crypto in DCE_IR.PrimaryCurrencyList) {
                    IRT.ParseDCE_IR(crypto, fiat, false);
                }
            }

            Dictionary<string, DCE.MarketSummary> mSummaries = DCE_IR.GetCryptoPairs();
            if (mSummaries.Count < 1) {
                await SendMessage("`Market Summary` :: ⁉️ No market data available", editMessage: editMsg);
            }
            else {
                await SendMessage("`Market Summary` :: 📊 " + fiat + " market prices:" + Environment.NewLine + Environment.NewLine, editMessage: editMsg);
                string masterStr = "------------------------------" + Environment.NewLine;
                foreach (KeyValuePair<string, DCE.MarketSummary> mSummary in mSummaries) {
                    if (mSummary.Value.SecondaryCurrencyCode.ToUpper() == fiat) {
                        masterStr += "  *" + mSummary.Value.PrimaryCurrencyCode + "*: $" + Utilities.FormatValue((mSummary.Value.CurrentLowestOfferPrice + mSummary.Value.CurrentHighestBidPrice) / 2) + " / " + Utilities.FormatValue(mSummary.Value.DayVolumeXbt);
                        // if this is BTC, and the last time we compared IR vol against BTCM was less than a minute ago, and we have a set emoji...
                        if ((mSummary.Value.PrimaryCurrencyCode.ToUpper() == "XBT") && (BTCMemojiLastSet + TimeSpan.FromMinutes(1) > DateTime.Now) && !string.IsNullOrEmpty(BTCMemoji)) {
                            masterStr += " " + BTCMemoji;
                        }
                        masterStr += Environment.NewLine;
                    }
                }
                masterStr += "------------------------------";
                await SendMessage(masterStr);
            }
            TGstate.ResetMenu();
        }

        // verifies and splits a pair sent by the user is legit and exists as a real pair
        private Tuple<string, string> verifyChosenPair(string pair, string subMenu) {
            string crypto = "";
            string fiat = "";

            pair = pair.Trim();
            string pair2 = pair;

            // let's see if we can massage the string into the right format
            if (pair2.IndexOf("-") == -1) {
                if (pair2.IndexOf(" ") == -1) {
                    // try and solve for crypto and fiat run together eg BTCAUD
                    if (pair2.Length == 6) {  // assuming something like BTCAUD
                        pair2 = pair2.Substring(0, 3) + "-" + pair2.Substring(3, 3);
                    }
                    else if (pair2.Length == 7) {  // assuming something like USDTAUD
                        pair2 = pair2.Substring(0, 4) + "-" + pair2.Substring(4, 3);
                    }
                }
                else {  // ok we have a space, so maybe something liek "BTC AUD"
                    pair2 = pair2.Replace(" ", "-");
                }
            }

            // need to consider whether we sanitise the pair string before sending it to SplitPair(), or we upgrade SplitPair to deal with shitty strings
            Tuple<string, string> pairTup = Utilities.SplitPair(pair2.ToUpper());
            if (pairTup.Item1 == "" || pairTup.Item2 == "") {
                SendMessage("`" + subMenu + "` :: ⚠️ Couldn't parse the pair, please try again or 'quit' to exit this menu.");
                return new Tuple<string, string>("", "");
            }

            string pairInternal = pair2.ToUpper();
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
                SendMessage("`" + subMenu + "` :: ⚠️ This pair (" + pair  + ") doesn't exist, please try again or 'quit' to exit this menu.");
                return new Tuple<string, string>("", "");
            }
            return new Tuple<string, string>(crypto.ToUpper(), fiat.ToUpper());
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
                    "  Original vol: " + crypto + " " + Utilities.FormatValue(bho.Original.Volume, 8, false) +
                    (bho.Outstanding.HasValue ? Environment.NewLine + "  Outstanding vol: " + crypto + " " + Utilities.FormatValue(bho.Outstanding.Value, 8, false) : "") + Environment.NewLine +
                    "  Date created: " + bho.CreatedTimestampUtc.ToLocalTime() + Environment.NewLine;
                TGstate.openOrdersToList.Add(count, bho);
                count++;
            }
            return masterStr;
        }

        // This will build a text version of the order book for the user to check out
        // assumes that the pairTup tuple is not empty and has been checked in the verifyChosenPair method
        private async void ShowOrderBook(Tuple<string, string> pairTup) {

            string pair = pairTup.Item1 + "-" + pairTup.Item2;
            int sleepBuffer = 50;  // we sleep for a bit, if the API complains we start blowing this out

            bool firstRun = true;
            TGstate.RefreshingOrderBook = true;
            DateTime startLoop = DateTime.Now;
            do {

                if (!DCE_IR.IR_OBs.ContainsKey(pair)) {
                    await SendMessage("`Show Order Book` :: ⚠️ This order book was empty, cannot display anything.");
                    break;
                }
                ConcurrentDictionary<decimal, ConcurrentDictionary<string, DCE.OrderBook_IR>> buyOrders = DCE_IR.IR_OBs[pair].Item1;
                ConcurrentDictionary<decimal, ConcurrentDictionary<string, DCE.OrderBook_IR>> sellOrders = DCE_IR.IR_OBs[pair].Item2;
                if ((buyOrders.Count > 0) && (sellOrders.Count > 0)) {
                    string masterStr = "`Show Order Book` :: 📚 " + pairTup.Item1 + "-" + pairTup.Item2 + Environment.NewLine + Environment.NewLine;

                    IOrderedEnumerable<KeyValuePair<decimal, ConcurrentDictionary<string, DCE.OrderBook_IR>>> orderedBids;
                    IOrderedEnumerable<KeyValuePair<decimal, ConcurrentDictionary<string, DCE.OrderBook_IR>>> orderedOffers;
                    KeyValuePair<decimal, ConcurrentDictionary<string, DCE.OrderBook_IR>>[] tempArrayBook = DCE_IR.IR_OBs[pair].Item2.ToArray();  // because we're buying from the sell orders
                    orderedOffers = tempArrayBook.OrderBy(k => k.Key);
                    tempArrayBook = DCE_IR.IR_OBs[pair].Item1.ToArray();  // because we're selling to the buy orders
                    orderedBids = tempArrayBook.OrderByDescending(k => k.Key);

                    masterStr += "```" + Environment.NewLine;

                    string tempOffers = "";

                    // let's show 10 orders per book
                    for (int i = 0; i < 10; i++) {
                        // the price should be padded out to 12 characters to cover "$ 999 999.99"
                        string price = "$ " + Utilities.FormatValue(orderedOffers.ElementAt(i).Key);
                        int padding = 12 - (price.Length);
                        for (int h = 1; h < padding; h++) {
                            price += " ";
                        }

                        // now let's get the volume
                        decimal volume = 0;
                        bool myOrder = false;
                        foreach (KeyValuePair<string, DCE.OrderBook_IR> orderGuid in orderedOffers.ElementAt(i).Value) {
                            volume += orderGuid.Value.Volume;

                            // while we're spinning through the sub orders, let's see if one of them is my order
                            if (pIR.openOrders.Contains(new Guid(orderGuid.Key))) {
                                myOrder = true;
                            }
                        }

                        string volumeStr = Utilities.FormatValue(volume, 8, false);
                        string tempLine = price + "   " + volumeStr;

                        if (myOrder) tempLine += "  <<";

                        tempLine += Environment.NewLine;

                        tempOffers = tempLine + tempOffers;  // gotta work this one backwards
                    }

                    // spread
                    decimal bestOffer = orderedOffers.ElementAt(0).Key;
                    decimal bestBid = orderedBids.ElementAt(0).Key;

                    masterStr += tempOffers + "```" + Environment.NewLine + "👆 Offers   ($ " + Utilities.FormatValue((bestOffer - bestBid)) + ")   Bids 👇" + Environment.NewLine + Environment.NewLine + "```" + Environment.NewLine;
                    string tempBids = "";

                    // let's show 10 orders per book
                    for (int i = 0; i < 10; i++) {
                        // the price should be padded out to 12 characters to cover "$ 999 999.99"
                        string price = "$ " + Utilities.FormatValue(orderedBids.ElementAt(i).Key);
                        int padding = 12 - (price.Length);
                        for (int h = 1; h < padding; h++) {
                            price += " ";
                        }

                        // now let's get the volume 
                        decimal volume = 0;
                        bool myOrder = false;
                        foreach (KeyValuePair<string, DCE.OrderBook_IR> orderGuid in orderedBids.ElementAt(i).Value) {
                            volume += orderGuid.Value.Volume;

                            // while we're spinning through the sub orders, let's see if one of them is my order
                            if (pIR.openOrders.Contains(new Guid(orderGuid.Key))) {
                                myOrder = true;
                            }
                        }

                        string volumeStr = Utilities.FormatValue(volume, 8, false);
                        string tempLine = price + "   " + volumeStr;
                        if (myOrder) tempLine += "  <<";
                        tempLine += Environment.NewLine;
                        tempBids += tempLine;
                    }
                    masterStr += tempBids + "```";
                    try {
                        await SendMessage(masterStr, buttons: QuitToMain(), editMessage: !firstRun);
                    }
                    catch (Exception ex) {
                        Debug.Print("TGBot: whoops, probably getting rate limited.  In the order book refresh loop.  new loop pause is: " + (Properties.Settings.Default.UITimerFreq + sleepBuffer).ToString() + ", error:  " + ex.Message);
                        Thread.Sleep(2000);
                        sleepBuffer += 200;
                    }
                    if (firstRun) firstRun = false;  // not our first run anymoawah

                }
                else {  // one of the order books was empty
                    await SendMessage("`Show Order Book` :: ⚠️ This order book was empty, cannot display anything.");
                    break;
                }

                Thread.Sleep(Properties.Settings.Default.UITimerFreq + sleepBuffer);
            } while (TGstate.RefreshingOrderBook && !Properties.Settings.Default.TelegramAllNewMessages && (startLoop + TimeSpan.FromMinutes(1) > DateTime.Now));

            TGstate.ResetMenu();

        }

        public async Task SendMessage(string message, Telegram.Bot.Types.Enums.ParseMode pMode = Telegram.Bot.Types.Enums.ParseMode.MarkdownV2, InlineKeyboardMarkup buttons = null, bool editMessage = false) {

            if ((Properties.Settings.Default.TelegramChatID == 0) || (TGstate.authStage != 2)) {
                Debug.Print("TG: no chat ID or user not authenticated, cannot send message: " + message);
                return;
            }

            if (Properties.Settings.Default.TelegramAllNewMessages) editMessage = false;  // ignore what's sent, we don't want to edit anything

            if (pMode == Telegram.Bot.Types.Enums.ParseMode.MarkdownV2) {
                message = message.Replace("-", "\\-").Replace(".", "\\.").Replace("(", "\\(").Replace(")", "\\)").Replace("=", "\\=")
                    .Replace(">", "\\>").Replace("!", "\\!").Replace("|", "\\|").Replace("#", "\\#").Replace("XBT", "BTC");
            }

            if (NextMsgIsNew) {  // don't edit the message, instead post a new one
                NextMsgIsNew = false;
                editMessage = false;
            }

            if (message.Equals(LastMessage) && editMessage) return;  // if we're editing, we're not allowed to try and send exactly the same message
            LastMessage = message;

            if (editMessage) LatestMessageID = (await botClient.EditMessageTextAsync(Properties.Settings.Default.TelegramChatID, LatestMessageID, message, pMode, false, buttons)).MessageId;
            else LatestMessageID = (await botClient.SendTextMessageAsync(Properties.Settings.Default.TelegramChatID, message, pMode, false, false, 0, buttons)).MessageId;
        }

        public async void closedOrders(Page<BankHistoryOrder> cOrders, string APIkey) {
            if (cOrders.Data.Count() > 0) {

                if (APIkey != Properties.Settings.Default.IRAPIPubKey) {
                    Debug.Print("closedOrders - bailed due to mismatched APIkey");
                    return;  // different key, I guess this is the wrong data?
                }

                if (cOrders.Data.First().PrimaryCurrencyCode.ToString().ToUpper() == "BTC")
                    Debug.Print("TG closed orders: we have been sent a BTC order??");

                string cryptoTmp = (cOrders.Data.First().PrimaryCurrencyCode.ToString().ToUpper() == "XBT" ? "BTC" : cOrders.Data.First().PrimaryCurrencyCode.ToString());
                string pair = (cryptoTmp + "-" + cOrders.Data.First().SecondaryCurrencyCode).ToUpper();
                List<BankHistoryOrder> ordersToNotify = new List<BankHistoryOrder>();

                /*if (pair == "BTC-AUD") {
                    if (notifiedOrders.ContainsKey(pair)) {
                        Debug.Print(DateTime.Now + " - Initial NotifiedOrders count: " + notifiedOrders[pair].Count + " -- sent APIKey: " + APIkey + ", stored APIKey: " + Properties.Settings.Default.IRAPIPubKey);
                    }
                    else Debug.Print("BTC-AUD not in notifiedOrders" + " -- sent APIKey: " + APIkey + ", stored APIKey: " + Properties.Settings.Default.IRAPIPubKey);
                }*/


                if (!notifiedOrders.ContainsKey(pair)) notifiedOrders.TryAdd(pair, new List<Guid>());
                if (!closedOrdersFirstRun.ContainsKey(pair)) closedOrdersFirstRun.TryAdd(pair, true);

                // for some reason we're doubling up orders here I think into notifiedOrders
                foreach (BankHistoryOrder cOrder in cOrders.Data) {
                    if (!notifiedOrders[pair].Contains(cOrder.OrderGuid)) {
                        ordersToNotify.Add(cOrder);
                        notifiedOrders[pair].Add(cOrder.OrderGuid);  // persistent for the session
                    }
                }

                /*if (pair == "BTC-AUD") {
                    Debug.Print(DateTime.Now + " - BTCAUD closedOrders - order count: " + cOrders.Data.Count() + " -- sent APIKey: " + APIkey + ", stored APIKey: " + Properties.Settings.Default.IRAPIPubKey);
                    Debug.Print("BTCAUD closedOrders - notifiedOrders count: " + notifiedOrders[pair].Count + " -- sent APIKey: " + APIkey + ", stored APIKey: " + Properties.Settings.Default.IRAPIPubKey);
                    Debug.Print("BTCAUD closedOrders - ordersToNotify count: " + ordersToNotify.Count + " -- sent APIKey: " + APIkey + ", stored APIKey: " + Properties.Settings.Default.IRAPIPubKey);
                    Debug.Print("BTCAUD closedOrders - closedOrdersFirstRun: " + closedOrdersFirstRun[pair] + " -- sent APIKey: " + APIkey + ", stored APIKey: " + Properties.Settings.Default.IRAPIPubKey);
                }*/

                if (!closedOrdersFirstRun[pair]) {

                    if (ordersToNotify.Count > 0) {
                        // send message..
                        foreach (BankHistoryOrder cOrder in ordersToNotify) {
                            if (cOrder.Status == OrderStatus.Filled) {
                                string crypto = cOrder.PrimaryCurrencyCode.ToString().ToUpper();
                                await SendMessage("*Order Filled!* 🤝" + Environment.NewLine +
                                    "  Pair: " + crypto + "-" + cOrder.SecondaryCurrencyCode.ToString().ToUpper() + Environment.NewLine +
                                    "  Value: $" + Utilities.FormatValue(cOrder.Value.Value, 2) + Environment.NewLine +
                                    "  Avg price: $" + Utilities.FormatValue(cOrder.AvgPrice.Value, 2) + Environment.NewLine +
                                    "  Volume: " + crypto + ": " + Utilities.FormatValue(cOrder.Volume, 8, false) + Environment.NewLine +
                                    "  Order created: " + cOrder.CreatedTimestampUtc.ToLocalTime());
                                NextMsgIsNew = true;  // don't edit this message if the next message normally would
                            }
                        }
                        try {
                            pIR.GetAccounts();
                        }
                        catch (Exception ex) {
                            Debug.Print("Tried to grab the accounts after checking if we have any new closed orders but it failed: " + ex.Message);
                        }
                    }
                }
                else {
                    closedOrdersFirstRun[pair] = false;
                    Debug.Print(DateTime.Now + " - TGBot - in closedOrders sub, this is the first run for " + pair);
                }
            }
        }

        enum CommandChosen {
            Forget, CancelOrder, Nothing, MarketBaiter, MarketOrder, ViewClosed, ShowOrderBook
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
            public bool RefreshingOrderBook { get; set; } = false;

            public TelegramState(TelegramBot _TGBot) {
                TGBot = _TGBot;
            }

            public void ResetMenu(bool editMsg = false) {
                if (RefreshingOrderBook) RefreshingOrderBook = false;  // if we're refreshing the order book then don't reset yet, first let it quit.  once quit we'll call this again.
                else {
                    commandChosen = CommandChosen.Nothing;
                    commandSubStage = 0;
                    orderToCancel = 0;
                    openOrdersToList.Clear();
                    ChosenPair = new Tuple<string, string>("", "");
                    Volume = -1;
                    TGBot.SendMessage("*IR Ticker TelegramBot Top Menu*" + Environment.NewLine +
                        "Account: `" + Properties.Settings.Default.APIFriendly + "`" + Environment.NewLine +
                        "Please enter your command, or 'help' for a list of commands.", buttons: MainMenuButtons(), editMessage: editMsg); ; ;
                }

            }
        }
    }
}
