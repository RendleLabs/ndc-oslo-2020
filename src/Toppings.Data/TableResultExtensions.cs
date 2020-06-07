using Microsoft.Azure.Cosmos.Table;

namespace Toppings.Data
{
    internal static class TableResultExtensions
    {
        public static bool IsSuccessStatusCode(this TableResult result) =>
            result.HttpStatusCode >= 200 && result.HttpStatusCode < 400;
    }
}