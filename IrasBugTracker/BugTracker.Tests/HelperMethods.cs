using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace BugTracker.Tests
{
    public static class HelperMethods
    {
        public static void LoginUser(IWebDriver webDriver, string url, string email, string password)
        {
            webDriver.Navigate().GoToUrl(url);
            var inputEmail = webDriver.FindElement(By.Id("exampleInputEmail"));
            inputEmail.Click();
            inputEmail.SendKeys(email);

            var inputPassword = webDriver.FindElement(By.Id("Password"));
            inputPassword.Click();
            inputPassword.SendKeys(password);

            inputPassword.SendKeys(Environment.NewLine);
        }

        public static bool EnterTextInfoForId(IWebDriver webDriver, string inputControlId, string input)
        {
            bool ret = false;
            if (webDriver.FindElements(By.Id(inputControlId)).Count > 0)
            {
                IWebElement element = webDriver.FindElement(By.Id(inputControlId));
                element.Click();
                element.SendKeys(input);
                ret = true;
            }

            return ret;
        }
    }
}
