using InstaWorker;
using InstaWorker.Driver;

namespace InstaApp
{
    class Program
    {
        static void Main()
        {
            var chromeDriverFabric = new ChromeDriverFabric(@"C:\Users\Олег\source\repos\InstaApp\InstaWorker\ChromeDriver\");
            using (var chromeDriver = chromeDriverFabric.GetWebDriver())
            {
                var authorizator = new Liker(chromeDriver);
                authorizator.Process("accounts.txt");
            }
        }
    }
}