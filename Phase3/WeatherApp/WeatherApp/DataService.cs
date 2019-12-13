using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WeatherApp
{
    public class DataService
    {
        public static async Task<String> GetDataFromService(string queryString)
        {
            var client = new HttpClient();
            var response = await client.GetAsync(queryString);
            string data = String.Empty;
            if (response != null && response.StatusCode == HttpStatusCode.OK)
            {
                data = response.Content.ReadAsStringAsync().Result;
            }

            return data;
        }
    }
}
