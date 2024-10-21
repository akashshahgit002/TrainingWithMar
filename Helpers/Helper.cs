using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace TrainingWithAutomation
{
    public class Helper
    {
        private IWebDriver driver;
        public Helper(IWebDriver webDriver)
        {
            driver = webDriver;
        }

        /// <summary>
        /// This method capture the screenshot
        /// </summary>
        /// <param name="fileName">'fileName' to enter file name</param>
        public void TakeScreenshot(string fileName)
        {
            Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();
            string screenshotPath = Path.Combine(Directory.GetCurrentDirectory(), "Home.png");
            screenshot.SaveAsFile(screenshotPath);
            Console.WriteLine($"Screenshot is captured at: {screenshotPath}");
        }

        public string GenerateRandomString(int length = 8)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            var result = new char[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = chars[random.Next(chars.Length)];
            }
            return new string(result);
        }

        public void EnterText(By locator, string text, string elementName)
        {
            driver.FindElement(locator).Clear();
            driver.FindElement(locator).SendKeys(text);
            Console.WriteLine($"Entered text: {text} into the element name: {elementName}");
        }

        public void ClickElement(By locator, string elementName)
        {
            driver.FindElement(locator).Click();
            Console.WriteLine($"Clicked on the element name: {elementName}");
        }

        public void HoverElement(By locator, string elementName)
        {
            var element = driver.FindElement(locator);
            Actions actions = new Actions(driver);
            actions.MoveToElement(element).Perform();
            Console.WriteLine($"Hovered over element name: {elementName}");
        }

        public void DragAndDrop(By sourceLocator, By targetLocator)
        {
            var sourceElement = driver.FindElement(sourceLocator);
            var targetElement = driver.FindElement(targetLocator);
            Actions actions = new Actions(driver);
            actions.DragAndDrop(sourceElement, targetElement).Perform();
            Console.WriteLine($"Dragged element located by {sourceElement} and dropped on element located by {targetElement}");
        }

        public void EnterTextAndPressEnter(By locator, string text, string elementName)
        {
            driver.FindElement(locator).Clear();
            driver.FindElement(locator).SendKeys(text);
            driver.FindElement(locator).SendKeys(Keys.Enter);
            Console.WriteLine($"Entered text: {text} into the element name: {elementName}");
        }

        public void AssertMessage(By locator, string expectedMessage)
        {
            IWebElement messageElement = driver.FindElement(locator);
            string actualMessage = messageElement.Text;
            Assert.That(actualMessage, Is.EqualTo(expectedMessage), $"Assertion is failed: Expected {expectedMessage}, but got {actualMessage}");
        }

        public void ScrollToElement(By locator, string elementName)
        {
            try
            {
                var element = driver.FindElement(locator);
                var jsExecutor = (IJavaScriptExecutor)driver;
                jsExecutor.ExecuteScript("arguments[0].scrollIntoView(true);", element);
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(locator));
                Console.WriteLine($"Scrolled to the element name: {elementName}");
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine($"Element Name: {elementName} was not found to scroll to.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while scrolling to the element name: {elementName}. Error: {ex.Message}");
            }
        }

    }
}
