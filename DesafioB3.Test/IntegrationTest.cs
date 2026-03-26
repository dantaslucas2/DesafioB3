using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace DesafioB3.Test
{
    public class IntegrationTest
    {
        private readonly ITestOutputHelper _output;
        private readonly IntegrationTestFixture _fixture;

        public IntegrationTest(ITestOutputHelper output, IntegrationTestFixture fixture)
        {
            _output = output;
            _fixture = fixture;
        }

        [Fact]
        public async Task EmailToSellIntegrationTests()
        {
            string asset = "PETR4";

            await _fixture.GetCurrentPrice(asset);

            _output.WriteLine("Teste de integração para envio de email de venda");
        }
         
        [Fact]
        public async Task EmailToBuyIntegrationTests()
        {
            _output.WriteLine("Teste de integração para envio de email de compra");
            Assert.True(false);
        }
    }
}
