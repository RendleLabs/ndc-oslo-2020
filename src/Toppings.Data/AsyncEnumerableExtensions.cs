using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Toppings.Data
{
    internal static class AsyncEnumerableExtensions
    {
        public static async Task<List<T>> ToListAsync<T>(this IAsyncEnumerable<T> source, CancellationToken token)
        {
            var list = new List<T>();

            await foreach (var item in source.WithCancellation(token))
            {
                list.Add(item);
            }

            return list;
        }
    }
}