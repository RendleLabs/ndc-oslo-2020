using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Orders.PubSub;

namespace Orders
{
    public class OrdersService : Orders.OrdersBase
    {
        private readonly Toppings.ToppingsClient _toppingsClient;
        private readonly IOrderMessages _orderMessages;
        private readonly IOrderPublisher _orderPublisher;
        private readonly ILogger<OrdersService> _logger;
        
        public OrdersService(Toppings.ToppingsClient toppingsClient,
            IOrderMessages orderMessages,
            IOrderPublisher orderPublisher,
            ILogger<OrdersService> logger)
        {
            _toppingsClient = toppingsClient;
            _orderMessages = orderMessages;
            _orderPublisher = orderPublisher;
            _logger = logger;
        }

        public override async Task<PlaceOrderResponse> PlaceOrder(PlaceOrderRequest request, ServerCallContext context)
        {
            var time = DateTimeOffset.UtcNow;

            await _orderPublisher.PublishOrder(request.ToppingIds, time);
            
            var decrementStockRequest = new DecrementStockRequest();
            decrementStockRequest.ToppingIds.AddRange(request.ToppingIds);

            await _toppingsClient.DecrementStockAsync(decrementStockRequest);
            
            return new PlaceOrderResponse
            {
                Time = time.ToTimestamp()
            };
        }

        public override async Task Subscribe(SubscribeRequest request,
            IServerStreamWriter<SubscribeResponse> responseStream, ServerCallContext context)
        {
            var cancellationToken = context.CancellationToken;
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var orderMessage = await _orderMessages.ReadAsync(cancellationToken);
                    var response = new SubscribeResponse
                    {
                        Time = orderMessage.Time.ToTimestamp()
                    };
                    response.ToppingIds.AddRange(orderMessage.ToppingIds);
                    await responseStream.WriteAsync(response);
                }
                catch (OperationCanceledException)
                {
                    _logger.LogInformation("Subscriber disconnected.");
                    break;
                }
            }
        }
    }
}
