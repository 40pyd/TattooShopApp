using _2ScullTattooShop.Models.ViewModels;

namespace _2ScullTattooShop.Models.BasketModels
{
    public class BasketItem
    {
        public int CartItemId { get; set; }
        public ProductViewModel Product { get; set; }
        public int Quantity { get; set; }
    }
}
