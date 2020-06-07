using Microsoft.Extensions.DependencyInjection;

namespace Orders.PubSub
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddOrderPubSub(this IServiceCollection services)
        {
            var orderMessages = new OrderMessages();
            services.AddSingleton(orderMessages);
            services.AddSingleton<IOrderMessages>(orderMessages);
            services.AddSingleton<IOrderPublisher, OrderPublisher>();
            services.AddHostedService<OrderSubscriber>();
            return services;
        }
    }
}