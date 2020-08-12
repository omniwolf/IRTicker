using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using System.Diagnostics;
using System.Net.Http.Headers;

namespace IRTicker {
    class Slack {

        private static readonly HttpClient client = new HttpClient();

        /// <summary>
        /// duration of 0 means it will last forever
        /// </summary>
        /// <param name="status"></param>
        /// <param name="emoji"></param>
        /// <param name="duration"></param>
        public void setStatus(string status = "", string emoji = "", long duration = 0, string name = "") {

            if (name == "" && (Properties.Settings.Default.SlackDefaultName == "")) {
                var profChange = new SlackProfile_noname {
                    profile = new SlackStatus_noname {
                        status_emoji = emoji,
                        status_text = status,
                        status_expiration = duration + (duration == 0 ? 0 : DateTimeOffset.Now.ToUnixTimeSeconds())  // if we send 0 for duration, then send it on to the slack API
                    }
                };
                SendMessageAsync(Properties.Settings.Default.SlackToken, profChange).Wait();
            }
            else {
                if (name == "") name = Properties.Settings.Default.SlackDefaultName;  // set their default name if we don't specify anything (this should be for when the app closes)
                var profChange = new SlackProfile {
                    profile = new SlackStatus {
                        status_emoji = emoji,
                        status_text = status,
                        status_expiration = duration + (duration == 0 ? 0 : DateTimeOffset.Now.ToUnixTimeSeconds()),  // if we send 0 for duration, then send it on to the slack API
                        display_name = name
                    }
                };
                SendMessageAsync(Properties.Settings.Default.SlackToken, profChange).Wait();
            }
        }

        public class SlackProfile {
            public SlackStatus profile { get; set; }
        }

        public class SlackProfile_noname {
            public SlackStatus_noname profile { get; set; }
        }

        public class SlackStatus {
            public string status_text { get; set; }
            public string status_emoji { get; set; }
            public long status_expiration { get; set; }
            public string display_name { get; set; }
        }

        public class SlackStatus_noname {
            public string status_text { get; set; }
            public string status_emoji { get; set; }
            public long status_expiration { get; set; }
        }

        // reponse from message methods
        public class SlackMessageResponse {
            public bool ok { get; set; }
            public string error { get; set; }
            public string channel { get; set; }
            public string ts { get; set; }
        }

        // a slack message
        public class SlackMessage {
            public string channel { get; set; }
            public string text { get; set; }
            public bool as_user { get; set; }
            public SlackAttachment[] attachments { get; set; }
        }

        // a slack message attachment
        public class SlackAttachment {
            public string fallback { get; set; }
            public string text { get; set; }
            public string image_url { get; set; }
            public string color { get; set; }
        }

        // sends a slack message asynchronous
        // throws exception if message can not be sent
        public static async Task SendMessageAsync(string token, object msg) {
            // serialize method parameters to JSON
            var content = JsonConvert.SerializeObject(msg);

            try {
                var httpContent = new StringContent(
                    content,
                    Encoding.UTF8,
                    "application/json"
                );

                // set token in authorization header
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                // send message to API
                var response = await client.PostAsync("https://slack.com/api/users.profile.set", httpContent).ConfigureAwait(continueOnCapturedContext: false);
                // fetch response from API
                var responseJson = await response.Content.ReadAsStringAsync();

                // convert JSON response to object
                SlackMessageResponse messageResponse =
                    JsonConvert.DeserializeObject<SlackMessageResponse>(responseJson);

                // throw exception if sending failed
                if ((messageResponse == null) || (messageResponse.ok == false)) {
                    throw new Exception(
                        "failed to send message. error: " + messageResponse.error
                    );
                }
            }
            catch (Exception ex) {
                Debug.Print(DateTime.Now + " - exception when trying to do Slack API stuff.  Probably network down? - " + ex.Message);
            }
        }
    }
}
