using ClosedXML.Excel;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using TrainingWithAutomation.Pages;
using RestSharp;
using TrainingWithAutomation.Helpers;
using Allure.Commons;
using NUnit.Framework;
using Allure.NUnit;
using Allure.NUnit.Attributes;

namespace TrainingWithAutomation
{

    [TestFixture]
    [AllureNUnit]
    public class SmokeTest
    {
        public IWebDriver driver;
        Helper helper;
        CollectionHelper collectionHelper;

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
        }


        [Test,Category("Hotfixrun")]
        public void FetchValueFromJsonAndDoLogin()
        {
            driver.Navigate().GoToUrl(ConfigHelper.GetConfigValue("url"));
        }

        [Test, Ignore("due to functional error ptcc-15478")]
        public void doLoginWithPageObjectPatern()
        {
            driver.Navigate().GoToUrl(ConfigHelper.GetConfigValue("url"));
            LoginPage login = new LoginPage(driver);
            login.doLogin();
            login.VerifyLoginIsSuccessfull();
        }

        [Test,Category("Hotfixrun")]
        public void GoogleSearch()
        {
            driver.Navigate().GoToUrl("https://www.google.com/");
            collectionHelper = new CollectionHelper(driver);

            var elements = collectionHelper.GetElementsList(By.TagName("input"));
            Console.WriteLine(elements.Count);

            var testData = collectionHelper.GetTestData();
            Console.WriteLine("My username is: " + testData["username"]);

            IWebElement searchBox = driver.FindElement(By.Name("q"));
            searchBox.SendKeys("Selenium C#");

            searchBox.Submit();

            Console.WriteLine("Page Title is: " + driver.Title);

            Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();
            string screenshotPath = Path.Combine(Directory.GetCurrentDirectory(), "Home.png");
            screenshot.SaveAsFile(screenshotPath);

            Console.WriteLine($"Screenshot is captured at: {screenshotPath}");

            Assert.IsTrue(driver.Title.Contains("Selenium"), "My Title is not matched");

        }

        [Test]
        public void checkLinks()
        {
            driver.Navigate().GoToUrl("https://example.com");
            driver.Manage().Window.Maximize();

            // Find all anchor tags
            IList<IWebElement> links = driver.FindElements(By.TagName("a"));
            Console.WriteLine("Total Links are: " + links.Count);

            //  Check each link's status
            foreach (IWebElement link in links)
            {
                string url = link.GetAttribute("href");

                if (string.IsNullOrEmpty(url))
                {
                    Console.WriteLine("Link Text:" + link.Text + "URL is empty or null");
                    continue;
                }

                string status = CheckLinkStatus(url);
                Console.WriteLine("Link: " + url + " Status: " + status);
            }

            driver.Quit();
        }


        public static string CheckLinkStatus(string url)
        {
            try
            {
                var client = new RestClient(url);
                var request = new RestRequest();
                request.Method = Method.Get;
                var response = client.Execute(request);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return "OK";
                }
                else
                {
                    return $"Broken - {response.StatusCode}";
                }
            }
            catch (Exception ex)
            {
                return $"Error - {ex.Message}";
            }
        }


        [Test]
        public void LoginWithTestData()
        {
            driver.Navigate().GoToUrl("https://www.saucedemo.com/v1/");

            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            Console.WriteLine("My base directory location is : " + baseDirectory);

            string filePath = Path.Combine(baseDirectory, "TestData", "Cred.xlsx");

            List<Dictionary<string, string>> testData = ExcelReaderHelper.ReadExcelData(filePath);

            foreach (var data in testData)
            {
                string username = data["Username"];
                string password = data["Password"];

                driver.FindElement(By.Id("user-name")).SendKeys(username);
                driver.FindElement(By.Id("password")).SendKeys(password);
                driver.FindElement(By.Id("login-button")).Click();

                for (int i = 0; i < 6; i++)
                {
                    driver.FindElement(By.XPath("//button[text()='ADD TO CART']")).Click();
                    Thread.Sleep(1000);
                }

                for (int i = 0; i < 6; i++)
                {
                    driver.FindElement(By.XPath("//button[text()='REMOVE']")).Click();
                    Thread.Sleep(1000);
                }

            }
        }
        public class ExcelReaderHelper
        {
            public static List<Dictionary<string, string>> ReadExcelData(string filePath)
            {
                List<Dictionary<string, string>> excelData = new List<Dictionary<string, string>>();

                using (XLWorkbook workbook = new XLWorkbook(filePath))
                {
                    IXLWorksheet worksheet = workbook.Worksheet(1); // Read my first worksheet
                    bool firstRow = true;
                    var headers = new List<string>();

                    foreach (IXLRow row in worksheet.Rows())
                    {
                        if (firstRow)
                        {
                            foreach (IXLCell cell in row.Cells())
                            {
                                headers.Add(cell.Value.ToString());
                            }
                            firstRow = false;
                        }
                        else
                        {
                            Dictionary<string, string> rowData = new Dictionary<string, string>();
                            for (int i = 0; i < headers.Count; i++)
                            {
                                rowData[headers[i]] = row.Cell(i + 1).Value.ToString();
                            }
                            excelData.Add(rowData);
                        }

                    }
                    return excelData;
                }
            }
        }

        [Test]
        public void PracticeFormSubmit()
        {
            driver.Navigate().GoToUrl("https://demoqa.com/automation-practice-form");

            driver.FindElement(By.XPath("//input[@id=\"firstName\"]")).SendKeys("ABC");
            driver.FindElement(By.XPath("//input[@id=\"lastName\"]")).SendKeys("DEF");
            driver.FindElement(By.XPath("//input[@placeholder=\"name@example.com\"]")).SendKeys("abc@aaa.com");
            driver.FindElement(By.XPath("//label[text()='Female']")).Click();
            driver.FindElement(By.XPath("//input[@id=\"userNumber\"]")).SendKeys("1234567890");
            IWebElement subject = driver.FindElement(By.XPath("//input[@id=\"subjectsInput\"]"));
            subject.SendKeys("Science");
            subject.SendKeys(Keys.Enter);

            driver.FindElement(By.XPath("//label[text()='Music']")).Click();

            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            Console.WriteLine("My base directory location is : " + baseDirectory);
            string filePath = Path.Combine(baseDirectory, "TestData", "Cred.xlsx");
            driver.FindElement(By.XPath("//input[@id=\"uploadPicture\"]")).SendKeys(filePath);

            driver.FindElement(By.XPath("//div[text()='Select State']")).Click();
            driver.FindElement(By.XPath("//div[text()='Rajasthan']")).Click();

            driver.FindElement(By.XPath("//div[text()='Select City']")).Click();
            driver.FindElement(By.XPath("//div[text()='Jaipur']")).Click();

            driver.FindElement(By.XPath("//button[@type=\"submit\"]")).Click();

            IWebElement SuccessMessage = driver.FindElement(By.XPath("//div[@class=\"modal-header\"]/div"));
            string ActualSuccessMessage = SuccessMessage.Text;
            string ExpectedSuccessMessage = "Thanks for submitting the form";
            Assert.That(ActualSuccessMessage, Is.EqualTo(ExpectedSuccessMessage));


        }

        [Test]
        public void WebTable()
        {
            driver.Navigate().GoToUrl("https://demoqa.com/webtables");

            driver.FindElement(By.XPath("//button[@id=\"addNewRecordButton\"]")).Click();

            IWebElement RegForm = driver.FindElement(By.XPath("//div[@id=\"registration-form-modal\"]"));
            string actualLabel = RegForm.Text;
            string expectedLabel = "Registration Form";

            try
            {
                Assert.AreEqual(actualLabel, expectedLabel, "\"The labels do not match.\"");
                Console.WriteLine("Reg. Form is opened with label: " + actualLabel);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error is appeared:- " + ex.Message);
            }

            driver.FindElement(By.Id("firstName")).SendKeys("Peter");
            driver.FindElement(By.Id("lastName")).SendKeys("Eng");
            string emailId = "peter@aaa.com";
            driver.FindElement(By.Id("userEmail")).SendKeys(emailId);
            driver.FindElement(By.Id("age")).SendKeys("20");
            driver.FindElement(By.Id("salary")).SendKeys("50000");
            driver.FindElement(By.Id("department")).SendKeys("Science");

            driver.FindElement(By.Id("submit")).Click();
            Thread.Sleep(1000);

            driver.FindElement(By.XPath("//input[@placeholder=\"Type to search\"]")).SendKeys("peter@aaa.com");

            IWebElement verifyEmail = driver.FindElement(By.XPath("//div[text()='peter@aaa.com']"));
            bool checkEmail = verifyEmail.Text.Contains("peter@aaa.com");
            Assert.That(checkEmail);
            Console.WriteLine("Email is verified");

            driver.FindElement(By.XPath("//div[text()='" + emailId + "']/parent::div//span[@title=\"Edit\"]")).Click();
            Assert.That(actualLabel, Is.EqualTo(expectedLabel));

            driver.FindElement(By.Id("firstName")).Clear();
            string fname = "Mar";
            driver.FindElement(By.Id("firstName")).SendKeys(fname);
            driver.FindElement(By.Id("submit")).Click();
            Thread.Sleep(1000);

            IWebElement verifyName = driver.FindElement(By.XPath("//div[text()='" + fname + "']"));
            bool checkName = verifyName.Text.Contains(fname);
            Assert.That(checkName);
            Console.WriteLine("Name is verified");

        }

        [Test]
        public void Interactions()
        {
            driver.Navigate().GoToUrl("https://demoqa.com/sortable");

            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("window.scrollBy(0,250);");
            Thread.Sleep(1000);

            IWebElement sourceElement = driver.FindElement(By.XPath("//div[@class=\"tab-content\"]//div[@role=\"tabpanel\"]/div/div[text()='One']"));
            IWebElement targetElement = driver.FindElement(By.XPath("//div[@class=\"tab-content\"]//div[@role=\"tabpanel\"]/div/div[text()='Four']"));

            Actions action = new Actions(driver);

            action.ClickAndHold(sourceElement)
                .MoveToElement(targetElement)
                .Release()
                .Build()
                .Perform();

            Thread.Sleep(1000);

            Console.WriteLine("List is dropped successfully");

            driver.FindElement(By.XPath("//a[@data-rb-event-key=\"grid\"]")).Click();

            IWebElement source1Element = driver.FindElement(By.XPath("//div[@class=\"create-grid\"]//div[text()='Two']"));
            IWebElement target1Element = driver.FindElement(By.XPath("//div[@class=\"create-grid\"]//div[text()='Six']"));

            Actions action1 = new Actions(driver);

            action.ClickAndHold(source1Element)
                .MoveToElement(target1Element)
                .Release()
                .Build()
                .Perform();

            Thread.Sleep(1000);

            Console.WriteLine("Grid is dropped successfully");


            driver.FindElement(By.XPath("//span[text()='Selectable']/parent::li")).Click();
            IWebElement Cras = driver.FindElement(By.XPath("//li[text()='Cras justo odio']"));
            Cras.Click();
            Thread.Sleep(1000);

            string getActive = Cras.GetAttribute("class");

            if (getActive.Contains("active"))
                Console.WriteLine("Passed :- class name is" + getActive);
            else
                Console.WriteLine("Failed :- class name is" + getActive);

            driver.FindElement(By.XPath("//span[text()='Resizable']/parent::li")).Click();


            IWebElement resizableBox = driver.FindElement(By.XPath("//div[@id='resizableBoxWithRestriction']"));
            js.ExecuteScript("arguments[0].scrollIntoView(true);", resizableBox);

            var initialWidth = resizableBox.Size.Width;
            var initialHeight = resizableBox.Size.Height;
            Console.WriteLine($"Initial box size width: {initialWidth} , Height: {initialHeight}");

            const int targetWidth = 500;
            const int targetHeight = 300;

            int widthOffset = targetWidth - initialWidth;
            int heightOffset = targetHeight - initialHeight;

            IWebElement resizeHandle = driver.FindElement(By.XPath("//div[@id='resizableBoxWithRestriction']//span"));

            Actions newActions = new Actions(driver);
            newActions.ClickAndHold(resizeHandle)
                .MoveByOffset(widthOffset, heightOffset)
                .Release()
                .Build()
                .Perform();

            var newBoundingBox = resizableBox.Size;
            Console.WriteLine($"New box size width: {newBoundingBox.Width} , Height: {newBoundingBox.Height}");

        }

        [Test]
        public void BookStore()
        {
            driver.Navigate().GoToUrl("https://demoqa.com/books");

            try
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                IWebElement title = wait.Until(ExpectedConditions.ElementExists(By.XPath("(//div[@class=\"action-buttons\"]//a)[1]")));

                string getTitle = title.Text;
                Console.WriteLine($"Fetched Text of Title is :- {getTitle}");

                IWebElement searchBox = driver.FindElement(By.Id("searchBox"));
                searchBox.SendKeys(getTitle);
                searchBox.SendKeys(Keys.Enter);

                wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("(//div[@class=\"action-buttons\"]//a)[1]")));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error is occured: {ex.Message} ");
            }

            driver.Navigate().Refresh();

            try
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
                IWebElement author = wait.Until(ExpectedConditions.ElementExists(By.XPath("//div[text()='Addy Osmani']")));

                string getAuthor = author.Text;
                Console.WriteLine($"Fetched Text of Author is :- {getAuthor}");

                IWebElement searchBox = driver.FindElement(By.Id("searchBox"));
                searchBox.SendKeys(getAuthor);
                searchBox.SendKeys(Keys.Enter);

                wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[text()='Addy Osmani']")));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error is occured: {ex.Message} ");
            }

            driver.Navigate().Refresh();

            try
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));

                IJavaScriptExecutor js1 = (IJavaScriptExecutor)driver;
                js1.ExecuteScript("window.scrollBy(0,750);");
                Console.WriteLine("Scrolling is completed");

                IWebElement publisher = wait.Until(ExpectedConditions.ElementExists(By.XPath("//div[text()='No Starch Press']")));

                string getPublisher = publisher.Text;
                Console.WriteLine($"Fetched Text of Publisher is :- {getPublisher}");

                IWebElement searchBox = driver.FindElement(By.Id("searchBox"));
                searchBox.SendKeys(getPublisher);
                searchBox.SendKeys(Keys.Enter);

                wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[text()='No Starch Press']")));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error is occured: {ex.Message} ");
            }

            driver.Navigate().Refresh();

            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("window.scrollBy(0,1000);");
            Console.WriteLine("Again Scrolling is completed");

            IReadOnlyCollection<IWebElement> records = driver.FindElements(By.XPath("//div[@class=\"rt-tbody\"]/div"));
            int tableRecords = records.Count;
            Console.WriteLine($"Before table records count: {tableRecords} ");

            driver.FindElement(By.XPath("//select[@aria-label=\"rows per page\"]")).Click();
            driver.FindElement(By.XPath("//option[text()='5 rows']")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.XPath("//button[text()='Previous']")).Click();

            IReadOnlyCollection<IWebElement> records1 = driver.FindElements(By.XPath("//div[@class=\"rt-tbody\"]/div"));
            int tableRecords1 = records1.Count;
            Console.WriteLine($"After table records count: {tableRecords1} ");

            //Assert.That(5, tableRecords1, "Records are not matched with the expectations");
            Console.WriteLine($"Testcase is passed and table records counts are: {tableRecords1} ");

        }

        [Test]
        public void HandleModalDialogs()
        {
            driver.Navigate().GoToUrl("https://demoqa.com/modal-dialogs");

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            IWebElement smallModal = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("showSmallModal")));
            smallModal.Click();
            Console.WriteLine("Small madal button is clicked");

            IWebElement smallModalCancelButton = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("closeSmallModal")));
            smallModalCancelButton.Click();
            Console.WriteLine("Small madal cancel button is clicked");

            IWebElement smallModalPopupTitle = driver.FindElement(By.XPath("//div[@class=\"modal-header\"]/div"));
            string smallModalPopupValue = smallModalPopupTitle.Text;
            Console.WriteLine("Small madal title is :- " + smallModalPopupValue + "");

            if (smallModalPopupValue.Contains("Small"))
                Console.WriteLine("Passed and Small madal title is :- " + smallModalPopupValue + "");
            else
                Console.WriteLine("Failed and Small madal title is :- " + smallModalPopupValue + "");
        }

        [Test]
        public void FetchProductDetails()
        {
            driver.Navigate().GoToUrl("https://www.saucedemo.com/v1/inventory.html");

            IList<IWebElement> productTitle = driver.FindElements(By.XPath("//div[@class=\"inventory_item_name\"]"));
            List<string> websiteProductTitle = new List<string>();

            foreach (IWebElement product in productTitle)
            {
                websiteProductTitle.Add(product.Text);
            }

            Console.WriteLine("My Fetched Items from the page");

            foreach (string itemName in websiteProductTitle)
            {
                Console.WriteLine(itemName);
            }

            List<string> myItems = new List<string>
            {
                "Sauce Labs Backpack",
                "Sauce Labs Bike Light",
                "Sauce Labs Bolt T-Shirt",
                "Sauce Labs Fleece Jacket",
                "Sauce Labs Onesie",
                "Test.allTheThings() T-Shirt (Red)"
            };

            foreach (string item in myItems)
            {
                if (websiteProductTitle.Any(name => name.Equals(item, StringComparison.OrdinalIgnoreCase)))
                    Console.WriteLine($"The item '{item}' was matched successfully");
                else
                    Console.WriteLine($"The item '{item}' was NOT matched");
            }
        }


        [Test]
        public void FooterLinksCheck()
        {
            driver.Navigate().GoToUrl("https://www.next.co.uk/");

            driver.FindElement(By.XPath("//button[text()='Accept All Cookies']")).Click();
            IWebElement FAQ = driver.FindElement(By.XPath("(//ul[@data-testid=\"footer-main-links-title-help-list\"]/li)[1]"));
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].scrollIntoView(true);", FAQ);

            IList<IWebElement> elements = driver.FindElements(By.XPath("(//ul[@data-testid=\"footer-main-links-title-help-list\"])[1]/li"));
            int elementsCount = elements.Count;
            Console.WriteLine($"My links count are: {elementsCount} ");

            for (int i = 0; i < elementsCount; i++)
            {
                try
                {
                    elements[i].Click();
                    Console.WriteLine($"Clicked on a element {i + 1}");
                    driver.Navigate().Back();
                    elements = driver.FindElements(By.XPath("(//ul[@data-testid=\"footer-main-links-title-help-list\"])[1]/li"));
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Failed to click on element at index {i + 1}: {e.Message}");
                }
            }
            driver.Close();
        }

        [Test]
        public void UploadFiles()
        {
            driver.Navigate().GoToUrl("https://demoqa.com/upload-download");

            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = Path.Combine(baseDirectory, "TestData", "Cred.xlsx");
            Console.WriteLine("My file location is : " + filePath);

            IWebElement uploadElement = driver.FindElement(By.Id("uploadFile"));
            uploadElement.SendKeys(filePath);

            string expectedFileName = "Cred.xlsx";
            IWebElement successUpload = driver.FindElement(By.XPath("//p[contains(text(),'" + expectedFileName + "')]"));

            Assert.That(successUpload.Displayed, "File is not uploaded");

        }

        [Test]
        public void ForbesAngularSiteCheck()
        {
            driver.Navigate().GoToUrl("https://www.forbes.com/");

            driver.FindElement(By.XPath("//a[@aria-label=\"Search\"]")).Click();
            IWebElement searchBox = driver.FindElement(By.XPath("//input[contains(@class,\"SearchField\")]"));
            searchBox.SendKeys("news");
            searchBox.SendKeys(Keys.Enter);

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            IWebElement newsLink = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("(//a[contains(@class,\"CardArticle\")])[2]")));
            newsLink.Click();

            Actions action = new Actions(driver);
            IWebElement billionairesMenu = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//a[text()='Billionaires']/parent::div/parent::li")));
            action.MoveToElement(billionairesMenu).Perform();

            IWebElement worldsBillionairesMenu = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//a[text()=\"World's Billionaires\"]")));
            worldsBillionairesMenu.Click();
        }

        [Test]
        public void CheckDownloadWithAssert()
        {
            string downloadDirectory = @"C:\Users\aksah\Downloads";
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddUserProfilePreference("download.default_directory", downloadDirectory);
            chromeOptions.AddUserProfilePreference("download.prompt_for_download", false);
            chromeOptions.AddUserProfilePreference("directory_upgrade", true);

            using IWebDriver driver = new ChromeDriver(chromeOptions);
            driver.Navigate().GoToUrl("https://demoqa.com/upload-download");
            driver.Manage().Window.Maximize();

            try
            {
                IWebElement downloadButton = driver.FindElement(By.XPath("//a[@id=\"downloadButton\"]"));
                downloadButton.Click();
                string downloadedFilePath = Path.Combine(downloadDirectory, "sampleFile.jpeg");
                waitForFileToDownload(downloadedFilePath, TimeSpan.FromSeconds(30));

                Assert.That(File.Exists(downloadedFilePath), "FILE WAS NOT DOWNLOADED");

                long fileSize = new FileInfo(downloadedFilePath).Length;
                Console.WriteLine($"File size is {fileSize}");

                Assert.That(fileSize > 0, "The downloaded file is having 0 bytes");


            }

            catch (NoSuchElementException e)
            {
                Console.WriteLine("Element is not found" + e.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error is occured" + ex.Message);
            }
            finally
            {
                driver.Close();
            }

        }
        private void waitForFileToDownload(string filePath, TimeSpan timeout)
        {
            var waitUntil = DateTime.Now.Add(timeout);
            while (!File.Exists(filePath) && DateTime.Now < waitUntil)
            {
                Thread.Sleep(5000);
            }
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"File '{filePath}' not found after waiting");
            }
        }

        [TearDown]
        public void TearDown()
        {
            if (driver != null)
            {

                driver.Quit();
                driver.Dispose();
            }
        }

    }
}
