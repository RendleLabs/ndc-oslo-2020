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
        private readonly Toppings.ToppingsClient _toppingsClient;
        private readonly ILogger<OrdersService> _logger;
        public OrdersService(Toppings.ToppingsClient toppingsClient, ILogger<OrdersService> logger)
        {
            _toppingsClient = toppingsClient;
            _logger = logger;
        }

        public override async Task<PlaceOrderResponse> PlaceOrder(PlaceOrderRequest request, ServerCallContext context)
        {
            var time = DateTimeOffset.UtcNow;
            
            var decrementStockRequest = new DecrementStockRequest();
            decrementStockRequest.ToppingIds.AddRange(request.ToppingIds);

            await _toppingsClient.DecrementStockAsync(decrementStockRequest);
            
            return new PlaceOrderResponse
            {
                Time = time.ToTimestamp()
            };
        }
    }
}
