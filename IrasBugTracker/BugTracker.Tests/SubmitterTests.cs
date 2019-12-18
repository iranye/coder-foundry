using System;
using System.Collections.ObjectModel;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace BugTracker.Tests
{
    [TestClass]
    public sealed class SubmitterTests : TestBase
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
        public void LoginAsSubmitter_CreateNewTicket_NewTicketCreated()
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

            // Assert My Projects has at least one item
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
            var dateTimeStamp = $"{DateTime.Now.ToString("yyyyMddHHmmss")}";

            Thread.Sleep(TimeSpan.FromMilliseconds(SleepMs));
            var ticketTitle = "Test Defect";
            Assert.IsTrue(HelperMethods.EnterTextInfoForId(WebDriver, "Title", ticketTitle + " " + dateTimeStamp));
            Assert.IsTrue(HelperMethods.EnterTextInfoForId(WebDriver, "Description", "Test Defect Description"));
            Assert.IsTrue(WebDriver.FindElements(By.Id("ProjectId")).Count > 0);
            var projectId = WebDriver.FindElement(By.Id("ProjectId"));
            projectId.Click();
            projectId.SendKeys(Keys.ArrowDown);
            projectId.SendKeys(Environment.NewLine);

            Assert.IsTrue(WebDriver.FindElements(By.Id("TicketTypeId")).Count > 0);
            var ticketType = WebDriver.FindElement(By.Id("TicketTypeId"));
            ticketType.Click();
            ticketType.SendKeys(Keys.ArrowDown);
            ticketType.SendKeys(Environment.NewLine);

            Assert.IsTrue(WebDriver.FindElements(By.XPath("//input[@type='submit']")).Count > 0);
            var submitButton = WebDriver.FindElement(By.XPath("//input[@type='submit']"));
            submitButton.Click();

            actualUrl = WebDriver.Url;
            actualUrl = actualUrl.Trim(new[] { '/' });

            expectedUrlEnd = "/Tickets/Index";
            Assert.IsTrue(actualUrl.EndsWith(expectedUrlEnd));

            Assert.IsTrue(WebDriver.FindElements(By.TagName("td")).Count > 0);
            bool newTicketExists = false;
            foreach (var td in WebDriver.FindElements(By.TagName("td")))
            {
                if (td.Text.Contains(ticketTitle) && td.Text.Contains(dateTimeStamp))
                {
                    newTicketExists = true;
                    break;
                }
            }
            Assert.IsTrue(newTicketExists);
        }
    }
}
