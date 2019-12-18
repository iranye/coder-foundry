using System;
using System.Collections.ObjectModel;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace BugTracker.Tests
{
    [TestClass]
    public class LoginTests
    {
        private IWebDriver _webDriver;
        private int _sleepMs = 2000;
        private const string _url = @"https://localhost:44306/";
        private const string _expectedLoggedInUrl = "https://localhost:44306/Home/Index";

        [TestInitialize]
        public void TestInitialize()
        {
            var options = new ChromeOptions();
            options.AddArgument("--start-maximized");
            _webDriver = new ChromeDriver(options);
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
            // Arrange
            _webDriver.Navigate().GoToUrl(_url);

            // Act
            var inputEmail =
                _webDriver.FindElement(By.Id("exampleInputEmail"));
            inputEmail.Click();
            inputEmail.SendKeys("auto-tester@maillinator.com");

            var inputPassword = _webDriver.FindElement(By.Id("Password"));
            inputPassword.Click();
            inputPassword.SendKeys("Test1234$");

            inputPassword.SendKeys(Environment.NewLine);
            var actualUrl = _webDriver.Url;
            actualUrl = actualUrl.Trim(new[] { '/' });

            // Assert
            Assert.AreEqual(_expectedLoggedInUrl, actualUrl);
        }

        [TestMethod]
        public void Test_AdminLoggedIn_CanPerformRoleChanges()
        {
            // Arrange
            string url = @"https://localhost:44306/";
            _webDriver.Navigate().GoToUrl(url);

            // Act
            var inputEmail =
                _webDriver.FindElement(By.Id("exampleInputEmail"));
            inputEmail.Click();
            inputEmail.SendKeys("auto-tester@maillinator.com");

            var inputPassword = _webDriver.FindElement(By.Id("Password"));
            inputPassword.Click();
            inputPassword.SendKeys("Test1234$");

            inputPassword.SendKeys(Environment.NewLine);

            var actualUrl = _webDriver.Url;
            actualUrl = actualUrl.Trim(new[] { '/' });
            Assert.AreEqual(_expectedLoggedInUrl, actualUrl);

            var expectedText = "Manage User Roles";
            IWebElement firstNavlinkItem = null;
            string navLinkListItemsXpath = "//li[@class='nav-item']";
            bool foundManageRolesLink = false;
            if (_webDriver.FindElements(By.XPath(navLinkListItemsXpath)).Count > 0)
            {
                ReadOnlyCollection<IWebElement> listItems = _webDriver.FindElements(By.XPath(navLinkListItemsXpath));
                foreach (var li in listItems)
                {
                    if (li.Text == expectedText)
                    {
                        foundManageRolesLink = true;
                        break;
                    }
                }
            }
            Assert.IsTrue(foundManageRolesLink);
        }
    }
}
