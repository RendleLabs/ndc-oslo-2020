using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using TestHelpers;
using Toppings.Data;

namespace Toppings.Tests
{
    public class ToppingsApplicationFactory : WebApplicationFactory<Startup>
    {
        public Toppings.ToppingsClient CreateToppingsClient()
        {
            var channel = this.CreateGrpcChannel();
            return new Toppings.ToppingsClient(channel);
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.Remove<IToppingData>();

                var list = new List<ToppingEntity>
                {
                    new ToppingEntity("cheese", "Cheese", 0.5m, 50),
                    new ToppingEntity("sauce", "Tomato Sauce", 0.5m, 50),
                };

                var sub = Substitute.For<IToppingData>();
                sub.GetAsync(Arg.Any<CancellationToken>())
                    .Returns(Task.FromResult(list));

                services.AddSingleton(sub);
            });
        }
    }
}