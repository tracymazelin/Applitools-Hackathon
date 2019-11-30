using System.Collections.Generic;
using System.Net;
using OpenQA.Selenium;

namespace Hackathon.TraditionalTests.Pages
{
    //Common methods that can be used throughout the app.  All page objects inherit from this class.
    public class BasePage
    {
        private readonly IWebDriver _driver;

        public BasePage(IWebDriver driver)
        {
            this._driver = driver;
        }

        #region Public Properties

        public IList<IWebElement> LinkElements => _driver.FindElements(By.TagName("a"));

        #endregion Public Properties

        #region Public Methods

        public void Open(string url)
        {
            _driver.Navigate().GoToUrl(url);
        }

        //this method sends a HEAD request with the full image url
        //if a response is returned, the server is telling us the image exists
        //otherwise, the image does not exist and won't be displayed
        public bool VerifyImageIsVisible(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "HEAD";

            bool imageExists;
            try
            {
                request.GetResponse();
                imageExists = true;
            }
            catch
            {
                imageExists = false;
            }

            return imageExists;
        }

        //looks for an exact text string on the page
        public bool VerifyTextExists(string textToVerify)
        {
            bool textExists;
            try
            {
                _driver.PageSource.Contains(textToVerify);
                textExists = true;
            }
            catch
            {
                textExists = false;
            }

            return textExists;
        }

        //gets the the full image url for use with VerifyImageIsVisible
        public string GetElementSrc(IWebElement element)
        {
            return element.GetAttribute("src");
        }

        #endregion Public Methods
    }
}