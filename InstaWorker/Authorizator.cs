using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace InstaWorker
{
    public class Authorizator
    {
        private IWebDriver _webDriver;
        private List<AccountDTO> _accounts;

        public Authorizator(IWebDriver webDriver)
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
                    _accounts.Add(accountDTO);
                }
            }
        }
    }
}