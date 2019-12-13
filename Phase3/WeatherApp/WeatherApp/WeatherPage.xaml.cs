using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeatherApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WeatherPage : ContentPage
    {
        public WeatherPage()
        {
            InitializeComponent();
            this.Title = "Sample Weather App";
            getWeatherBtn.Clicked += GetWeatherBtn_Clicked;

            this.BindingContext = new WeatherViewModel();
        }

        private async void GetWeatherBtn_Clicked(object sender, EventArgs e)
        {
            WeatherViewModel weather = await Core.GetWeatherViewModel("27106");
            getWeatherBtn.Text = weather.Title;
        }
    }
}