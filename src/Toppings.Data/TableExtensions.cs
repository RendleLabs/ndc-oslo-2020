using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using Microsoft.Azure.Cosmos.Table;

namespace Toppings.Data
{
    internal static class TableExtensions
    {
        public static async IAsyncEnumerable<T> ExecuteAsync<T>(this TableQuery<T> query, [EnumeratorCancellation] CancellationToken token = default)
        {
            TableQuerySegment<T> segment;
            do
            {
                segment = await query.ExecuteSegmentedAsync(null, token);
                foreach (var result in segment.Results)
                {
                    yield return result;
                }
            } while (segment.ContinuationToken != null && !token.IsCancellationRequested);
        }
    }
}