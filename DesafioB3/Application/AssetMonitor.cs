using DesafioB3.Infrastructure.Email;
using DesafioB3.Models.Enums;
using DesafioB3.Models.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace DesafioB3.Application
{
    internal class AssetMonitor
    {
        private readonly List<IApiConnector> _apiConnectors;
        private readonly int retryTime = 5000;
        private readonly EmailService _emailService;

        private MonitorState _state = MonitorState.NotTriggered;
        public AssetMonitor(IEnumerable<IApiConnector> apiConnectors, EmailService emailService)
        {
            _apiConnectors = apiConnectors.ToList();
            _emailService = emailService;
        }

        public async Task MonitorConnector(string asset, decimal targetToBuy, decimal targetToSell, CancellationToken cancellationToken)
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
                    if (value > targetToSell)
                    {
                        if (_state != MonitorState.SellTriggered)
                        {
                            await _emailService.SendEmail(asset, false, (decimal)value);
                            _state = MonitorState.SellTriggered;
                        }
                    }
                    else if (value < targetToBuy)
                    {
                        if (_state != MonitorState.BuyTriggered)
                        {
                            await _emailService.SendEmail(asset, true, (decimal)value);
                            _state = MonitorState.BuyTriggered;
                        }
                    }
                    else
                        _state = MonitorState.NotTriggered;
                    Console.WriteLine($"Asset value: {value}");
                }
                else
                    Console.WriteLine($"All APIs are unavailable");
                await Task.Delay(retryTime, cancellationToken);
            }
        }
    }
}
