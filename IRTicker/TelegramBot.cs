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

namespace IRTicker
{
    public class TelegramBot
    {

        PrivateIR pIR;
        DCE DCE_IR;

        static TelegramBotClient botClient;
        int authStage = 0;  // 0 = not authed, 1 = awaiting code, 2 = code accepted
        CommandChosen commandChosen = CommandChosen.Nothing;
        int commandSubStage = 0;
      
        public ConcurrentDictionary<string, bool> closedOrdersFirstRun = new ConcurrentDictionary<string, bool>();
        private ConcurrentDictionary<string, List<Guid>> notifiedOrders = new ConcurrentDictionary<string, List<Guid>>();

        public TelegramBot(PrivateIR _pIR, DCE _DCE_IR) {

            DCE_IR = _DCE_IR;
            pIR = _pIR;

            botClient = new TelegramBotClient("679618862:AAGxhT7BZtP0KlR13o7SKaCCNZjfDSaJqv0");

            botClient.OnMessage += Bot_OnMessage;
            botClient.StartReceiving();

            if (Properties.Settings.Default.TelegramChatID != 0) {
                authStage = 2;
                botClient.SendTextMessageAsync(606418720, "I'm alive!", Telegram.Bot.Types.Enums.ParseMode.Default, true, false, 0, null);
            }


        }

        async void Bot_OnMessage(object sender, MessageEventArgs e) {

            if (pIR == null) return;  // don't try anything if we don't have a legit account to login to

            if (e.Message.Text != null) {
                Console.WriteLine($"Received a text message in chat {e.Message.Chat.Id}.");

                switch (commandChosen) {

                    case CommandChosen.Nothing:
                        if ((Properties.Settings.Default.TelegramChatID == 0) && (authStage == 0)) {
                            await botClient.SendTextMessageAsync(e.Message.Chat.Id, "Hello, let's authenticate you.  Please enter your secret code", Telegram.Bot.Types.Enums.ParseMode.Default);
                            authStage = 1;
                            return;
                        }

                        if (authStage == 1) {
                            if (e.Message.Text == Properties.Settings.Default.TelegramCode) {
                                await botClient.SendTextMessageAsync(e.Message.Chat.Id, "Code accepted, you are now authenticated", Telegram.Bot.Types.Enums.ParseMode.Default);
                                authStage = 2;
                                Properties.Settings.Default.TelegramChatID = e.Message.Chat.Id;
                                Properties.Settings.Default.Save();
                            }
                            else {
                                await botClient.SendTextMessageAsync(e.Message.Chat.Id, "Code NOT accepted");
                                authStage = 0;
                            }
                            return;
                        }

                        if (authStage == 2) {

                            switch (e.Message.Text.ToLower()) {
                                case "forget":
                                    await SendMessage("I have forgotten you.You will need to reauthenticate.To do so send any message");
                                    authStage = 0;
                                    Properties.Settings.Default.TelegramChatID = 0;
                                    Properties.Settings.Default.Save();
                                    break;

                                case "cancel":
                                    commandChosen = CommandChosen.CancelOrder;
                                    SendMessage("Cancel order :: Specify which pair (eg BTC-AUD):", Telegram.Bot.Types.Enums.ParseMode.MarkdownV2);
                                    commandSubStage = 1;
                                    break;

                                default:
                                    await SendMessage("*Command List*" + Environment.NewLine + Environment.NewLine +
                                        "Forget             => Will unauthenticate the bot" + Environment.NewLine +
                                        "Cancel             => Will cancel the most recently placed order"
                                        , Telegram.Bot.Types.Enums.ParseMode.MarkdownV2);
                                    break;
                            }
                        }

                        break;

                    case CommandChosen.CancelOrder:
                        if (e.Message.Text.ToLower() == "quit") {
                            commandChosen = CommandChosen.Nothing;
                            commandSubStage = 0;
                            SendMessage("You have quit the Cancel order menu.");
                            return;
                        }

                        if (commandSubStage == 1) {  // 1 is picking the pair
                            string crypto = "";
                            string fiat = "";

                            // need to consider whether we sanitise the pair string before sending it to SplitPair(), or we upgrade SplitPair to deal with shitty strings
                            Tuple<string, string> pairTup = Utilities.SplitPair(e.Message.Text);

                            foreach (string _crypto in DCE_IR.PrimaryCurrencyList) {
                                foreach (string _fiat in DCE_IR.SecondaryCurrencyList) {

                                }
                            }

                        }

                        if (commandSubStage == 2) {  // 2 is we picking which order to cancel

                            if (int.TryParse(e.Message.Text, out int chosenOrderNumber)) {
                                // check if it's a legit order and cancel it

                            }
                            else {
                                SendMessage("Couldn't parse your answer.  Please try again and just type the number next to the order you want to cancel, nothing else.");
                                return;
                            }
                        }

                        break;
                }






                    await botClient.SendTextMessageAsync(Properties.Settings.Default.TelegramChatID, "Available commands: " + Environment.NewLine + Environment.NewLine +
                        "forget   :   this command will unauthenticate you");
                

            }
        }

        private string compileOpenOrders(string crypto, string fiat) {

            //var openOs = pIR.GetOpenOrders(crypto, fiat).Wait();

            return "";
        }

        public async Task SendMessage(string message, Telegram.Bot.Types.Enums.ParseMode pMode = Telegram.Bot.Types.Enums.ParseMode.Default) {
            if ((Properties.Settings.Default.TelegramChatID == 0) || (authStage != 2)) {
                Debug.Print("TG: no chat ID or user not authenticated, cannot send message: " + message);
                return;
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
                            await SendMessage("*Order Filled\\!*" + Environment.NewLine + Environment.NewLine +
                                "Pair: " + cOrder.PrimaryCurrencyCode.ToString().ToUpper() + "\\-" + cOrder.SecondaryCurrencyCode.ToString().ToUpper() + Environment.NewLine +
                                "Value: $" + cOrder.Value.ToString().Replace(".", "\\.") + Environment.NewLine +
                                "Avg price: $" + cOrder.AvgPrice.Value.ToString().Replace(".", "\\.") + Environment.NewLine +
                                "Volume: " + cOrder.PrimaryCurrencyCode.ToString().ToUpper() + ": " + cOrder.Volume.ToString().Replace(".", "\\.") + Environment.NewLine
                                , Telegram.Bot.Types.Enums.ParseMode.MarkdownV2);
                        }
                    }
                }
                else closedOrdersFirstRun[pair] = false;
            }


        }

        enum CommandChosen
        {
            Forget, CancelOrder, Nothing, StopMarketBaiter
        }
    }
}
