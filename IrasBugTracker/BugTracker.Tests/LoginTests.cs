using System;
using System.Collections.ObjectModel;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace BugTracker.Tests
{
    [TestClass]
    public sealed class LoginTests : TestBase
    {
        [TestInitialize]
        public void TestInitialize()
        {
            base.Initialize();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            base.Cleanup();
        }

        [TestMethod]
        public void Test_CanLogin_AsAdmin()
        {
            // Arrange
            var email = "auto-tester@maillinator.com";
            var password = "Test1234$";

            // Act
            HelperMethods.LoginUser(WebDriver, Url, email, password);

            // Assert
            var actualUrl = WebDriver.Url;
            actualUrl = actualUrl.Trim(new[] { '/' });

            // Assert
            Assert.AreEqual(ExpectedLoggedInUrl, actualUrl);
        }

        [TestMethod]
        public void Test_AdminLoggedIn_CanNavigateToPerformRoleChangesPage()
        {
            // Arrange
            var email = "auto-tester@maillinator.com";
            var password = "Test1234$";

            // Act
            HelperMethods.LoginUser(WebDriver, Url, email, password);

            // Assert
            var actualUrl = WebDriver.Url;
            actualUrl = actualUrl.Trim(new[] { '/' });
            Assert.AreEqual(ExpectedLoggedInUrl, actualUrl);

            var expectedText = "Manage User Roles";
            IWebElement roleSpecificLink = null;
            string navLinkListItemsXpath = "//li[@class='nav-item']";
            bool foundRoleSpecificLink = false;

            if (WebDriver.FindElements(By.XPath(navLinkListItemsXpath)).Count > 0)
            {
                ReadOnlyCollection<IWebElement> listItems = WebDriver.FindElements(By.XPath(navLinkListItemsXpath));
                foreach (var li in listItems)
                {
                    if (li.Text == expectedText)
                    {
                        foundRoleSpecificLink = true;
                        roleSpecificLink = li;
                        break;
                    }
                }
            }
            Assert.IsTrue(foundRoleSpecificLink);
            roleSpecificLink.Click();
            actualUrl = WebDriver.Url;
            actualUrl = actualUrl.Trim(new[] { '/' });
            string expectedUrlEnd = "/ManageUsers/ManageRoles";
            Assert.IsTrue(actualUrl.EndsWith(expectedUrlEnd));
        }

        [TestMethod]
        public void Test_CanLogin_AsProjectManager()
        {
            // Arrange
            var email = "pm-tester@maillinator.com";
            var password = "Test1234$";

            // Act
            HelperMethods.LoginUser(WebDriver, Url, email, password);

            // Assert
            var actualUrl = WebDriver.Url;
            actualUrl = actualUrl.Trim(new[] { '/' });

            // Assert
            Assert.AreEqual(ExpectedLoggedInUrl, actualUrl);
        }

        [TestMethod]
        public void Test_PmLoggedIn_CanNavigateToCreateNewProjectPage()
        {
            // Arrange
            var email = "pm-tester@maillinator.com";
            var password = "Test1234$";

            // Act
            HelperMethods.LoginUser(WebDriver, Url, email, password);

            // Assert
            var actualUrl = WebDriver.Url;
            actualUrl = actualUrl.Trim(new[] { '/' });
            Assert.AreEqual(ExpectedLoggedInUrl, actualUrl);

            var expectedText = "New Project";
            IWebElement roleSpecificLink = null;
            string navLinkListItemsXpath = "//li[@class='nav-item']";
            bool foundRoleSpecificLink = false;

            if (WebDriver.FindElements(By.XPath(navLinkListItemsXpath)).Count > 0)
            {
                ReadOnlyCollection<IWebElement> listItems = WebDriver.FindElements(By.XPath(navLinkListItemsXpath));
                foreach (var li in listItems)
                {
                    if (li.Text == expectedText)
                    {
                        foundRoleSpecificLink = true;
                        roleSpecificLink = li;
                        break;
                    }
                }
            }
            Assert.IsTrue(foundRoleSpecificLink);
            roleSpecificLink.Click();
            actualUrl = WebDriver.Url;
            actualUrl = actualUrl.Trim(new[] { '/' });
            string expectedUrlEnd = "/Projects/Create";
            Assert.IsTrue(actualUrl.EndsWith(expectedUrlEnd));
        }

        [TestMethod]
        public void Test_CanLogin_AsDeveloper()
        {
            // Arrange
            var email = "dev-tester@maillinator.com";
            var password = "Test1234$";

            // Act
            HelperMethods.LoginUser(WebDriver, Url, email, password);

            // Assert
            var actualUrl = WebDriver.Url;
            actualUrl = actualUrl.Trim(new[] { '/' });

            // Assert
            Assert.AreEqual(ExpectedLoggedInUrl, actualUrl);
        }

        [TestMethod]
        public void Test_DevLoggedIn_CanNotCreateNewProjectOrTicket()
        {
            // Arrange
            var email = "dev-tester@maillinator.com";
            var password = "Test1234$";

            // Act
            HelperMethods.LoginUser(WebDriver, Url, email, password);
            
            // Assert
            var actualUrl = WebDriver.Url;
            actualUrl = actualUrl.Trim(new[] { '/' });
            Assert.AreEqual(ExpectedLoggedInUrl, actualUrl);

            var expectedText = "New Project";
            IWebElement roleSpecificLink = null;
            string navLinkListItemsXpath = "//li[@class='nav-item']";
            bool foundRoleSpecificLink = false;

            if (WebDriver.FindElements(By.XPath(navLinkListItemsXpath)).Count > 0)
            {
                ReadOnlyCollection<IWebElement> listItems = WebDriver.FindElements(By.XPath(navLinkListItemsXpath));
                foreach (var li in listItems)
                {
                    if (li.Text == expectedText)
                    {
                        foundRoleSpecificLink = true;
                        roleSpecificLink = li;
                        break;
                    }
                }
            }
            Assert.IsFalse(foundRoleSpecificLink);

            if (WebDriver.FindElements(By.XPath("//a[@class='nav-link']")).Count > 0)
            {
                ReadOnlyCollection<IWebElement> anchors = WebDriver.FindElements(By.XPath(navLinkListItemsXpath));
                foreach (var a in anchors)
                {
                    if (a.GetAttribute("href") != null && a.GetAttribute("href").EndsWith("/Tickets/Create")
                        || a.Text.Contains(expectedText))
                    {
                        foundRoleSpecificLink = true;
                        roleSpecificLink = a;
                        break;
                    }
                }
            }
            Assert.IsFalse(foundRoleSpecificLink);
        }

        [TestMethod]
        public void Test_CanLogin_AsSubmitter()
        {
            // Arrange
            var email = "submitter-tester@maillinator.com";
            var password = "Test1234$";

            // Act
            HelperMethods.LoginUser(WebDriver, Url, email, password);

            // Assert
            var actualUrl = WebDriver.Url;
            actualUrl = actualUrl.Trim(new[] { '/' });

            Assert.AreEqual(ExpectedLoggedInUrl, actualUrl);
        }

        [TestMethod]
        public void Test_SubmitterLoggedIn_CanGoToCreateNewTicketPage()
        {
            // Arrange
            var email = "submitter-tester@maillinator.com";
            var password = "Test1234$";

            // Act
            HelperMethods.LoginUser(WebDriver, Url, email, password);

            var actualUrl = WebDriver.Url;
            actualUrl = actualUrl.Trim(new[] { '/' });
            Assert.AreEqual(ExpectedLoggedInUrl, actualUrl);

            var expectedText = "New Ticket";
            IWebElement roleSpecificLink = null;

            string newTicketXpath = "//*[text()='New Ticket']";

            bool foundRoleSpecificLink = false;

            if (WebDriver.FindElements(By.XPath(newTicketXpath)).Count > 0)
            {
                ReadOnlyCollection<IWebElement> items = WebDriver.FindElements(By.XPath(newTicketXpath));
                foreach (var li in items)
                {
                    if (li.Text == expectedText)
                    {
                        foundRoleSpecificLink = true;
                        roleSpecificLink = li;
                        break;
                    }
                }
            }
            Assert.IsTrue(foundRoleSpecificLink);
            roleSpecificLink.Click();
            actualUrl = WebDriver.Url;
            actualUrl = actualUrl.Trim(new[] { '/' });

            string expectedUrlEnd = "/Tickets/Create";
            Assert.IsTrue(actualUrl.EndsWith(expectedUrlEnd));
        }
    }
}
