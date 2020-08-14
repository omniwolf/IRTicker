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

        public ConcurrentDictionary<string, bool> closedOrdersFirstRun = new ConcurrentDictionary<string, bool>();
        private ConcurrentDictionary<string, List<Guid>> notifiedOrders = new ConcurrentDictionary<string, List<Guid>>();

        public TelegramBot(PrivateIR _pIR, DCE _DCE_IR, IRTicker _IRT) {

            DCE_IR = _DCE_IR;
            pIR = _pIR;
            IRT = _IRT;

            TGstate = new TelegramState(this);
            Debug.Print("starting tgbot with: " + Properties.Settings.Default.TelegramAPIToken);

            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;

            botClient = new TelegramBotClient(Properties.Settings.Default.TelegramAPIToken);

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
            botClient.StopReceiving();
        }

        public static InlineKeyboardMarkup MainMenuButtons() {
            return new InlineKeyboardMarkup(
                new InlineKeyboardButton[][] {
                    new InlineKeyboardButton[] {
                        InlineKeyboardButton.WithCallbackData("Summary AUD", "main-summ"),
                        InlineKeyboardButton.WithCallbackData("Account balances", "main-account")
                    },
                    new InlineKeyboardButton[] {
                        InlineKeyboardButton.WithCallbackData("Market buy BTC/AUD", "main-market-buy-btc"),
                        InlineKeyboardButton.WithCallbackData("Cancel order", "main-cancel")
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
                        InlineKeyboardButton.WithCallbackData("Quit to main menu", "quit")},
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
                        InlineKeyboardButton.WithCallbackData("Quit to main menu", "quit")},
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
                            SendMessage("`Cancel Order` :: ❓ Specify which pair (eg BTC-AUD):", buttons: CommonPairsButtons(), editMessage: true);
                            TGstate.commandSubStage = 1;
                            break;
                        case "pair-btcaud":
                            if (TGstate.commandChosen == CommandChosen.MarketOrder) MarketOrder_SubStage1("BTC-AUD", true);
                            else if (TGstate.commandChosen == CommandChosen.CancelOrder) CancelOrder_Sub("BTC-AUD", true);
                            break;
                        case "pair-ethaud":
                            if (TGstate.commandChosen == CommandChosen.MarketOrder) MarketOrder_SubStage1("ETH-AUD", true);
                            else if (TGstate.commandChosen == CommandChosen.CancelOrder) CancelOrder_Sub("ETH-AUD", true); break;
                        case "pair-xrpaud":
                            if (TGstate.commandChosen == CommandChosen.MarketOrder) MarketOrder_SubStage1("XRP-AUD", true);
                            else if (TGstate.commandChosen == CommandChosen.CancelOrder) CancelOrder_Sub("XRP-AUD", true); break;
                        case "pair-usdtaud":
                            if (TGstate.commandChosen == CommandChosen.MarketOrder) MarketOrder_SubStage1("USDT-AUD", true);
                            else if (TGstate.commandChosen == CommandChosen.CancelOrder) CancelOrder_Sub("USDT-AUD", true); break;
                        case "buy-order":
                            MarketBuy_Sub(true);
                            break;
                        case "sell-order":
                            MarketSell_Sub(true);
                            break;
                        case "yes":
                            switch (TGstate.commandChosen) {
                                case CommandChosen.StopMarketBaiter:
                                    if (TGstate.commandSubStage == 1) {
                                        pIR.marketBaiterActive = false;
                                        await SendMessage("`Market Baiter` :: ✅ Market baiter stopped.", editMessage: true);
                                    }
                                    break;

                                case CommandChosen.MarketOrder:
                                    if (TGstate.commandSubStage == 31) {  // 31 is to place the buy order (or not)

                                        BankOrder marketO;
                                        try {
                                            marketO = pIR.PlaceMarketOrder(TGstate.ChosenPair.Item1, TGstate.ChosenPair.Item2, OrderType.MarketBid, TGstate.Volume);
                                        }
                                        catch (Exception ex) {
                                            Debug.Print("TGBot: couldn't place the market buy order: " + ex.Message);
                                            await SendMessage("`Market Order` :: ⚠️ Order failed for the following reason:" + Environment.NewLine +
                                                ex.Message + Environment.NewLine + Environment.NewLine +
                                                "Try again?", buttons: YesNoButtons(), editMessage: true);
                                            return;
                                        }
                                        if ((marketO.Status == OrderStatus.Filled) || (marketO.Status == OrderStatus.Open)) {
                                            await SendMessage("`Market Order` :: ✅ Order placed!", editMessage: true);
                                        }
                                        else {
                                            await SendMessage("`Market Order` :: ⁉️ Order successful, but not filled?  Status: " + marketO.Status, editMessage: true);
                                        }
                                    }
                                    else if (TGstate.commandSubStage == 41) {  // 41 is to place the sell order (or not)

                                        BankOrder marketO;
                                        try {
                                            marketO = pIR.PlaceMarketOrder(TGstate.ChosenPair.Item1, TGstate.ChosenPair.Item2, OrderType.MarketOffer, TGstate.Volume);
                                        }
                                        catch (Exception ex) {
                                            Debug.Print("TGBot: couldn't place the market sell order: " + ex.Message);
                                            await SendMessage("`Market Order` :: ⚠️ Order failed for the following reason:" + Environment.NewLine +
                                                ex.Message + Environment.NewLine + Environment.NewLine +
                                                "Try again?", buttons: YesNoButtons(), editMessage: true);
                                            return;
                                        }
                                        if ((marketO.Status == OrderStatus.Filled) || (marketO.Status == OrderStatus.Open)) {
                                            await SendMessage("`Market Order` :: ✅ Order placed!", editMessage: true);
                                        }
                                        else {
                                            await SendMessage("`Market Order` :: ⁉️ Order successful, but not filled?  Status: " + marketO.Status, editMessage: true);
                                        }
                                    }
                                    break;

                                case CommandChosen.CancelOrder:
                                    if (TGstate.commandSubStage == 3) {  // 3 i confirming the cancel

                                        BankOrder cancelledOrder;
                                        try {
                                            cancelledOrder = pIR.CancelOrder(TGstate.openOrdersToList[TGstate.orderToCancel].OrderGuid.ToString());
                                        }
                                        catch (Exception ex) {
                                            SendMessage("`Cancel Order` :: ⚠️ Failed to cancel the order.  Try again?", buttons: YesNoButtons(), editMessage: true);
                                            return;
                                        }
                                        if (cancelledOrder.Status == OrderStatus.Cancelled) {
                                            await SendMessage("`Cancel Order` :: ✅ Order successfully cancelled.", editMessage: true);
                                        }
                                    }
                                    break;
                            }
                            TGstate.ResetMenu();
                            break;
                        case "no":
                            switch (TGstate.commandChosen) {
                                case CommandChosen.StopMarketBaiter:
                                    await SendMessage("`Market Baiter` :: Market baiter will continue.", editMessage: true);
                                    break;

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
                                        if (pIR.marketBaiterActive) {
                                            SendMessage("`Market Baiter` :: ⬆️ Market baiter is currently *active*" + Environment.NewLine +
                                                "  ❓ Do you wish to stop it?", buttons: YesNoButtons());
                                            TGstate.commandSubStage = 1;
                                            TGstate.commandChosen = CommandChosen.StopMarketBaiter;
                                        }
                                        else {
                                            await SendMessage("`Market Baiter` :: ⬇️ Market baiter is currently *inactive*.");
                                            TGstate.ResetMenu();
                                        }
                                        break;

                                    case "account":
                                        AccountBalances_Sub();
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
                                if (message.Text.ToLower() == "b") {
                                    MarketBuy_Sub();  // we have a sub for this because it gets called by a button too.  the sell side has no button, so we can just code directly into the parent sub
                                }
                                else if (message.Text.ToLower() == "s") {
                                    MarketSell_Sub();
                                }
                                else {
                                    SendMessage("`Market Order` :: ⚠️ Unrecognised command.  Try again or 'quit' to exit");
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

                                    SendMessage("`Market Order` :: You wish to place a market *BUY* order for " + TGstate.ChosenPair.Item1 + "-" + TGstate.ChosenPair.Item2 + " of size " + vol + "." + Environment.NewLine +
                                        "  Current best offer is: $" + Utilities.FormatValue(bestOffer) + Environment.NewLine +
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
                                    SendMessage("`Market Order` :: You wish to place a market *SELL* order for " + TGstate.ChosenPair.Item1 + "-" + TGstate.ChosenPair.Item2 + " of size " + vol + "." + Environment.NewLine +
                                        "  Current best bid is: $" + Utilities.FormatValue(bestBid) + Environment.NewLine +
                                        "  Order approx value: $" + Utilities.FormatValue(bestBid * vol) + Environment.NewLine + Environment.NewLine +
                                        "❓ Do you wish to proceed?", buttons: YesNoButtons());

                                    TGstate.Volume = vol;
                                    TGstate.commandSubStage = 41;
                                }
                                else {
                                    SendMessage("`Market Order` :: ⚠️ Invalid volume, please try again or 'quit' to exit.");
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

        private async void CancelOrder_Sub(string message, bool editMsg = false) {
            Tuple<string, string> pairTup;
            // if we failed because of nonces, but we have a legit pair, we can retry (if they send 'r')
            if ((message.ToUpper() == "R") && (!string.IsNullOrEmpty(TGstate.ChosenPair.Item1)) && (!string.IsNullOrEmpty(TGstate.ChosenPair.Item1))) {
                pairTup = TGstate.ChosenPair;
            }
            else {
                pairTup = verifyChosenPair(message, "Cancel order");
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
                        "Please enter the pair again, or send 'r' to retry with " + pairTup.Item1 + "-" + pairTup.Item2 + ".", editMessage: editMsg);
                    return;
                }

                if (openOs.Data.Count() > 0) {
                    SendMessage(compileOpenOrders(openOs, pairTup.Item1, pairTup.Item2), editMessage: editMsg);
                    TGstate.commandSubStage = 2;
                }
                else {
                    await SendMessage("`Cancel Order` :: No open orders to cancel for pair " + pairTup.Item1 + "-" + pairTup.Item2 + ".  Exiting cancel order menu.");
                    TGstate.ResetMenu();
                }
            }
            else {
                SendMessage("`Cancel Order` :: ⚠️ " + pairTup.Item1 + "-" + pairTup.Item2 + " pair doesn't exist. Try again or 'quit' to exit.", editMessage: editMsg);
            }
        }

        private void MarketOrder_InvalidVolume(bool editMsg = false) {
            SendMessage("`Market Order` :: ⚠️ Invalid volume, please try again or enter 'quit' to exit to the main menu.", editMessage: editMsg);
        }

        private void MarketOrder_SubStage1(string message, bool editMsg = false) {
            Tuple<string, string> pairTup = verifyChosenPair(message, "Market Order");
            if (pairTup.Item1 != "") {
                TGstate.ChosenPair = pairTup;
                SendMessage("`Market Order` :: ❓ " + pairTup.Item1 + "-" + pairTup.Item2 + " chosen.  Is this a buy or a sell? (b/s)", buttons: BuySellButtons(), editMessage: editMsg);
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
                foreach (KeyValuePair<string, Account> acc in accounts) {
                    if (TGstate.ChosenPair.Item2 == acc.Key) {
                        msg += "  " + acc.Key.ToUpper() + " account balance: $" + Utilities.FormatValue(acc.Value.AvailableBalance) + Environment.NewLine;
                    }
                }
            }
            catch (Exception ex) {
                Debug.Print("TGBot: couldn't get account balance for buy, will just continue without it.  error: " + ex.Message);
            }

            msg += Environment.NewLine + "  ❓ How much " + TGstate.ChosenPair.Item1 + " do you want to buy?";
            SendMessage(msg, editMessage: editMsg);

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
                foreach (KeyValuePair<string, Account> acc in accounts) {
                    if (TGstate.ChosenPair.Item1 == acc.Key) {
                        msg += "  " + acc.Key.ToUpper() + " account balance: " + Utilities.FormatValue(acc.Value.AvailableBalance) + Environment.NewLine;
                    }
                }
            }
            catch (Exception ex) {
                Debug.Print("TGBot: couldn't get account balance for buy, will just continue without it.  error: " + ex.Message);
            }
            SendMessage(msg + Environment.NewLine + "  ❓ How much " + TGstate.ChosenPair.Item1 + " do you want to sell?", editMessage: editMsg);

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
                        masterStr += "  *" + mSummary.Value.PrimaryCurrencyCode + "*: $" + Utilities.FormatValue((mSummary.Value.CurrentLowestOfferPrice + mSummary.Value.CurrentHighestBidPrice) / 2) + " / " + Utilities.FormatValue(mSummary.Value.DayVolumeXbt) + Environment.NewLine;
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

        public async Task SendMessage(string message, Telegram.Bot.Types.Enums.ParseMode pMode = Telegram.Bot.Types.Enums.ParseMode.MarkdownV2, InlineKeyboardMarkup buttons = null, bool editMessage = false) {
            if ((Properties.Settings.Default.TelegramChatID == 0) || (TGstate.authStage != 2)) {
                Debug.Print("TG: no chat ID or user not authenticated, cannot send message: " + message);
                return;
            }

            if (pMode == Telegram.Bot.Types.Enums.ParseMode.MarkdownV2) {
                message = message.Replace("-", "\\-").Replace(".", "\\.").Replace("(", "\\(").Replace(")", "\\)").Replace("=", "\\=")
                    .Replace(">", "\\>").Replace("!", "\\!").Replace("|", "\\|").Replace("#", "\\#").Replace("XBT", "BTC");
            }

            if (editMessage) LatestMessageID = (await botClient.EditMessageTextAsync(Properties.Settings.Default.TelegramChatID, LatestMessageID, message, pMode, false, buttons)).MessageId;
            else LatestMessageID = (await botClient.SendTextMessageAsync(Properties.Settings.Default.TelegramChatID, message, pMode, false, false, 0, buttons)).MessageId;
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

            public void ResetMenu(bool editMsg = false) {
                commandChosen = CommandChosen.Nothing;
                commandSubStage = 0;
                orderToCancel = 0;
                openOrdersToList.Clear();
                ChosenPair = new Tuple<string, string>("", "");
                Volume = -1;
                TGBot.SendMessage("*IR Ticker TelegramBot Top Menu*" + Environment.NewLine +
                    "Please enter your command, or 'help' for a list of commands.", buttons: MainMenuButtons(), editMessage: editMsg);
                //botClient.SendTextMessageAsync(Properties.Settings.Default.TelegramChatID, "Common commands:", Telegram.Bot.Types.Enums.ParseMode.Default, false, false, 0, MainMenuButtons());

            }
        }
    }
}
