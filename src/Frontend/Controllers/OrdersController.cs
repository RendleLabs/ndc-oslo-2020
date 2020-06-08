using System.Linq;
using System.Threading.Tasks;
using Frontend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Frontend.Controllers
{
    [Route("orders")]
    public class OrdersController : Controller
    {
        private readonly Orders.OrdersClient _ordersClient;
        private readonly ILogger<OrdersController> _log;

        public OrdersController(Orders.OrdersClient ordersClient, ILogger<OrdersController> log)
        {
            _ordersClient = ordersClient;
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

            var response = await _ordersClient.PlaceOrderAsync(request);
            
            return View();
        }
    }
}