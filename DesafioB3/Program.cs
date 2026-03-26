using DesafioB3.APIConnector;
using DesafioB3.Models.Interfaces;
using DesafioB3.Smtp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using Microsoft.Extensions.Configuration;
using DesafioB3.Models;

namespace DesafioB3
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string asset;
            decimal targetToSell;
            decimal targetToBuy;

            if (args.Length ==  3)
            {
                try
                {
                    asset = args[0];
                    targetToSell = decimal.Parse(args[1], CultureInfo.InvariantCulture);
                    targetToBuy = decimal.Parse(args[2], CultureInfo.InvariantCulture);
                }
                catch
                {
                    throw new Exception("Invalid arguments");
                }
                var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

                var emailSettings = configuration
                    .GetSection("ApiKeys")
                    .Get<TokensSettings>();

                var serviceProvider = new ServiceCollection()
                        .AddScoped<IApiConnector, AlphaVantage>()
                        .AddScoped<IApiConnector, FinancialModel>()
                        .AddScoped<IApiConnector, YahooFinance>()
                        .AddScoped<List<IApiConnector>>(serviceProvider =>
                        {
                            return serviceProvider.GetServices<IApiConnector>().ToList();
                        })
                        .AddScoped<AssetMonitor>()
                        .BuildServiceProvider();

                using (var cancellationTokenSource = new CancellationTokenSource())
                {
                    var assetMonitor = serviceProvider.GetRequiredService<AssetMonitor>();

                    var task = Task.Factory.StartNew(() => assetMonitor.MonitorConnector(cancellationTokenSource, asset, targetToBuy, targetToSell), TaskCreationOptions.LongRunning);

                    Console.WriteLine("Press enter to end the program");
                    Console.ReadLine();

                    cancellationTokenSource.Cancel();
                    await task;
                }
            }

        }
    }
}
