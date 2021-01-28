namespace RMDesktopUI.Helpers
{
    using RMDesktopUI.Models;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;

    public class APIHelper : IAPIHelper
    {
        public HttpClient apiClient;


        public APIHelper()
        {
            InitializeClient();
        }

        private void InitializeClient()
        {
            string api = ConfigurationManager.AppSettings["wAPI"];
            apiClient = new HttpClient();
            apiClient.BaseAddress = new Uri(api);
            apiClient.DefaultRequestHeaders.Accept.Clear();
            apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        
        public async Task<AuthenticatedUser> Authenticate(string uName, string pwd)
        {
            var data = new FormUrlEncodedContent(new[] 
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", uName),
                new KeyValuePair<string, string>("password", pwd)
            });

            using(HttpResponseMessage response = await apiClient.PostAsync("/Token", data))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<AuthenticatedUser>();
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
}
