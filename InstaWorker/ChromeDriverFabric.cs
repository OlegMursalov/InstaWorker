using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace InstaWorker
{
    public static class ChromeDriverFabric
    {
        private readonly static string _path = "ChromeDriver\\chromedriver.exe";

        private static ChromeDriver _chromeDriverSingleton = null;

        public static IWebDriver GetChromeWebDriver()
        {
            if (_chromeDriverSingleton == null)
            {
                _chromeDriverSingleton = new ChromeDriver(_path);
            }

            return _chromeDriverSingleton;
        }
    }
}