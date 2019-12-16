using System;
using System.Text.RegularExpressions;
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
            GetWeatherBtn.Clicked += GetWeatherBtn_Clicked;

            this.BindingContext = new WeatherViewModel();
        }

        private async void GetWeatherBtn_Clicked(object sender, EventArgs e)
        {
            var zipCodeInput = zipCodeEntry.Text;
            if (!String.IsNullOrWhiteSpace(zipCodeInput) && IsUSorCanadianZipCode(zipCodeInput))
            {
                WeatherViewModel weather = await Core.GetWeatherViewModel(zipCodeInput);
                this.BindingContext = weather;
                GetWeatherBtn.Text = "Search Again";
            }
        }

        private bool IsUSorCanadianZipCode(string zipCode)
        {
            string pattern = @"^\d{5}-\d{4}|\d{5}|[A-Z]\d[A-Z] \d[A-Z]\d$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(zipCode);
        }
    }
}