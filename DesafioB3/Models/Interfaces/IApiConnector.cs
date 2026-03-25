using System;
using System.Collections.Generic;
using System.Text;

namespace DesafioB3.Models.Interfaces
{
    public interface IApiConnector
    {
        Task ConfigureToken();
        Task<decimal> GetValue(string asset);
    }
}
