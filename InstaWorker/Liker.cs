using InstaWorker.DTO;
using InstaWorker.Strings;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.IO;

namespace InstaWorker
{
    public class Liker
    {
        private IWebDriver _webDriver;
        private List<AccountDTO> _accounts;

        public Liker(IWebDriver webDriver)
        {
            _webDriver = webDriver;
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

        private void SetTextToWebElement()
        {

        }

        private bool Authorize(AccountDTO accountDTO)
        {
            var authorizeFormUri = Consts.InstaAuthorizeFormUri;
            _webDriver.Navigate().GoToUrl(authorizeFormUri);
            var userNameTextBox = _webDriver.FindElement(By.Name("username"));
            userNameTextBox.SendKeys();
            return false;
        }
    }
}