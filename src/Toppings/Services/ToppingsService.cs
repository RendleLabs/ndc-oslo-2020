using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Toppings.Data;

namespace Toppings
{
    public class ToppingsService : Toppings.ToppingsBase
    {
        private readonly IToppingData _data;
        private readonly ILogger<ToppingsService> _logger;
        
        public ToppingsService(IToppingData data, ILogger<ToppingsService> logger)
        {
            _data = data;
            _logger = logger;
        }

        public override async Task<AvailableResponse> GetAvailable(AvailableRequest request, ServerCallContext context)
        {
            var toppings = await _data.GetAsync(context.CancellationToken);
            var availableToppings = toppings.Select(t => new AvailableTopping
            {
                Topping = new Topping
                {
                    Id = t.Id,
                    Name = t.Name,
                    Price = (float) t.Price
                },
                Quantity = t.StockCount
            });
            var response = new AvailableResponse();
            response.Toppings.AddRange(availableToppings);
            return response;
        }
    }
}
