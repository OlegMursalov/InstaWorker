using OpenQA.Selenium;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace InstaWorker.Driver
{
    public class HtmlWorker
    {
        private IWebDriver _webDriver;

        public HtmlWorker(IWebDriver webDriver)
        {
            _webDriver = webDriver;
        }

        public IWebElement TryFindElement(By by, int times, int millisecondsPerTime)
        {
            var task = Task.Run(() =>
            {
                for (int i = 0; i < times; i++)
                {
                    try
                    {
                        var elem = _webDriver.FindElement(by);
                        if (elem != null)
                        {
                            return elem;
                        }
                    }
                    catch (NoSuchElementException)
                    {
                        Thread.Sleep(millisecondsPerTime);
                        continue;
                    }
                }
                return null;
            });
            return task.Result;
        }

        public void SetTextToWebElement(IWebElement webElement, string text)
        {
            webElement.Clear();
            webElement.SendKeys(text);
        }

        public ReadOnlyCollection<IWebElement> GetAllWebElements(By by)
        {
            return _webDriver.FindElements(by);
        }

        public void Click(IWebElement webElement, By by)
        {
            var innerElem = webElement.FindElement(by);
            if (innerElem != null)
            {
                innerElem.Click();
            }
        }

        public void SetTextToWebElement(By by, string text)
        {
            var elem = _webDriver.FindElement(by);
            SetTextToWebElement(elem, text);
        }

        public void Click(By by)
        {
            var elem = _webDriver.FindElement(by);
            elem.Click();
        }
    }
}