using DocumentFormat.OpenXml.Bibliography;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingWithAutomation.Helpers
{
    public class CollectionHelper
    {
        private IWebDriver driver;
        public CollectionHelper(IWebDriver webDriver)
        {
            driver = webDriver;
        }

        public List<IWebElement> GetElementsList(By locator)
        {
            List<IWebElement> elementList = new List<IWebElement>(driver.FindElements(locator));
            return elementList;
        }

        public Dictionary<string, string> GetTestData()
        {
            Dictionary<string, string> testData = new Dictionary<string, string>
            {
                { "username","standarduser"},
                { "password","sauce_demo"},
                { "url","https://www.google.com"}
            };
            return testData;
        }

        //FIFO Order
        public void QuequeData()
        {
            Queue<string> taskQueue = new Queue<string>();
            taskQueue.Enqueue("Open Browser");
            taskQueue.Enqueue("Navigating to the URL");
            taskQueue.Enqueue("Enter credentials");
            taskQueue.Enqueue("Click Login button");

            while (taskQueue.Count > 0)
            {
                string tasks = taskQueue.Dequeue();
                Console.WriteLine(tasks);
            }
        }
        //LIFO Order
        public void StackData()
        {
            Stack<string> navigationStack = new Stack<string>();
            navigationStack.Push("Home Page");
            navigationStack.Push("Product Page");
            navigationStack.Push("Checkout Page");

            while (navigationStack.Count > 0)
            {
                string page = navigationStack.Pop();
                Console.WriteLine(page);
            }
        }

        public void HashSetData()
        {
            HashSet<string> uniqieUrls = new HashSet<string>
            {
                "https://google.com",
                "https://yahoo.com",
                "https://abc.com"
            };

            foreach (string url in uniqieUrls)
            {
                Console.WriteLine(url);
            }
        }

    }
}
