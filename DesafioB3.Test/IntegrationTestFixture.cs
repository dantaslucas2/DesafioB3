namespace DesafioB3.Test
{
    public class IntegrationTestFixture
    {
        public Smtp4DevClient Smtp4DevClient { get; }
        public IConfiguration Configuration { get; }
        public IServiceProvider ServiceProvider { get; }

        public IntegrationTestFixture()
        {
            var smtp4DevSettings = new Smtp4DevSettings();
            var httpClient = new HttpClient { BaseAddress = new Uri(smtp4DevSettings.BaseUrl) };
            Smtp4DevClient = new Smtp4DevClient(httpClient);
        }

        public async Task GetCurrentPrice(string asset, CancellationToken cancellationToken = default)
        {
            var connectors = ServiceProvider.GetServices<IApiConnector>().ToList();

            foreach (var connector in connectors)
            {
                return await connector.GetCurrentPriceAsync(asset, cancellationToken);
            }
        }
    }
}