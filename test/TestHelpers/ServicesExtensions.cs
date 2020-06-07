using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace TestHelpers
{
    public static class ServicesExtensions
    {
        public static IServiceCollection Remove<T>(this IServiceCollection services)
        {
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(T));
            
            if (!(descriptor is null))
            {
                services.Remove(descriptor);
            }

            return services;
        }
    }
}