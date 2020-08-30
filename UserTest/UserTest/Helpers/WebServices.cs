using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace UserTest.Helpers
{
    public class WebServices
    {
        const string REPO = "https://raw.githubusercontent.com/BrenoAngelotti/tcc/master/";

        const string FORM = "https://docs.google.com//forms/d/e/1FAIpQLSeieYx95v0DvjMSjaAup7Z8NhmTMaq1llmdjv9LHqGcXsnFag/formResponse";

        public static async Task<bool> IsDataCollectionActive()
        {
            var path = $"{REPO}dataCollection.json";

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("cache-control", "no-cache");
                var httpResponse = await httpClient.GetAsync(path);
                if (httpResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var responseContent = await httpResponse.Content.ReadAsStringAsync();

                    var collectingData = JsonConvert.DeserializeObject<Dictionary<string, bool>>(responseContent);
                    return collectingData["collectingData"];
                }
            }
            return false;
        }

        public static async Task<bool> SendDataToForm()
        {
            var data = App.Current.UserData;
            var formContent = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "entry.1412722627", (data.HasMotion ? "true" : "false") },
                { "entry.1364788360", (data.DarkTheme ? "true" : "false") },

                { "entry.2045937829", data.ClarityQuestion.ToString() },
                { "entry.2037255656", data.EnjoyabilityQuestion.ToString() },

                { "entry.84660025", data.Tasks[0].FirstQuestion.ToString() },
                { "entry.990099456", data.Tasks[0].SecondQuestion.ToString() },

                { "entry.513143045", data.Tasks[1].FirstQuestion.ToString() },
                { "entry.698483230", data.Tasks[1].SecondQuestion.ToString() },

                { "entry.255989520", data.Tasks[2].FirstQuestion.ToString() },
                { "entry.54384551", data.Tasks[2].SecondQuestion.ToString() },

                { "entry.1591893337", data.Tasks[3].FirstQuestion.ToString() },
                { "entry.267786997", data.Tasks[3].SecondQuestion.ToString() },

                { "entry.751604436", data.Tasks[4].FirstQuestion.ToString() },
                { "entry.1151637867", data.Tasks[4].SecondQuestion.ToString() },

                { "entry.1277033517", data.TestCode }
            });

            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add("cache-control", "no-cache");
                    var httpResponse = await httpClient.PostAsync(FORM, formContent);
                    if (httpResponse.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        return true;
                    }
                }
            }
            catch (Exception) {}
            return false;
        }

    }
}