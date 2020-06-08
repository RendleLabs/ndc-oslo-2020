using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace Orders
{
    public class OrdersService : Orders.OrdersBase
    {
        private readonly ILogger<OrdersService> _logger;
        public OrdersService(ILogger<OrdersService> logger)
        {
            _logger = logger;
        }

        public override async Task<PlaceOrderResponse> PlaceOrder(PlaceOrderRequest request, ServerCallContext context)
        {
            var time = DateTimeOffset.UtcNow;
            return new PlaceOrderResponse
            {
                Time = time.ToTimestamp()
            };
        }
    }
}
