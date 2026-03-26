using DesafioB3.Models;
using DesafioB3.Models.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

namespace DesafioB3.APIConnector
{
    internal class FinancialModel : IApiConnector
    {
        private string apiKei;
        private const string BaseUrl = "https://financialmodelingprep.com";
        private const string endpoint = "/api/v3/quote-short/";
        public async Task ConfigureToken()
        {
            apiKei = Environment.GetEnvironmentVariable("FINANCIAL_MODEL_API_KEY");
        }

        public async Task<decimal> GetValue(string asset)
        {
            HttpClient _client = new HttpClient();
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage();

            httpRequestMessage.RequestUri = new Uri(BaseUrl + endpoint + asset + ".SA?apikey=" + apiKei);
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
