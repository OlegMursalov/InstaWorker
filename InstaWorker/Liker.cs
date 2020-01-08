using InstaWorker.Driver;
using InstaWorker.DTO;
using InstaWorker.Strings;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace InstaWorker
{
    public class Liker
    {
        private IWebDriver _webDriver;
        private HtmlWorker _htmlWorker;
        private List<AccountDTO> _accounts;

        public Liker(IWebDriver webDriver)
        {
            _webDriver = webDriver;
            _htmlWorker = new HtmlWorker(webDriver);
            _accounts = new List<AccountDTO>();
        }
        
        public void Process(string fileName, string profileName)
        {
            if (!File.Exists(fileName))
            {
                throw new ArgumentException($"Файл {fileName} не найден");
            }

            var lines = File.ReadAllLines(fileName);
            foreach (var line in lines)
            {
                var arr = line.Split(';');
                if (arr.Length == 2)
                {
                    var accountDTO = new AccountDTO
                    {
                        EmailOrPhone = arr[0],
                        Password = arr[1]
                    };
                    var isAuthorized = Authorize(accountDTO);
                    if (isAuthorized)
                    {
                        accountDTO.IsAuthorized = true;
                        SetLikeToPublications(profileName, amountLast: 5);
                    }
                    _accounts.Add(accountDTO);
                }
            }
        }

        private void SetLikeToPublications(string profileName, int amountLast)
        {
            var rand = new Random();
            _webDriver.Navigate().GoToUrl($"{Consts.InstaMainUri}/{profileName}");
            var elemCollection = _htmlWorker.GetAllWebElements(By.ClassName(Consts.InstaPublicationBlockCSSClass)).Take(amountLast);
            foreach (var webElement in elemCollection)
            {
                Thread.Sleep(rand.Next(1, 10) * 100);
                _htmlWorker.Click(webElement, By.TagName(Consts.LinkHtmlTagName));
                var likeSpan = _htmlWorker.TryFindElement(By.CssSelector(Consts.LikeSpanCSSSelector), 5, 1000);
                likeSpan.Click();
                _htmlWorker.Click(By.CssSelector(Consts.ClosePublicationBtnCSSSelector));
            }
        }

        private bool Authorize(AccountDTO accountDTO)
        {
            var authorizeFormUriPart = Consts.InstaAuthorizeFormUriPart;
            _webDriver.Navigate().GoToUrl($"{Consts.InstaMainUri}/{authorizeFormUriPart}");
            var userNameTextBox = _htmlWorker.TryFindElement(By.Name(Consts.InstaAuthorizeFormLoginElemName), 5, 1000);
            _htmlWorker.SetTextToWebElement(userNameTextBox, accountDTO.EmailOrPhone);
            _htmlWorker.SetTextToWebElement(By.Name(Consts.InstaAuthorizeFormPasswordElemName), accountDTO.Password);
            _htmlWorker.Click(By.CssSelector(Consts.InstaAuthorizeFormSubmitCSSSelector));
            Thread.Sleep(5000);
            var url = _webDriver.Url;
            return !string.IsNullOrEmpty(url) && !url.Contains(authorizeFormUriPart);
        }
    }
}