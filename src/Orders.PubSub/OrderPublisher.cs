using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace Orders.PubSub
{
    public interface IOrderPublisher : IDisposable
    {
        Task PublishOrder(IEnumerable<string> toppingIds, DateTimeOffset time);
    }

    public class OrderPublisher : IOrderPublisher
    {
        private const string ConnectionString = "ndcworkshop.redis.cache.windows.net:6380,password=bsB7CZhUC0g4KeAz15hQtTvIuH4PpY8tfePNs0PohBM=,ssl=True,abortConnect=False";
        private readonly IConnectionMultiplexer _redis;
        private readonly ISubscriber _sub;
        private readonly ILogger<OrderPublisher> _log;

        public OrderPublisher(ILogger<OrderPublisher> log)
        {
            _log = log;
            _redis = ConnectionMultiplexer.Connect(ConnectionString);
            _sub = _redis.GetSubscriber();
        }

        public async Task PublishOrder(IEnumerable<string> toppingIds, DateTimeOffset time)
        {
            var message = new OrderMessage
            {
                ToppingIds = toppingIds.ToArray(),
                Time = time
            }.ToBytes();
            await _sub.PublishAsync("orders", message);
        }

        public void Dispose()
        {
            _redis.Dispose();
        }
    }
}
