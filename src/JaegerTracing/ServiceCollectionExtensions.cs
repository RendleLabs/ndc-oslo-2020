using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenTracing.Util;

namespace JaegerTracing
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddJaegerTracing(this IServiceCollection services)
        {
            var jaeger = Environment.GetEnvironmentVariable("JAEGER_SERVICE_NAME");
            
            if (!string.IsNullOrEmpty(jaeger))
            {
                services.AddOpenTracing();
                services.AddSingleton(provider =>
                {
                    var loggerFactory = provider.GetRequiredService<ILoggerFactory>();

                    var config = Jaeger.Configuration.FromEnv(loggerFactory);
                    var tracer = config.GetTracer();

                    if (!GlobalTracer.IsRegistered())
                    {
                        GlobalTracer.Register(tracer);
                    }

                    return tracer;
                });
            }

            return services;
        }
    }
}
