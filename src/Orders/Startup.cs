using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Certs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orders.PubSub;

namespace Orders
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var handler = DevelopmentModeCertificateHelper.CreateClientHandler();
            services.AddHttpClient("grpc").ConfigurePrimaryHttpMessageHandler(() => handler);
            
            services.AddGrpcClient<Toppings.ToppingsClient>((provider, options) =>
            {
                var config = provider.GetService<IConfiguration>();
                var uri = config.GetServiceUri("Toppings");
                options.Address = uri;
            })
                .ConfigureChannel((provider, channel) =>
                {
                    channel.HttpClient = provider.GetRequiredService<IHttpClientFactory>().CreateClient("grpc");
                    channel.DisposeHttpClient = true;
                });
            services.AddOrderPubSub();
            services.AddGrpc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<OrdersService>();

                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                });
            });
        }
    }
}
