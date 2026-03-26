using DesafioB3.Models.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using Microsoft.Extensions.Configuration;
using DesafioB3.Application;
using DesafioB3.Domain.Models;
using DesafioB3.Smtp.APIConnector;
using DesafioB3.Infrastructure.Email;

namespace DesafioB3
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string asset;
            decimal targetToSell;
            decimal targetToBuy;

            if (args.Length != 3)
            {
                Console.WriteLine("Invalid arguments \nMust provide exactly 3 arguments in the following order. \n <Asset> <SellPrice> <BuyPrice>");
                return;
            }
            else if (args.Length ==  3)
            {
                try
                {
                    asset = args[0];
                    targetToSell = decimal.Parse(args[1], CultureInfo.InvariantCulture);
                    targetToBuy = decimal.Parse(args[2], CultureInfo.InvariantCulture);
                    Console.WriteLine($"Asset {asset} Target to sell {targetToSell} Target to buy {targetToBuy}");
                }
                catch
                {
                    throw new Exception("Invalid arguments");
                }

                var configuration = new ConfigurationBuilder()  
                    .SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();

                var builder = new ServiceCollection();

                builder.AddSingleton<IConfiguration>(configuration);

                builder.Configure<TokensSettings>(configuration.GetSection("ApiKeys"));
                builder.Configure<EmailSettings>(configuration.GetSection("Email"));

                builder.AddSingleton<EmailService>();

                builder.AddHttpClient<AlphaVantage>();
                builder.AddSingleton<IApiConnector, AlphaVantage>();

                builder.AddHttpClient<FinancialModel>();
                builder.AddSingleton<IApiConnector, FinancialModel>();

                builder.AddSingleton<IApiConnector, YahooFinance>();

                builder.AddSingleton<AssetMonitor>();


                var serviceProvider = builder.BuildServiceProvider();

                using (var cancellationTokenSource = new CancellationTokenSource())
                {
                    var assetMonitor = serviceProvider.GetRequiredService<AssetMonitor>();

                    Console.CancelKeyPress += (_, e) =>
                    {
                        e.Cancel = true;
                        cancellationTokenSource.Cancel();
                    };

                    await assetMonitor.MonitorConnector(asset, targetToBuy, targetToSell, cancellationTokenSource.Token);

                }
            }

        }
    }
}
