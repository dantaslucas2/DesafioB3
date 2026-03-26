using DesafioB3.Application;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace DesafioB3.Test
{
    public class IntegrationTest : IClassFixture<IntegrationFixture>
    {
        private readonly ITestOutputHelper _output;
        private readonly IntegrationFixture _fixture;

        public IntegrationTest(ITestOutputHelper output, IntegrationFixture fixture)
        {
            _output = output;
            _fixture = fixture;
        }

        [Fact]
        public async Task EmailToSellIntegrationTests()
        {
            string asset = "PETR4";

            var currentPrice = await _fixture.GetCurrentPriceAsync(asset);

            var targetToSell = currentPrice - 0.01m;
            var targetToBuy = 0.01m;

            var monitor = _fixture._service.GetRequiredService<AssetMonitor>();
            var emailReader = _fixture._service.GetRequiredService<EmailReader>();

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(12));
            using var emailCts = new CancellationTokenSource(TimeSpan.FromSeconds(25));

            var monitorTask = monitor.MonitorConnector(asset, targetToBuy, targetToSell, cts.Token);

            var email = await emailReader.WaitForEmailAsync(
                subjectContains: asset,
                timeout: TimeSpan.FromSeconds(20),
                cancellationToken: emailCts.Token);

            cts.Cancel();

            Assert.NotNull(email);
            Assert.Contains(asset, email.Value.Subject, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("sell", email.Value.Body, StringComparison.OrdinalIgnoreCase);
            _output.WriteLine("Teste de integração para envio de email de venda");
        }

        [Fact]
        public async Task EmailToBuyIntegrationTests()
        {
            _output.WriteLine("Teste de integração para envio de email de compra");
        }
    }
}
