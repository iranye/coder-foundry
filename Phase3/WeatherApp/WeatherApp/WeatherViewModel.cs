using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp
{
    public class WeatherViewModel
    {
        public string Title { get; set; } = String.Empty;
        public string Temperature { get; set; } = String.Empty;
        public string Wind { get; set; } = String.Empty;
        public string Humidity { get; set; } = String.Empty;
        public string Visibility { get; set; } = String.Empty;
        public string Sunrise { get; set; } = String.Empty;
        public string Sunset { get; set; } = String.Empty;
    }
}
