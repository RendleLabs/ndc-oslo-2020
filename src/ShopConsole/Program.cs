using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ShopConsole
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    if (!(args.Length == 1 && Uri.TryCreate(args[0], UriKind.Absolute, out var address)))
                    {
                        address = new Uri("https://localhost:5005");
                    }

                    services.AddGrpcClient<Orders.Orders.OrdersClient>(options =>
                    {
                        options.Address = address;
                    });
                    services.AddHostedService<Worker>();
                });
    }
}
