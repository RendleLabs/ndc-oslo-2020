using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Certs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Frontend
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
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
                .ConfigureChannel(((provider, channel) =>
                {
                    channel.HttpClient = provider.GetRequiredService<IHttpClientFactory>().CreateClient("grpc");
                    channel.DisposeHttpClient = true;
                }));


            services.AddGrpcClient<Orders.OrdersClient>((provider, options) =>
            {
                var config = provider.GetService<IConfiguration>();
                var uri = config.GetServiceUri("Orders");
                options.Address = uri;
            });
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}