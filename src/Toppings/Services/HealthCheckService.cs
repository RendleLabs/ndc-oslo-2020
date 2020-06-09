using System;
using System.Threading.Tasks;
using Grpc.Core;
using Toppings.Data;

// ReSharper disable once CheckNamespace
namespace Toppings
{
    public class HealthCheckService : Health.HealthBase
    {
        private readonly IToppingData _data;

        public HealthCheckService(IToppingData data)
        {
            _data = data;
        }

        public override async Task<HealthCheckResponse> Check(HealthCheckRequest request, ServerCallContext context)
        {
            if (await _data.PingAsync(context.CancellationToken))
            {
                return new HealthCheckResponse
                {
                    Status = HealthCheckResponse.Types.ServingStatus.Serving
                };
            }

            return new HealthCheckResponse
            {
                Status = HealthCheckResponse.Types.ServingStatus.NotServing
            };
        }

        public override async Task Watch(HealthCheckRequest request, IServerStreamWriter<HealthCheckResponse> responseStream, ServerCallContext context)
        {
            while (!context.CancellationToken.IsCancellationRequested)
            {
                if (await _data.PingAsync(context.CancellationToken))
                {
                    await responseStream.WriteAsync(new HealthCheckResponse
                    {
                        Status = HealthCheckResponse.Types.ServingStatus.Serving
                    });
                }
                else
                {
                    await responseStream.WriteAsync(new HealthCheckResponse
                    {
                        Status = HealthCheckResponse.Types.ServingStatus.NotServing
                    });
                }

                await Task.Delay(TimeSpan.FromSeconds(5));
            }
        }
    }
}