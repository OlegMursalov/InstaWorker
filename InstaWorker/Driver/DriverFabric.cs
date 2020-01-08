using OpenQA.Selenium;

namespace InstaWorker.Driver
{
    public abstract class DriverFabric
    {
        public abstract IWebDriver GetWebDriver();
    }
}