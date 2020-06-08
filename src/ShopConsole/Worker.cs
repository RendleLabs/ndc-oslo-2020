using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orders;

namespace ShopConsole
{
    public class Worker : BackgroundService
    {
        private readonly Orders.Orders.OrdersClient _ordersClient;
        private readonly ILogger<Worker> _logger;

        public Worker(Orders.Orders.OrdersClient ordersClient, ILogger<Worker> logger)
        {
            _ordersClient = ordersClient;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var subscribeRequest = new SubscribeRequest();
                    var sub = _ordersClient.Subscribe(subscribeRequest);

                    await foreach (var response in sub.ResponseStream.ReadAllAsync(stoppingToken))
                    {
                        var toppings = string.Join(", ", response.ToppingIds);
                        Console.WriteLine($"{response.Time.ToDateTimeOffset():T}: {toppings}");
                    }
                }
                catch (OperationCanceledException)
                {
                    _logger.LogInformation("Worker stopping.");
                    return;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                }
            }
        }
    }
}
