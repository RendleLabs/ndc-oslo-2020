using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace Orders.PubSub
{
    public class OrderSubscriber : BackgroundService
    {
        private const string ConnectionString = "ndcworkshop.redis.cache.windows.net:6380,password=bsB7CZhUC0g4KeAz15hQtTvIuH4PpY8tfePNs0PohBM=,ssl=True,abortConnect=False";
        private readonly IConnectionMultiplexer _redis;
        private readonly ISubscriber _sub;
        private readonly OrderMessages _orderMessages;
        private readonly ILogger<OrderSubscriber> _log;

        public OrderSubscriber(OrderMessages orderMessages, ILogger<OrderSubscriber> log)
        {
            _orderMessages = orderMessages;
            _log = log;
            _redis = ConnectionMultiplexer.Connect(ConnectionString);
            _sub = _redis.GetSubscriber();
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var queue = await _sub.SubscribeAsync("orders");
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var queueMessage = await queue.ReadAsync(stoppingToken);
                    var orderMessage = OrderMessage.FromBytes(queueMessage.Message);
                    await _orderMessages.WriteAsync(orderMessage, stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    _log.LogInformation("Service stopping.");
                    break;
                }
            }
        }

        public override void Dispose()
        {
            _redis.Dispose();
            base.Dispose();
        }
    }
}