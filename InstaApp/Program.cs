using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InstaWorker;

namespace InstaApp
{
    class Program
    {
        static void Main()
        {
            var chromeDriver = ChromeDriverFabric.GetChromeWebDriver();
            var authorizator = new Authorizator(chromeDriver);
            authorizator.Process("accounts.txt");
        }
    }
}