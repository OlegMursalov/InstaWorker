using InstaWorker.Driver;
using InstaWorker.DTO;
using InstaWorker.Strings;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

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
        
        public void Process(string fileName)
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
                    _accounts.Add(accountDTO);
                }
            }
        }

        private bool Authorize(AccountDTO accountDTO)
        {
            var authorizeFormUri = Consts.InstaAuthorizeFormUri;
            _webDriver.Navigate().GoToUrl(authorizeFormUri);
            var userNameTextBox = _htmlWorker.TryFindElement(By.Name(Consts.InstaAuthorizeFormLoginElemName), 5, 1000);
            _htmlWorker.SetTextToWebElement(userNameTextBox, accountDTO.EmailOrPhone);
            _htmlWorker.SetTextToWebElement(By.Name(Consts.InstaAuthorizeFormPasswordElemName), accountDTO.Password);
            _htmlWorker.Click(By.CssSelector(Consts.InstaAuthorizeFormSubmitCSSSelector));
            return false;
        }
    }
}