using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using Hackathon.TraditionalTests.Helpers;
using OpenQA.Selenium;

namespace Hackathon.TraditionalTests.Pages
{
    //All of the page interactions happen in this page object class.  That way, if the elements change, we only have to change them here.
    //Assertions don't happen here, they happen in the tests
    //Contains state (locators) and behavior(methods)
    public class HomePage : BasePage
    {
        private readonly IWebDriver _driver;

        public HomePage(IWebDriver driver) : base(driver)
        {
            this._driver = driver;
        }

        #region Public Properties

        public IWebElement TableTransactions => _driver.FindElement(By.Id("transactionsTable"));
        public IWebElement TableHeadingAmount => _driver.FindElement(By.Id("amount"));
        public IWebElement ButtonCompareExpenses => _driver.FindElement(By.Id("showExpensesChart"));
        public IWebElement Canvas => _driver.FindElement(By.Id("canvas"));
        public IWebElement ButtonShowNewData => _driver.FindElement(By.Id("addDataset"));
        public IWebElement FlashSale => _driver.FindElement(By.XPath("//div[@id='flashSale']/img"));
        public IWebElement FlashSale2 => _driver.FindElement(By.XPath("//div[@id='flashSale2']/img"));

        #endregion Public Properties

        #region Public Methods

        //returns a list of table rows
        public IList<IWebElement> GetTableRows()
        {
            return _driver.FindElements(By.TagName("tr"));
        }

        //returns the amount column data, parsed so it can be used to compare
        public List<float> GetAmountColumn()
        {
            IList<IWebElement> data = _driver.FindElements(By.XPath("//td[@class='text-right bolder nowrap']"));
            var amountColumn = new List<float>();
            foreach (var amount in data)
            {
                //strip the text we don't need and put the values into a list of floats
                float a = float.Parse(Regex.Replace(amount.Text, @"[, USD]", string.Empty));
                amountColumn.Add(a);
            }

            return amountColumn;
        }

        //sort the table by amount
        public void SortTableByAmount()
        {
            TableHeadingAmount.Click();
        }

        //verify table is visible
        public bool TableOfTransactionsExists()
        {
            return TableTransactions.Displayed;
        }

        //load the expense chart
        //it has animation and loads slowly
        //adding an explicit wait is a bad practice and this should be refactored
        public bool ShowExpenseChart()
        {
            ButtonCompareExpenses.Click();
            Thread.Sleep(2000);
            return Canvas.Displayed;
        }

        //add 2019 data
        //also has animation
        //adding an explicit wait is a bad practice and this should be refactored
        public void ShowNextYearData()
        {
            ButtonShowNewData.Click();
            Thread.Sleep(2000);
        }

        //returns the raw bytes of the image using javascript's toDataURL method
        public string GetCanvasImage()
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
            return
                js.ExecuteScript(
                        "return document.getElementsByTagName('Canvas')[0].toDataURL('image/png').replace('data:image/png;base64,', '')")
                    .ToString();
        }

        //saves the image
        public void GetBarChartImage(string filename)
        {
            File.WriteAllBytes(ImageCompare.ScreenshotsPath + filename, Convert.FromBase64String(GetCanvasImage()));
        }

        #endregion Public Methods
    }
}