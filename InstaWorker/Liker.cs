using InstaWorker.Driver;
using InstaWorker.DTO;
using InstaWorker.Strings;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.IO;
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
                        SetLikeToLastPublication(profileName);
                    }
                    _accounts.Add(accountDTO);
                }
            }
        }

        private void SetLikeToLastPublication(string profileName)
        {
            _webDriver.Navigate().GoToUrl($"{Consts.InstaMainUri}/{profileName}");
            foreach (var webElement in _htmlWorker.GetAllWebElements(By.ClassName(Consts.InstaPublicationBlockCSSClass)))
            {
                _htmlWorker.Click(webElement, By.TagName(Consts.LinkHtmlTagName));
                var likeDiv = _htmlWorker.TryFindElement(By.CssSelector(Consts.LikeDivCSSSelector), 5, 1000);
                likeDiv.Click();
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