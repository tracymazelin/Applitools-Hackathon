using System;
using BoDi;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using TechTalk.SpecFlow;

namespace Hackathon.TraditionalTests.Hooks
{
    [Binding]
    public class WebDriverHooks
    {
        private readonly IObjectContainer _container;
        public IWebDriver driver;

        public WebDriverHooks(IObjectContainer container)
        {
            this._container = container;
        }

        [BeforeScenario]
        public void CreateWebDriver()
        {
            //Before each test, insantiate the browser and set an implicit wait
            var options = new ChromeOptions();
            options.AddArgument("start-maximized");

            ChromeDriver driver = new ChromeDriver(options);

            // Implicit wait for Selenium.  If an element isn't found, it will keep trying to find it for 5 seconds before failing
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            _container.RegisterInstanceAs<IWebDriver>(driver);
        }

        [AfterScenario]
        public void DestroyWebDriver()
        {
            //Close the browser after every test
            var driver = _container.Resolve<IWebDriver>();

            if (driver != null)
            {
                driver.Quit();
                driver.Dispose();
            }
        }
    }
}