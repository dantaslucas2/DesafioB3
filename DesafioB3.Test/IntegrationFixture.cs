using DesafioB3.Domain.Models;
using DesafioB3.Infrastructure.Email;
using DesafioB3.Test.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DesafioB3.Models.Interfaces;
using DesafioB3.Smtp.APIConnector;
using DesafioB3.Application;
using System;
using System.Collections.Generic;
using System.Text;

namespace DesafioB3.Test
{
    public class IntegrationFixture
    {
        public IServiceProvider _service {  get; set; }

        public IntegrationFixture()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.Test.json", optional: false)
                .Build();

            var services = new ServiceCollection();
            services.AddSingleton<IConfiguration>(configuration);
            services.Configure<TokensSettings>(configuration.GetSection("ApiKeys"));
            services.Configure<EmailSettings>(configuration.GetSection("Email"));
            services.Configure<ImapSettings>(configuration.GetSection("Imap"));

            services.AddSingleton<EmailService>();
            services.AddSingleton<EmailReader>();

            services.AddSingleton<IApiConnector, YahooFinance>();
            services.AddSingleton<AssetMonitor>();

            services.AddHttpClient<AlphaVantage>();
            services.AddSingleton<IApiConnector, AlphaVantage>();

            services.AddHttpClient<FinancialModel>();
            services.AddSingleton<IApiConnector, FinancialModel>();



            _service = services.BuildServiceProvider();
        }

        public async Task<decimal> GetCurrentPriceAsync(string asset)
        {
            var connectors = _service.GetServices<IApiConnector>();

            foreach (var connector in connectors)
            {
                try
                {
                    return await connector.GetValue(asset);
                }
                catch
                {
                    Console.WriteLine("Error");
                }
            }
            throw new NotImplementedException();
        }
    }
}
