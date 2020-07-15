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

namespace IRTicker
{
    public class TelegramBot
    {

        // 679618862:AAGxhT7BZtP0KlR13o7SKaCCNZjfDSaJqv0

        static TelegramBotClient botClient;
        int authStage = 0;  // 0 = not authed, 1 = awaiting code, 2 = code accepted
        private bool closedOrdersFirstRun = true;
        private List<Guid> notifiedOrders = new List<Guid>();

        public TelegramBot() {

            botClient = new TelegramBotClient("679618862:AAGxhT7BZtP0KlR13o7SKaCCNZjfDSaJqv0");

            botClient.OnMessage += Bot_OnMessage;
            botClient.StartReceiving();

            if (Properties.Settings.Default.TelegramChatID != 0) {
                authStage = 2;
                botClient.SendTextMessageAsync(606418720, "I'm alive!", Telegram.Bot.Types.Enums.ParseMode.Default, true, false, 0, null);
            }


        }

        async void Bot_OnMessage(object sender, MessageEventArgs e) {
            if (e.Message.Text != null) {
                Console.WriteLine($"Received a text message in chat {e.Message.Chat.Id}.");

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

                if ((authStage == 2) && (e.Message.Text == "forget")) {
                    authStage = 0;
                    Properties.Settings.Default.TelegramChatID = 0;
                    Properties.Settings.Default.Save();
                    await botClient.SendTextMessageAsync(e.Message.Chat.Id, "I have forgotten you.  You will need to reauthenticate.  To do so send any message");
                    return;
                }

                if (authStage == 2) {
                    await botClient.SendTextMessageAsync(Properties.Settings.Default.TelegramChatID, "Available commands: " + Environment.NewLine + Environment.NewLine +
                        "forget   :   this command will unauthenticate you");
                }

            }
        }

        public async Task SendMessage(string message, Telegram.Bot.Types.Enums.ParseMode pMode = Telegram.Bot.Types.Enums.ParseMode.Default) {
            if ((Properties.Settings.Default.TelegramChatID == 0) || (authStage != 2)) {
                Debug.Print("TG: no chat ID or user not authenticated, cannot send message: " + message);
                return;
            }
            await botClient.SendTextMessageAsync(Properties.Settings.Default.TelegramChatID, message, pMode);
        }

        public async void closedOrders(Page<BankHistoryOrder> cOrders) {

            List<BankHistoryOrder> ordersToNotify = new List<BankHistoryOrder>();

            foreach (BankHistoryOrder cOrder in cOrders.Data) {
                if (!notifiedOrders.Contains(cOrder.OrderGuid)) {
                    ordersToNotify.Add(cOrder);
                    notifiedOrders.Add(cOrder.OrderGuid);  // persistent for the session
                }
            }

            if (!closedOrdersFirstRun) {
                // send message..
                foreach (BankHistoryOrder cOrder in ordersToNotify) {
                    if (cOrder.Status == OrderStatus.Filled) {
                        await SendMessage("*Order Filled\\!*" + Environment.NewLine + Environment.NewLine +
                            "Pair: " + cOrder.PrimaryCurrencyCode + "\\-" + cOrder.SecondaryCurrencyCode + Environment.NewLine +
                            "Value: $" + cOrder.Value.ToString().Replace(".", "\\.") + Environment.NewLine +
                            "Avg price: " + cOrder.AvgPrice.ToString().Replace(".", "\\.") + Environment.NewLine +
                            "Volume: " + cOrder.PrimaryCurrencyCode + ": " + cOrder.Volume.ToString().Replace(".", "\\.") + Environment.NewLine
                            , Telegram.Bot.Types.Enums.ParseMode.MarkdownV2);
                    }
                }
            }
            else closedOrdersFirstRun = false;
        }
    }
}
