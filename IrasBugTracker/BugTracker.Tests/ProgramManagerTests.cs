using System;
using System.Collections.ObjectModel;
using System.Threading;
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
        public void LoginAsPm_AssignTicket_TicketAssigned()
        {
            // Arrange
            var email = "dev-tester@mailinator.com";
            var password = "Test1234$";

            // Act
            HelperMethods.LoginUser(WebDriver, Url, email, password);

            // Assert - Verify there are no Notifications
            var unreadNotifcationsCount = WebDriver.FindElement(By.Id("unread-notify-count"));
            Assert.AreEqual("0", unreadNotifcationsCount.Text);
            Thread.Sleep(TimeSpan.FromMilliseconds(SleepMs));

            // Arrange
            email = "pm-tester@mailinator.com";
            password = "Test1234$";

            // Act
            HelperMethods.LoginUser(WebDriver, Url, email, password);

            // Assert
            var actualUrl = WebDriver.Url;
            actualUrl = actualUrl.Trim(new[] { '/' });
            Assert.AreEqual(ExpectedLoggedInUrl, actualUrl);
            
            // Assert My Projects has at least one item
            Assert.AreEqual(1, WebDriver.FindElements(By.Id("myProjects")).Count);
            IWebElement myProjectsLink = WebDriver.FindElement(By.Id("myProjects"));

            string expectedUrlEnd = "/Projects/AssignedIndex";
            int retryCount = 3;
            do
            {
                myProjectsLink.Click();
                Thread.Sleep(TimeSpan.FromMilliseconds(SleepMs));
                actualUrl = WebDriver.Url;
                actualUrl = actualUrl.Trim(new[] { '/' });
            } while (!actualUrl.Contains("/Projects/AssignedIndex") && retryCount-- > 0);

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

            // Go to Project Details Page
            Assert.IsTrue(testProjectExists);
            testProjectLink.Click();
            Thread.Sleep(TimeSpan.FromMilliseconds(SleepMs));
            actualUrl = WebDriver.Url;
            actualUrl = actualUrl.Trim(new[] { '/' });

            Assert.IsTrue(actualUrl.Contains("/Projects/Details/"));

            // Find the First Unassigned Ticket and Assign it to dev-tester@mailinator.com
            Assert.IsTrue(WebDriver.FindElements(By.TagName("tr")).Count > 0);

            IWebElement editLink = null;
            foreach (var tr in WebDriver.FindElements(By.TagName("tr")))
            {
                if (tr.FindElements(By.TagName("th")).Count > 0)
                {
                    var assigneeColHeader = tr.FindElements(By.TagName("th"))[3];
                    assigneeColHeader.Click();
                    Thread.Sleep(TimeSpan.FromMilliseconds(SleepMs));
                    break;
                }
            }
            foreach (var tr in WebDriver.FindElements(By.TagName("tr")))
            {
                if (tr.FindElements(By.TagName("td")).Count == 0)
                {
                    continue;
                }
                var assignee = tr.FindElements(By.TagName("td"))[3];
                if (String.IsNullOrWhiteSpace(assignee.Text))
                {
                    var tdWithActions = tr.FindElements(By.TagName("td"))[9];
                    editLink = tdWithActions.FindElements(By.TagName("a"))[0];
                    break;
                }

            }
            Assert.IsNotNull(editLink, "Could not find an Un-assigned Ticket");
            editLink.Click();
            Thread.Sleep(TimeSpan.FromMilliseconds(SleepMs));

            // Ticket Edit Page
            actualUrl = WebDriver.Url;
            actualUrl = actualUrl.Trim(new[] { '/' });
            Assert.IsTrue(actualUrl.Contains("/Tickets/Edit/"));

            Assert.AreEqual(1, WebDriver.FindElements(By.Id("AssignedToId")).Count);
            var assignedTo = WebDriver.FindElement(By.Id("AssignedToId"));
            assignedTo.Click();
            assignedTo.SendKeys(Keys.ArrowDown);
            assignedTo.SendKeys(Environment.NewLine);

            Assert.AreEqual(1, WebDriver.FindElements(By.XPath("//input[@type='submit']")).Count);
            var submitButton = WebDriver.FindElement(By.XPath("//input[@type='submit']"));
            submitButton.Click();

            actualUrl = WebDriver.Url;
            actualUrl = actualUrl.Trim(new[] { '/' });
            Assert.IsTrue(actualUrl.Contains("/Tickets/Edit/"));

            // Arrange
            email = "dev-tester@mailinator.com";
            password = "Test1234$";

            // Act
            HelperMethods.LoginUser(WebDriver, Url, email, password);
            Thread.Sleep(TimeSpan.FromMilliseconds(SleepMs));

            // Assert Dev now has a Notification
            unreadNotifcationsCount = WebDriver.FindElement(By.Id("unread-notify-count"));
            Assert.AreEqual("1", unreadNotifcationsCount.Text);

            unreadNotifcationsCount.Click();
            Assert.IsTrue(WebDriver.FindElements(By.XPath("//button[@type='submit']/*/div[@class='small text-gray-500']")).Count > 0);
            var notification = WebDriver.FindElement(By.XPath("//button[@type='submit']/*/div[@class='small text-gray-500']"));
            notification.Click();

            Thread.Sleep(TimeSpan.FromMilliseconds(SleepMs));
            unreadNotifcationsCount = WebDriver.FindElement(By.Id("unread-notify-count"));
            Assert.AreEqual("0", unreadNotifcationsCount.Text);
        }
    }
}
