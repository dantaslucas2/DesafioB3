using System;
using System.Collections.Generic;
using System.Text;

namespace DesafioB3.Models
{
    public class FinancialModelResponse
    {
        public List<Stock> Stoke { get; set; }
    }

    public class Stock
    {
        public string symbol { get; set; }
        public decimal price { get; set; }
        public int volume { get; set; }
    }
}
