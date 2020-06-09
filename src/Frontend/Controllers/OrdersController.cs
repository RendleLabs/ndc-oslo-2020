using System.Linq;
using System.Threading.Tasks;
using Frontend.Models;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Frontend.Controllers
{
    [Route("orders")]
    public class OrdersController : Controller
    {
        private readonly Orders.OrdersClient _ordersClient;
        private readonly IAuthHelper _authHelper;
        private readonly ILogger<OrdersController> _log;

        public OrdersController(Orders.OrdersClient ordersClient, IAuthHelper authHelper, ILogger<OrdersController> log)
        {
            _ordersClient = ordersClient;
            _authHelper = authHelper;
            _log = log;
        }

        [HttpPost]
        public async Task<IActionResult> Order([FromForm]HomeViewModel viewModel)
        {
            var toppingIds = viewModel.Toppings
                .Where(t => t.Selected)
                .Select(t => t.Id);
            
            var request = new PlaceOrderRequest();
            request.ToppingIds.AddRange(toppingIds);

            // var token = await _authHelper.GetTokenAsync();
            // var metadata = new Metadata {{"Authorization", $"Bearer {token}"}};
            // var response = await _ordersClient.PlaceOrderAsync(request, metadata);
            
            var response = await _ordersClient.PlaceOrderAsync(request);
            
            return View();
        }
    }
}