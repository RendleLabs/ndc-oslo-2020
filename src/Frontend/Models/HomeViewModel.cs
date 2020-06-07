using System.Collections.Generic;

namespace Frontend.Models
{
    public class HomeViewModel
    {
        public HomeViewModel()
        {
            
        }
        public HomeViewModel(List<ToppingViewModel> toppings)
        {
            Toppings = toppings;
        }

        public List<ToppingViewModel> Toppings { get; set; }
    }
}