using DesafioB3.Domain.Models;
using DesafioB3.Models.Interfaces;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http.Headers;
using System.Text;

namespace DesafioB3.Smtp.APIConnector
{
    internal class AlphaVantage : IApiConnector
    {
        private const string BaseUrl = "https://www.alphavantage.co";
        private const string endpoint = "TIME_SERIES_DAILY";
        private const string endpoint2 = "GLOBAL_QUOTE";
        public string ApiName { get; }

        private readonly HttpClient _client;
        private readonly string _apiKey;
        public AlphaVantage(HttpClient client, IOptions<TokensSettings> options)
        {
            _apiKey = options.Value.AlphaVantage;
            _client = client;
            _client.BaseAddress = new Uri(BaseUrl);
            ApiName = "AlphaVantage";
        }
        public async Task<decimal> GetValue(string asset)
        {
            var url = $"/query?function={endpoint2}&symbol={asset}.sa&apikey={_apiKey}";
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url);

            httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage responseMessage = await _client.SendAsync(httpRequestMessage);
            responseMessage.EnsureSuccessStatusCode();
            var response = await responseMessage.Content.ReadAsStringAsync();
            var responseStream = await responseMessage.Content.ReadAsStreamAsync();

            Root objectResponse = JsonConvert.DeserializeObject<Root>(response);

            var value = decimal.Parse(objectResponse.GlobalQuote.Price, CultureInfo.InvariantCulture);
            return value;
        }
    }
}
