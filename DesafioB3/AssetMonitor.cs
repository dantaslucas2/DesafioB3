using DesafioB3.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DesafioB3
{
    internal class AssetMonitor
    {
        private readonly List<IApiConnector> apiConnectors;
        private readonly int retryTime = 500;
        public AssetMonitor(List<IApiConnector> apiConnectors)
        {
            this.apiConnectors = apiConnectors ?? throw new NotImplementedException(nameof(apiConnectors));
        }

        public async Task MonitorConnector(CancellationTokenSource cancellationTokenSource, string asset, decimal targetBuy, decimal targetSell)
        {
            while (!cancellationTokenSource.IsCancellationRequested)
            {
                foreach (IApiConnector apiConnector in apiConnectors.ToList())
                {
                    try
                    {
                        decimal value = await apiConnector.GetValue(asset);
                        if (value > targetSell)
                        {
                            Smtp.EmailService.SendEmail(asset, false, value);
                        }
                        else if (value < targetBuy)
                        {
                            Smtp.EmailService.SendEmail(asset, true, value);
                        }
#if DEBUG
                        Console.WriteLine($"Asset value: {value}");
#endif
                        break;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Error Value {e}");
                        apiConnectors.Remove(apiConnector);
                        apiConnectors.Add(apiConnector);
                    }
                    await Task.Delay(retryTime);
                }
            }
        }
    }
}
