using Newtonsoft.Json;
using SmurfWalletOW.Model;
using SmurfWalletOW.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SmurfWalletOW.Service
{
    public class OverwatchApiService : IOverwatchApiService
    {
        public Task<Profile> GetProfileAsync(string btag)
        {
             return Task.Factory.StartNew(() => GetProfile(btag));
        }

        private Profile GetProfile(string btag)
        {
            Profile profile = null;
            if (btag == null)
                return profile;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://ow-api.com/v1/stats/pc/eu/imaleo-2583/profile");
            request.Method = "GET";
            request.Accept = "application/json";

            using (HttpClient client = new HttpClient())
            {
                var urlString = string.Format("https://ow-api.com/v1/stats/pc/eu/{0}/profile", btag.Replace("#", "-"));
                var url = new Uri(urlString);
                client.BaseAddress = url;
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                   var data = response.Content.ReadAsStringAsync().Result;
                   profile = JsonConvert.DeserializeObject<Profile>(data);
                }
            }
            return profile;

        }
    }
}
