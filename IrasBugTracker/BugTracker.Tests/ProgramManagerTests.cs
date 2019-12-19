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
        public void LoginAsPm_CreateNewProject_NewProjectCreated()
        {
            // Arrange
            var email = "pm-tester@mailinator.com";
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

            // Arrange
            email = "pm-tester@mailinator.com";
            password = "Test1234$";

            // Act
            HelperMethods.LoginUser(WebDriver, Url, email, password);

            // Assert
            var actualUrl = WebDriver.Url;
            actualUrl = actualUrl.Trim(new[] { '/' });
            Assert.AreEqual(ExpectedLoggedInUrl, actualUrl);

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
            bool unassignedTicketExists = false;
            string linkText = "Edit";
            IWebElement editLink = null;
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
            Assert.IsNotNull(editLink);
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
