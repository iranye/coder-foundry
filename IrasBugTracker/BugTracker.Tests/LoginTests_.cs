using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace BugTracker.Tests
{
    public class LoginTests_
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
        public void Test_CanLogin_AsAdmin()
        {
            string url = @"https://localhost:44306/";
            _webDriver.Navigate().GoToUrl(url);
            var searchBox =
                _webDriver.FindElement(By.Id("exampleInputEmail"));
            searchBox.Click();
            searchBox.SendKeys("admin-tester@maillinator.com");

            // Assertions
            // url=https://localhost:44306/Home/Index
            // can see "Manage User Roles" control
        }

    }
}
