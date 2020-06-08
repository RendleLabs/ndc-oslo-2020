using System;
using System.Threading.Tasks;
using Xunit;

namespace Toppings.Tests
{
    public class ToppingsClientTests : IClassFixture<ToppingsApplicationFactory>
    {
        private readonly ToppingsApplicationFactory _factory;

        public ToppingsClientTests(ToppingsApplicationFactory factory)
        {
            _factory = factory;
        }
        
        [Fact]
        public async Task GetsAvailableToppings()
        {
            var client = _factory.CreateToppingsClient();
            var response = await client.GetAvailableAsync(new AvailableRequest());
            Assert.Equal(2, response.Toppings.Count);
        }
    }
}
