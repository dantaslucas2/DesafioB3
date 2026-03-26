using DesafioB3.Models;
using DesafioB3.Models.Interfaces;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http.Headers;
using System.Text;

namespace DesafioB3.APIConnector
{
    internal class AlphaVantage : IApiConnector
    {
        private const string BaseUrl = "https://www.alphavantage.co";
        private const string endpoint = "TIME_SERIES_DAILY";
        private const string endpoint2 = "GLOBAL_QUOTE";

        private readonly HttpClient _client;
        private readonly string _apiKey;
        public AlphaVantage(HttpClient client, IOptions<TokensSettings> options)
        {
            _apiKey = options.Value.AlphaVantage;
            _client = client;
            _client.BaseAddress = new Uri(BaseUrl);
        }
        public async Task<decimal> GetValue(string asset)
        {
            var url = $"/query?function={endpoint2}&symbol={asset}.sa&apikey={_apiKey}";
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url);

            httpRequestMessage.Method = HttpMethod.Get;
            httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage responseMessage = await _client.SendAsync(httpRequestMessage);
            var response = await responseMessage.Content.ReadAsStringAsync();
            var responseStream = await responseMessage.Content.ReadAsStreamAsync();

            Root objectResponse = JsonConvert.DeserializeObject<Root>(response);

            var value = decimal.Parse(objectResponse.GlobalQuote.Price, CultureInfo.InvariantCulture);
            return value;
        }
    }
}
