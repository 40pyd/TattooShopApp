using _2ScullTattooShop.Models.BasketModels;
using Microsoft.AspNetCore.Mvc;

namespace _2ScullTattooShop.Components
{
    public class BasketTotalViewComponent:ViewComponent
    {
        private Basket basket;

        public BasketTotalViewComponent(Basket _basket)
        {
            basket = _basket;
        }

        public IViewComponentResult Invoke() => View(basket);
    }
}
