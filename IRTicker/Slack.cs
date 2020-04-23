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

        public void setStatus(string status = "", string emoji = "", long duration = 0) {

            var profChange = new SlackProfile {
                profile = new SlackStatus {
                    status_emoji = emoji,
                    status_text = status,
                    status_expiration = duration + DateTimeOffset.Now.ToUnixTimeSeconds()
                }
            };
            
            SendMessageAsync(
                Properties.Settings.Default.SlackToken,
                profChange
            ).Wait();
        }

        public class SlackProfile {
            public SlackStatus profile { get; set; }
        }

        public class SlackStatus {
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
        public static async Task SendMessageAsync(string token, SlackProfile msg) {
            // serialize method parameters to JSON
            var content = JsonConvert.SerializeObject(msg);
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
            if (messageResponse.ok == false) {
                throw new Exception(
                    "failed to send message. error: " + messageResponse.error
                );
            }
        }
    }
}
