using DesafioB3.Models.Interfaces;
using DesafioB3.Smtp;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace DesafioB3
{
    internal class AssetMonitor
    {
        private readonly List<IApiConnector> _apiConnectors;
        private readonly int retryTime = 500;
        private readonly EmailService _emailService;
        public AssetMonitor(IEnumerable<IApiConnector> apiConnectors, EmailService emailService)
        {
            _apiConnectors = apiConnectors.ToList();
            _emailService = emailService;
        }

        public async Task MonitorConnector(string asset, decimal targetBuy, decimal targetSell, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                decimal? value = null;
                foreach (IApiConnector apiConnector in _apiConnectors.ToList())
                {
                    try
                    {
                        value = await apiConnector.GetValue(asset);
                        break;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Error Value {e}");
                        _apiConnectors.Remove(apiConnector);
                        _apiConnectors.Add(apiConnector);
                    }
                }
                if (value is not null)
                {
                    if (value > targetSell)
                    {
                        _emailService.SendEmail(asset, false, (decimal) value);
                    }
                    else if (value < targetBuy)
                    {
                        _emailService.SendEmail(asset, true, (decimal) value);
                    }
                    Console.WriteLine($"Asset value: {value}");
                    break;
                }
                else
                    Console.WriteLine($"All APIs are unavailable");
                await Task.Delay(retryTime);
            }
        }
    }
}
