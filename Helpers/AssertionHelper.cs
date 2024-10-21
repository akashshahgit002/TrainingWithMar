using DocumentFormat.OpenXml.Bibliography;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingWithAutomation
{
    public class AssertionHelper
    {
        private IWebDriver driver;
        public AssertionHelper(IWebDriver webDriver)
        {
            driver = webDriver;
        }
        public bool IsElementVisible(By locator, string elementName)
        {
            try
            {
                bool isVisible = driver.FindElement(locator).Displayed;
                Console.WriteLine($"Element Name: {elementName} is visible {isVisible}");
                return isVisible;
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine($"Element Name: {elementName} is not visible");
                return false;
            }
        }

        public bool IsElementEnabled(By locator, string elementName)
        {
            bool isEnabled = driver.FindElement(locator).Displayed;
            Console.WriteLine($"Element Name: {elementName} is enabled {isEnabled}");
            return isEnabled;
        }

        public bool IsElementDisplayed(By locator, string elementName)
        {
            bool isDisplayed = driver.FindElement(locator).Displayed;
            Console.WriteLine($"Element Name: {elementName} is displayed {isDisplayed}");
            return isDisplayed;
        }

        public void WaitForElementToBeClickable(By locator, string elementName, int timeoutInSeconds = 10)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(locator));
            Console.WriteLine($"Element Name: {elementName} is now clickable");
        }
    }
}
