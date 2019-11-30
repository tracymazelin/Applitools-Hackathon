using OpenQA.Selenium;

namespace Hackathon.TraditionalTests.Pages
{
    //All of the page interactions happen in this page object class.  That way, if the elements change, we only have to change them here.
    //Assertions don't happen here, they happen in the tests
    //Contains state (locators) and behavior(methods)
    public class LoginPage : BasePage
    {
        private readonly IWebDriver _driver;

        public LoginPage(IWebDriver driver) : base(driver)
        {
            this._driver = driver;
        }

        #region Public Properties

        public IWebElement TxtUserName => _driver.FindElement(By.Id("username"));
        public IWebElement TxtPassword => _driver.FindElement(By.Id("password"));
        public IWebElement BtnLogIn => _driver.FindElement(By.Id("log-in"));
        public IWebElement IconUsername => _driver.FindElement(By.ClassName("os-icon-user-male-circle"));
        public IWebElement IconPassword => _driver.FindElement(By.ClassName("os-icon-fingerprint"));
        public IWebElement TxtBoxRememberMe => _driver.FindElement(By.ClassName("form-check-input"));
        public IWebElement LoginLogo => _driver.FindElement(By.XPath("//div[@class='logo-w']//img"));
        public IWebElement TwitterIcon => _driver.FindElement(By.XPath("//a[@href='#']/img"));
        public IWebElement FbIcon => _driver.FindElement(By.XPath("//a[@href='#'][2]/img"));
        public IWebElement LinkedInIcon => _driver.FindElement(By.XPath("//a[@href='#'][3]/img"));
        public IWebElement UserIcon => _driver.FindElement(By.XPath("//div[@class='pre-icon os-icon os-icon-user-male-circle']"));
        public IWebElement PasswordIcon => _driver.FindElement(By.XPath("//div[@class='pre-icon os-icon os-icon-fingerprint']"));

        //For the alert element, it could have been technically possible to use id contains "random" but I assumed this was intended
        //to mean a true random id and so went with the 2nd role attribute which is common to both versions
        public IWebElement ErrorAlert => _driver.FindElement(By.XPath("//div[@role='alert'][2]"));

        #endregion Public Properties

        #region Public Methods

        //enters the username
        public void EnterUsername(string username)
        {
            TxtUserName.Clear();
            TxtUserName.SendKeys(username);
        }

        //enters the password
        public void EnterPassword(string password)
        {
            TxtPassword.Clear();
            TxtPassword.SendKeys(password);
        }

        //clicks submit button
        public void SubmitCredentials()
        {
            BtnLogIn.Click();
        }

        //simulates login
        public void Login(string username, string password)
        {
            EnterUsername(username);
            EnterPassword(password);
            SubmitCredentials();
        }

        //checks that alert is visually present with text
        public bool AlertIsPresent(string alertText)
        {
            //for v2, the alert isn't present because a z-index value is added to the element
            var zindex = ErrorAlert.GetCssValue("z-index");
            //verify the alert is displayed, it contains text, and the element is not hidden..
            if (ErrorAlert.Displayed && ErrorAlert.Text.Contains(alertText) && zindex != "-1")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion Public Methods
    }
}