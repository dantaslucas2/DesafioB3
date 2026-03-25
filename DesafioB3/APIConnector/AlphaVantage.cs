using DesafioB3.Models;
using DesafioB3.Models.Interfaces;
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
        private string apiKei;
        private const string BaseUrl = "https://www.alphavantage.co";
        private const string endpoint = "TIME_SERIES_DAILY";
        private const string endpoint2 = "GLOBAL_QUOTE";
        public async Task ConfigureToken()
        {
            apiKei = Environment.GetEnvironmentVariable("ALPHAVANTAGE_API_KEY");
        }

        public async Task<decimal> GetValue(string asset)
        {
            HttpClient _client = new HttpClient();
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage();

            httpRequestMessage.RequestUri = new Uri(BaseUrl + "/query?function=" + endpoint2 + "&symbol=" + asset + ".sa&apikey=" + apiKei);
            httpRequestMessage.Method = HttpMethod.Get;
            httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage responseMessage = await _client.SendAsync(httpRequestMessage);
            var response = await responseMessage.Content.ReadAsStringAsync();
            var responseStream = await responseMessage.Content.ReadAsStreamAsync();

            Root objectResponse = JsonConvert.DeserializeObject<Root>(response);
            //var objectResponse1 = Utf8Json.JsonSerializer.Deserialize<Root>(response);

            var value = decimal.Parse(objectResponse.GlobalQuote.Price, CultureInfo.InvariantCulture);
            return value;
        }
    }
}
