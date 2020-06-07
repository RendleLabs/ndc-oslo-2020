using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Orders.PubSub
{
    public interface IOrderMessages
    {
        ValueTask<OrderMessage> ReadAsync(CancellationToken token);
    }

    public class OrderMessages : IOrderMessages
    {
        private static readonly UnboundedChannelOptions ChannelOptions = new UnboundedChannelOptions
        {
            SingleWriter = true,
            SingleReader = false,
            AllowSynchronousContinuations = false
        };

        private readonly Channel<OrderMessage> _channel;

        public OrderMessages()
        {
            _channel = Channel.CreateUnbounded<OrderMessage>(ChannelOptions);
        }

        internal ValueTask WriteAsync(OrderMessage message, CancellationToken token) =>
            _channel.Writer.WriteAsync(message, token);

        public ValueTask<OrderMessage> ReadAsync(CancellationToken token) =>
            _channel.Reader.ReadAsync(token);
    }
}