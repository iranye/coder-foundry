using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace BugTracker.Tests
{
    public class TestBase
    {
        protected int SleepMs = 5000;
        protected const string Url = @"https://localhost:44306/";
        protected const string ExpectedLoggedInUrl = "https://localhost:44306/Home/Index";

        private IWebDriver _webDriver;
        public IWebDriver WebDriver
        {
            get { return _webDriver; }
            set { _webDriver = value; }
        }

        protected void Initialize()
        {
            var options = new ChromeOptions();
            options.AddArgument("--start-maximized");
            WebDriver = new ChromeDriver(options);
        }

        protected void Cleanup()
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(SleepMs));
            WebDriver.Close();
        }
    }
}
