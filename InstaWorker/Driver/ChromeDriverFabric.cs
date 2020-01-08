using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace InstaWorker.Driver
{
    public class ChromeDriverFabric : DriverFabric
    {
        private readonly string _pathToDriver;
        private ChromeDriver _chromeDriverSingleton = null;

        public ChromeDriverFabric(string pathToDriver)
        {
            _pathToDriver = pathToDriver;
        }

        public override IWebDriver GetWebDriver()
        {
            if (_chromeDriverSingleton == null)
            {
                _chromeDriverSingleton = new ChromeDriver(_pathToDriver);
            }
            return _chromeDriverSingleton;
        }
    }
}