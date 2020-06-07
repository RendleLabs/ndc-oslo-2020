namespace Frontend.Models
{
    public class ToppingViewModel
    {
        public ToppingViewModel()
        {
            
        }
        public ToppingViewModel(string id, string name, decimal price)
        {
            Id = id;
            Name = name;
            Price = price;
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public bool Selected { get; set; }
    }
}