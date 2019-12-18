using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace BugTracker.Tests
{
    [TestClass]
    public class UnitTest1
    {
        private IWebDriver _webDriver;
        private int _sleepMs = 2000;

        [TestInitialize]
        public void TestInitialize()
        {
            _webDriver = new ChromeDriver();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(_sleepMs));
            _webDriver.Close();
        }

        [TestMethod]
        public void TestMethod1()
        {
            string url = @"https://gatherer.wizards.com/Pages/Default.aspx";
            _webDriver.Navigate().GoToUrl(url);
            var searchBox =
                _webDriver.FindElement(By.Id("ctl00_ctl00_MainContent_Content_SearchControls_CardSearchBoxParent_CardSearchBox"));

            searchBox.Clear();
            searchBox.SendKeys("Brass Man");

        }
    }
}
