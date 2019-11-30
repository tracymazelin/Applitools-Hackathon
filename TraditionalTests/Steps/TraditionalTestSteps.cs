using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Hackathon.TraditionalTests.Helpers;
using Hackathon.TraditionalTests.Pages;
using NUnit.Framework;
using OpenQA.Selenium;
using TechTalk.SpecFlow;

namespace Hackathon.TraditionalTests.Steps
{
    [Binding]
    public class TraditionalTestSteps
    {
        private readonly IWebDriver _driver;
        private readonly LoginPage _loginPage;
        private readonly HomePage _homePage;
        private IList<IWebElement> _unsortedData;

        public static string Url = ConfigurationManager.AppSettings["V2"];
        public static string WithAds = ConfigurationManager.AppSettings["showAds"];

        public TraditionalTestSteps(IWebDriver driver)
        {
            _driver = driver;
            _loginPage = new LoginPage(driver);
            _homePage = new HomePage(driver);
        }

        [Given(@"I open the login page")]
        public void NavigateToHomePage()
        {
            _loginPage.Open(Url);
            Assert.IsTrue(_driver.Title.Contains("ACME demo app"));
        }

        [StepDefinition(@"I verify the expected fields exist in the page")]
        public void FieldsExist()
        {
            Assert.IsTrue(_loginPage.TxtUserName.Displayed, "The username field is not visible");
            Assert.AreEqual("Enter your username", _loginPage.TxtUserName.GetAttribute("placeholder"), "The username field does not have the correct placeholder text");
            Assert.IsTrue(_loginPage.TxtPassword.Displayed, "The password field is not visible");
            Assert.AreEqual("Enter your password", _loginPage.TxtPassword.GetAttribute("placeholder"), "The password field does not have the correct placeholder text");
        }

        [StepDefinition(@"I verify the expected field labels exist in the page")]
        public void FieldsLabelsExist()
        {
            Assert.IsTrue(_loginPage.VerifyTextExists("Username"), "The username field label is not visible");
            Assert.IsTrue(_loginPage.VerifyTextExists("Password"), "The password field label is not visible");
        }

        [StepDefinition(@"I verify the expected images exist in the page")]
        public void ImagesExistInThePage()
        {
            //gets the src attribute from each image and then sends a head request to validate that it's visible
            Assert.IsTrue(_loginPage.VerifyImageIsVisible(_loginPage.GetElementSrc(_loginPage.LoginLogo)), "The login logo is not visible");
            Assert.IsTrue(_loginPage.VerifyImageIsVisible(_loginPage.GetElementSrc(_loginPage.TwitterIcon)), "The twitter icon is not visible");
            Assert.IsTrue(_loginPage.VerifyImageIsVisible(_loginPage.GetElementSrc(_loginPage.FbIcon)), "The facebook icon is not visible");
            Assert.IsTrue(_loginPage.VerifyImageIsVisible(_loginPage.GetElementSrc(_loginPage.LinkedInIcon)), "The linkedIn icon is not visible");
            Assert.IsTrue(_loginPage.IconUsername.Displayed, "The username icon is not visible");
            Assert.IsTrue(_loginPage.IconPassword.Displayed, "The password icon is not visible");
        }

        [StepDefinition(@"I verify the expected link elements exist in the page")]
        public void LinksExist()
        {
            Assert.AreEqual(4, _loginPage.LinkElements.Count, "There are missing link elements");
        }

        [StepDefinition(@"I verify the login button exists in the page")]
        public void LoginButtonExists()
        {
            Assert.IsTrue(_loginPage.BtnLogIn.Displayed, "The login button is not visible");
            Assert.IsTrue(_loginPage.BtnLogIn.Enabled, "The login button is not enabled");
        }

        [StepDefinition(@"I verify the Remember Me checkbox exists in the page")]
        public void RememberMeExists()
        {
            Assert.IsTrue(_loginPage.VerifyTextExists("Remember Me"), "The remember me field label is not visible");
            Assert.IsTrue(_loginPage.TxtBoxRememberMe.Displayed, "The remember me checkbox is not enabled");
            Assert.IsFalse(_loginPage.TxtBoxRememberMe.Selected, "The remember me checkbox is checked by default");
        }

        [StepDefinition(@"I enter username '(.*)' and password '(.*)'")]
        public void EnterUsernamePassword(string username, string password)
        {
            _loginPage.EnterUsername(username);
            Assert.AreEqual(username, _loginPage.TxtUserName.GetAttribute("value"));

            _loginPage.EnterPassword(password);
            Assert.AreEqual(password, _loginPage.TxtPassword.GetAttribute("value"));

            _loginPage.SubmitCredentials();
        }

        [StepDefinition(@"I verify the page contains text '(.*)'")]
        public void VerifyLoginStatus(string text)
        {
            if (text == "Recent Transactions")
            {
                Assert.IsTrue(_homePage.VerifyTextExists(text));
            }
            else
            {
                Assert.IsTrue(_loginPage.AlertIsPresent(text), "Alert text is not visible");
            }
        }

        [StepDefinition(@"I see a table of transactions")]
        public void TableOfTransactionsVisible()
        {
            Assert.IsTrue(_homePage.TableOfTransactionsExists(), "The transactions table isn't visible");
            _unsortedData = _homePage.GetTableRows();
        }

        [StepDefinition(@"I click the amount column to sort it")]
        public void SortTableByAmount()
        {
            _homePage.SortTableByAmount();
        }

        [StepDefinition(@"I verify that the table rows remained in tact")]
        public void VerifyTableData()
        {
            IList<IWebElement> sortedData = _homePage.GetTableRows();
            //compare unsorted values with sorted values
            bool dataIsTheSame = !_unsortedData.Except(sortedData).Any() && !sortedData.Except(_unsortedData).Any();
            Assert.IsTrue(dataIsTheSame, "The table data is not in tact after a sort");
        }

        [StepDefinition(@"I verify the amount column is in ascending order")]
        public void VerifyColumnIsSorted()
        {
            //create two lists.  1 with unsorted data, the other with sorted.  Compare them.
            List<float> amountColumn = _homePage.GetAmountColumn();
            var amountSorted = amountColumn.OrderBy(a => a).ToList();
            CollectionAssert.AreEqual(amountSorted, amountColumn, "The column is not sorted in ascending order");
        }

        [StepDefinition(@"I click compare expenses and see a bar chart")]
        public void CompareExpenses()
        {
            Assert.IsTrue(_homePage.ShowExpenseChart(), "The expense chart is not visible");
        }

        [StepDefinition(@"I validate the bar chart has the correct data")]
        public void ValidateBarChart()
        {
            //create a unique filename using a guid
            string filename = Guid.NewGuid().ToString() + ".png";
            _homePage.GetBarChartImage(filename);
            //compare the baseline image with the new one and check for match
            //the algorithm wasn't sensitive enough to point out the two bars that had differing heights
            //this is much better suited to a visual AI tool like Applitools
            Assert.IsTrue(ImageCompare.CompareImages(ImageCompare.ScreenshotsPath + "2017-2018-ExpensesBaseLine.png",
                ImageCompare.ScreenshotsPath + filename), "The chart does not match the baseline image");
        }

        [StepDefinition(@"I validate next years data is displayed")]
        public void ValidateNextYearData()
        {
            _homePage.ShowNextYearData();
            string filename = Guid.NewGuid().ToString() + ".png";
            _homePage.GetBarChartImage(filename);
            Assert.IsTrue(ImageCompare.CompareImages(ImageCompare.ScreenshotsPath + "2017-2019-ExpensesBaseLine.png",
                ImageCompare.ScreenshotsPath + filename), "The chart does not match the baseline image");
        }

        [StepDefinition(@"I open the login page with Ads enabled")]
        public void NavigateToHomePageWithAds()
        {
            _loginPage.Open(Url + WithAds);
            Assert.IsTrue(_driver.Title.Contains("ACME demo app"));
        }

        [StepDefinition(@"I verify both flashSale ads are displayed on the page")]
        public void AdIsDisplayedOnPage()
        {
            //gets the element src attribute and sends a HEAD request to verify it's presence on the page
            Assert.IsTrue(_homePage.VerifyImageIsVisible(_homePage.GetElementSrc(_homePage.FlashSale)), "The first Flash Sale Ad is not visible");
            Assert.IsTrue(_homePage.VerifyImageIsVisible(_homePage.GetElementSrc(_homePage.FlashSale2)), "The second Flash Sale Ad is not visible");
        }
    }
}