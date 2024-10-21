using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingWithAutomation.Pages
{
    public class LoginPage
    {
        public IWebDriver driver;
        AssertionHelper assertionHelper;
        Helper helper;
        #region Login Page Controllers
        private By usernameTextbox = By.Id("user-name");
        private By PasswordTextbox = By.Id("password");
        private By LoginButton = By.Id("login-button");
        private By ItemName = By.XPath("(//div[@class=\"inventory_item_name\"])[1]");
        #endregion


        #region Login Page Constructors
        public LoginPage(IWebDriver webDriver)
        {
            driver = webDriver;
            assertionHelper = new AssertionHelper(driver);
            helper = new Helper(driver);
        }
        #endregion

        #region Login Page Action Methods
        public void doLogin()
        {
            helper.EnterText(usernameTextbox, "standard_user", "Username");
            helper.EnterText(PasswordTextbox, "standard_user", "Password");
            helper.ClickElement(LoginButton, "Login Button");
        }
        #endregion

        #region Login Page Assertion Methods
        public void VerifyLoginIsSuccessfull()
        {
            assertionHelper.IsElementVisible(ItemName, "Sauce Labs Backpack");
        }

        #endregion
    }
}