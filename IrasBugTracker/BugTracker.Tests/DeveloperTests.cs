using System;
using System.Security.Policy;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;

namespace BugTracker.Tests
{
    [TestClass]
    public class DeveloperTests : TestBase
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
        public void LoginAsDev_NavigateToTicketEdit_TicketEditPageShown()
        {
            // Arrange
            var email = "dev-tester@mailinator.com";
            var password = "Test1234$";

            // Act
            HelperMethods.LoginUser(WebDriver, Url, email, password);
            
            // Assert
            var actualUrl = WebDriver.Url;
            actualUrl = actualUrl.Trim(new[] { '/' });
            Assert.AreEqual(ExpectedLoggedInUrl, actualUrl);
            Thread.Sleep(TimeSpan.FromMilliseconds(SleepMs));

            var expectedText = "My Projects";
            string myProjectsXpath = "//*[text()='My Projects']";

            // Assert My Projects has at least one item
            Assert.IsTrue(WebDriver.FindElements(By.XPath(myProjectsXpath)).Count > 0);
            IWebElement myProjectsLink = WebDriver.FindElement(By.XPath(myProjectsXpath));
            myProjectsLink.Click();
            Thread.Sleep(TimeSpan.FromMilliseconds(SleepMs));

            actualUrl = WebDriver.Url;
            actualUrl = actualUrl.Trim(new[] { '/' });
            string expectedUrlEnd = "/Projects/AssignedIndex";
            Assert.IsTrue(actualUrl.EndsWith(expectedUrlEnd));

            Assert.IsTrue(WebDriver.FindElements(By.TagName("td")).Count > 0);
            bool testProjectExists = false;
            string projectTitle = "Testing Only";
            IWebElement testProjectLink = null;
            foreach (var td in WebDriver.FindElements(By.TagName("td")))
            {
                if (td.Text.Contains(projectTitle))
                {
                    testProjectExists = true;
                    testProjectLink = td.FindElement(By.TagName("a"));
                    break;
                }
            }

            Assert.IsTrue(testProjectExists);

            testProjectLink.Click();
            Thread.Sleep(TimeSpan.FromMilliseconds(SleepMs));

            // Project Details Page
            actualUrl = WebDriver.Url;
            actualUrl = actualUrl.Trim(new[] { '/' });
            Assert.IsTrue(actualUrl.Contains("/Projects/Details/"));

            // Find the First Unassigned Ticket and Assign it to dev-tester@mailinator.com
            Assert.IsTrue(WebDriver.FindElements(By.TagName("tr")).Count > 0);
            IWebElement link = null;
            foreach (var tr in WebDriver.FindElements(By.TagName("tr")))
            {
                if (tr.FindElements(By.TagName("td")).Count == 0)
                {
                    continue;
                }
                var assignee = tr.FindElements(By.TagName("td"))[3];
                if (!String.IsNullOrWhiteSpace(assignee.Text))
                {
                    var linkToTicketDash = tr.FindElements(By.TagName("td"))[0];
                    link = linkToTicketDash.FindElements(By.TagName("a"))[0];
                    break;
                }

            }
            Assert.IsNotNull(link, "Could not find an Assigned Ticket");
            link.Click();
            Thread.Sleep(TimeSpan.FromMilliseconds(SleepMs));

            // Ticket Dashboard Page
            actualUrl = WebDriver.Url;
            actualUrl = actualUrl.Trim(new[] { '/' });
            Assert.IsTrue(actualUrl.Contains("/Tickets/Dashboard/"));

            var editLink = WebDriver.FindElement(By.XPath("//a[text()='Edit']"));
            editLink.Click();

            // Ticket Dashboard Page
            actualUrl = WebDriver.Url;
            actualUrl = actualUrl.Trim(new[] { '/' });
            Assert.IsTrue(actualUrl.Contains("/Tickets/Edit/"));
        }
    }
}
