using System.Collections.Generic;
using Microsoft.Azure.Cosmos.Table;

namespace Toppings.Data
{
    public class ToppingEntity : TableEntity
    {
        public ToppingEntity()
        {
            PartitionKey = "toppings";
        }
        
        public ToppingEntity(string id, string name, decimal price, int stockCount) : this()
        {
            Id = id;
            Name = name;
            Price = price;
            StockCount = stockCount;
        }

        [IgnoreProperty]
        public string Id
        {
            get => RowKey;
            set => RowKey = value;
        }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int StockCount { get; set; }
    }
}
