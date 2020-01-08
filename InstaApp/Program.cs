using InstaWorker;
using InstaWorker.Driver;

namespace InstaApp
{
    class Program
    {
        static void Main()
        {
            var chromeDriverFabric = new ChromeDriverFabric("ChromeDriver\\chromedriver.exe");
            var chromeDriver = chromeDriverFabric.GetWebDriver();
            var authorizator = new Liker(chromeDriver);
            authorizator.Process("accounts.txt");
        }
    }
}