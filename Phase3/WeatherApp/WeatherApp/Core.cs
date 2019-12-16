using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WeatherApp
{
    public class Core
    {
        public static async Task<WeatherViewModel> GetWeatherViewModel(string zipCode)
        {
            string openWeatherkey = "bab095c7df7c83c287d14b64558c6fcf";
            string queryString = $@"https://api.openweathermap.org/data/2.5/weather?zip={zipCode},us&appid={openWeatherkey}&units=imperial";

            string results = await DataService.GetDataFromService(queryString).ConfigureAwait(false);

            WeatherViewModel viewModel = null;
            if (!String.IsNullOrWhiteSpace(results))
            {
                Rootobject weatherData = JsonConvert.DeserializeObject<Rootobject>(results);

                DateTime time = new System.DateTime(1970,1,1,0,0,0,0);
                DateTime sunrise = time.AddSeconds((double) weatherData.sys.sunrise);
                DateTime sunset = time.AddSeconds((double)weatherData.sys.sunset);
                DateTime sunriseLocal = sunrise.ToLocalTime();
                DateTime sunsetLocal = sunset.ToLocalTime();

                viewModel = new WeatherViewModel
                {
                    Title = weatherData.name,
                    Temperature = weatherData.main.temp.ToString(),
                    Wind = weatherData.wind.speed.ToString(),
                    Humidity = weatherData.main.humidity.ToString(),
                    Visibility = weatherData.visibility.ToString(),
                    Sunrise = sunriseLocal.ToString("hh:mm"),
                    Sunset = sunsetLocal.ToString("hh:mm")
                };
            }


            return viewModel;
        }
    }
}
