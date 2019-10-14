using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace UserTest.Helpers
{
    public class WebServices
    {
        const string REPO = "https://raw.githubusercontent.com/BrenoAngelotti/motion_study/master/";

        const string FORM = "https://docs.google.com/forms/u/1/d/e/1FAIpQLSfNa9Bsd-RZk0tw8dq56pCVEtqJdYCMsXmisj3Y_vjwARpFJA/formResponse";

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

            var formContent = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "entry.1156489473", (App.Current.UserData.HasMotion ? "Sim" : "Não") },
                { "entry.1304370013", "Via app" },
                { "entry.566626876_year", DateTime.Now.Year.ToString() },
                { "entry.566626876_month", DateTime.Now.Month.ToString() },
                { "entry.566626876_day", DateTime.Now.Day.ToString() },
                { "entry.597622892", new Random().Next(0, 10).ToString()},
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