using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using IraNye.WebApi.Models;
using Newtonsoft.Json;

namespace IraNye.WebApi.Test
{
    class Program
    {
        static async Task Main(string[] args)
        {
            List<Household> allHouseholds = await HouseholdWaiter.ServerAllHouseholdData();
            Console.WriteLine(String.Format("allHouseholds: {0}", allHouseholds == null ? "null" : allHouseholds.Count.ToString()));
            Console.ReadLine();
        }
    }

    public class HouseholdWaiter
    {
        public static async Task<List<Household>> ServerAllHouseholdData()
        {
            // Scotty endpoint: @"https://scottyapi.azurewebsites.net/api/Households/GetAllHouseholdDataAsJson";
            var endpoint = @"https://scottyapi.azurewebsites.net/api/Households/GetAllHouseholdDataAsJson";
            // var endpoint =  @"https://localhost:44315/Api/Households/GetAllHouseholds";
            // var endpoint =  @"https://iranyewebapi.azurewebsites.net/Api/Households/GetAllHouseholds";

            var dynamic = await DataService.GetDataFromServiceAsync(endpoint).ConfigureAwait(false);
            var allHouseholds = // JsonConvert.DeserializeObject<List<Household>>(dynamic);
                dynamic.ToObject(typeof(List<Household>));
            return allHouseholds;
        }
    }

    public class DataService
    {
        public static async Task<dynamic> GetDataFromServiceAsync(string queryString)
        {
            var client = new HttpClient();
            var response = await client.GetAsync(queryString);
            dynamic data = null;
            if (response != null)
            {
                string json = response.Content.ReadAsStringAsync().Result;
                data = JsonConvert.DeserializeObject(json);
            }

            return data;
        }

        public static dynamic GetDataFromService(string queryString)
        {
            var client = new HttpClient();
            var response = client.GetAsync(queryString);
            dynamic data = null;
            if (response != null)
            {
                // string json = response.Content.ReadAsStringAsync().Result;

                var json = response.Result.Content.ToString();
                data = JsonConvert.DeserializeObject(json);
            }

            return data;
        }
    }
}
