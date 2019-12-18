using System;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;

namespace BugTracker.Tests
{
    [TestClass]
    public sealed class ProgramManagerTests : TestBase
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
        public void LoginAsPm_CreateNewProject_NewProjectCreated()
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

            Assert.IsTrue(HelperMethods.EnterTextInfoForId(WebDriver, "Name", "Testing Only"));
            Assert.IsTrue(HelperMethods.EnterTextInfoForId(WebDriver, "Description", "Project Used Solely for Automation Testing"));

            Assert.IsTrue(WebDriver.FindElements(By.Id("//input[@type='submit']")).Count > 0);
            var submitButton = WebDriver.FindElement(By.Id("//input[@type='submit']"));
            submitButton.Click();

            // TODO: Verify the new Project was created
        }
    }
}
