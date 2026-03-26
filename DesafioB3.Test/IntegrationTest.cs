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

        public IntegrationTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void EmailToSellIntegrationTests()
        {
            _output.WriteLine("Teste de integração para envio de email de venda");
        }
         
        [Fact]
        public void EmailToBuyIntegrationTests()
        {
            _output.WriteLine("Teste de integração para envio de email de compra");
            Assert.True(false);
        }
    }
}
