using System;
using System.Configuration;
using Applitools;
using Applitools.Selenium;
using Hackathon.TraditionalTests.Pages;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Hackathon.VisualAITests
{
    [TestFixture]
    [Category("VisualAI")]
    public class VisualAITests
    {
        private EyesRunner _runner;
        private Eyes _eyes;
        private IWebDriver _driver;
        private LoginPage _loginPage;
        private HomePage _homePage;
        private const string AppName = "Hackathon";
        private static readonly string _url = ConfigurationManager.AppSettings["V2"];
        private static readonly string _showAds = ConfigurationManager.AppSettings["showAds"];

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            //Initialize the Runner
            _runner = new ClassicRunner();
            // Initialize the eyes SDK
            _eyes = new Eyes
            {
                ApiKey = Environment.GetEnvironmentVariable("APPLITOOLS_API_KEY", EnvironmentVariableTarget.User),
                ForceFullPageScreenshot = true
            };

            //All tests will go into a single batch..
            _eyes.Batch = new BatchInfo(AppName);
        }

        [SetUp]
        public void SetUp()
        {
            _driver = new ChromeDriver();
            _driver.Manage().Window.Maximize();
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

            _loginPage = new LoginPage(_driver);
            _homePage = new HomePage(_driver);
        }

        [Test]
        [Category("VisualAI")]
        [TestCase(TestName = "Verify login page UI elements"), Order(1)]
        public void LoginPageUIElementTest()
        {
            //Start test by setting App name and test name
            _eyes.Open(_driver, AppName, TestContext.CurrentContext.Test.Name);
            //Open login page
            _loginPage.Open(_url);
            //Take screenshot
            _eyes.CheckWindow("Login Page");
            //End test
            _eyes.CloseAsync();
        }

        [Test]
        [Category("VisualAI")]
        [TestCase(TestName = "Verify login page functionality")]
        public void DataDrivenTest()
        {
            //Start test by setting App name and test name
            _eyes.Open(_driver, AppName, TestContext.CurrentContext.Test.Name);
            //Open login page
            _loginPage.Open(_url);

            //Pass in empty credentials and submit
            _loginPage.Login("", "");
            //Take screenshot
            _eyes.CheckWindow("Verify no username or password throws error");

            //Pass in username but no password and submit
            _loginPage.Login("user", "");
            //Take screenshot
            _eyes.CheckWindow("Verify adding a username but no password throws error");

            //Pass in username but no password and submit
            _loginPage.Login("", "password");
            //Take screenshot
            _eyes.CheckWindow("Verify adding a password but no username throws error");

            //Pass in username but no password and submit
            _loginPage.Login("user", "password");
            //Take screenshot
            _eyes.CheckWindow("Verify adding a username and password takes user to the home page");

            //End test
            _eyes.CloseAsync();
        }

        [Test]
        [Category("VisualAI")]
        [TestCase(TestName = "Verify transaction table can be sorted by amount")]
        public void TableSortTest()
        {
            //Start test by setting App name and test name
            _eyes.Open(_driver, AppName, TestContext.CurrentContext.Test.Name);
            //Open login page
            _loginPage.Open(_url);
            //Pass in credentials and submit
            _loginPage.Login("user", "password");
            //Set match level to content
            _eyes.MatchLevel = MatchLevel.Content;
            //Take a screenshot of the table before it's sorted
            _eyes.CheckWindow("Unsorted Table");
            //Sort the table by amount
            _homePage.SortTableByAmount();
            //Take a screenshot of the table sorted
            _eyes.CheckWindow("Table Sorted By Amount");
            //End test
            _eyes.CloseAsync();
        }

        [Test]
        [Category("VisualAI")]
        [TestCase(TestName = "Verify canvas bar chart is visually correct")]
        public void CanvasChartTest()
        {
            //Start test by setting App name and test name
            _eyes.Open(_driver, AppName, TestContext.CurrentContext.Test.Name);
            //Open login page
            _loginPage.Open(_url);
            //Pass in credentials and submit
            _loginPage.Login("user", "password");
            //Show the expense chart
            _homePage.ShowExpenseChart();
            //Take a screenshot of the chart
            _eyes.CheckWindow("2017 and 2018 Expense Chart");
            //Show 2019 data
            _homePage.ShowNextYearData();
            //Take a screenshot of 2019 data
            _eyes.CheckWindow("2019 Data Added to Expense Chart");
            //End test
            _eyes.CloseAsync();
        }

        [Test]
        [Category("VisualAI")]
        [TestCase(TestName = "Verify dynamic ads display")]
        public void DynamicContentTest()
        {
            //Start test by setting App name and test name
            _eyes.Open(_driver, AppName, TestContext.CurrentContext.Test.Name);
            //Open login page
            _loginPage.Open(_url + _showAds);
            //Pass in credentials and submit
            _loginPage.Login("user", "password");
            //Set match level to layout
            _eyes.MatchLevel = MatchLevel.Layout;
            //Take screenshot
            _eyes.CheckWindow("Display Ads");
            //End test
            _eyes.CloseAsync();
        }

        [TearDown]
        public void AfterEach()
        {
            //Close the browser
            _driver.Quit();
            //If the test was aborted before eyes.close was called, ends the test as aborted.
            _eyes.AbortIfNotClosed();
            //Wait and collect all test results
            TestResultsSummary allTestResults = _runner.GetAllTestResults();
        }
    }
}