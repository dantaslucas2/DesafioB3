using DesafioB3.Models;
using DesafioB3.Models.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using YahooFinanceApi;
namespace DesafioB3.Smtp.APIConnector
{
    internal class YahooFinance : IApiConnector
    {
        public string ApiName { get; }

        public YahooFinance()
        {
            ApiName = "Yahoo Finance";
        }
        public async Task<decimal> GetValue(string asset)
        {
            string symbol = asset + ".SA";

            IReadOnlyDictionary<string, Security> securities = await Yahoo.Symbols(symbol).Fields(Field.RegularMarketPrice).QueryAsync();

            foreach (var security in securities)
            {
                Console.WriteLine($"Símbolo: {security.Key}");
                Console.WriteLine($"Preço de Mercado: {security.Value.RegularMarketPrice}");
            }
            return (decimal)securities.First().Value.RegularMarketPrice;
        }
    }
}
