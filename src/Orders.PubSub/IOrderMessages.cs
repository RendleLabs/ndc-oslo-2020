using System.Threading;
using System.Threading.Tasks;

namespace Orders.PubSub
{
    public interface IOrderMessages
    {
        ValueTask<OrderMessage> ReadAsync(CancellationToken token);
    }
}