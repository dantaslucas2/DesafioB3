using DesafioB3.Models;
using DesafioB3.Models.Interfaces;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

namespace DesafioB3.APIConnector
{
    internal class FinancialModel : IApiConnector
    {
        private string _apiKey;
        private const string BaseUrl = "https://financialmodelingprep.com";
        private const string endpoint = "/api/v3/quote-short/";
        private readonly HttpClient _client;

        public FinancialModel(HttpClient client, IOptions<TokensSettings> options)
        {
            _apiKey = options.Value.FinancialModel;
            _client = client;
            _client.BaseAddress = new Uri(BaseUrl);
          } 

        public async Task<decimal> GetValue(string asset)
        {
            var url = BaseUrl + endpoint + asset + ".SA?apikey=" + _apiKey;
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url);

            httpRequestMessage.Method = HttpMethod.Get;
            httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage responseMessage = await _client.SendAsync(httpRequestMessage);
            var response = await responseMessage.Content.ReadAsStringAsync();
            response = "{Stoke:" + response + "}";
            FinancialModelResponse objectResponse = JsonConvert.DeserializeObject<FinancialModelResponse>(response);

            return objectResponse.Stoke.First().price;
        }
    }
}
