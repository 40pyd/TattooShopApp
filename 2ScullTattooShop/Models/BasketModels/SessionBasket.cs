using _2ScullTattooShop.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using _2ScullTattooShop.Extensions;

namespace _2ScullTattooShop.Models.BasketModels
{
    public class SessionBasket:Basket
    {
        public static Basket GetBasket(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?
                .HttpContext.Session;
            SessionBasket basket = session?.GetJson<SessionBasket>("Basket") ??
                new SessionBasket();
            basket.Session = session;
            return basket;
        }

        [JsonIgnore]
        public ISession Session { get; set; }

        public override void AddItem(ProductViewModel product, int quantity)
        {
            base.AddItem(product, quantity);
            Session.SetJson("Basket", this);
        }

        public override void RemoveItem(ProductViewModel product)
        {
            base.RemoveItem(product);
            Session.SetJson("Basket", this);
        }

        public override void Clear()
        {
            base.Clear();
            Session.Remove("Basket");
        }
    }
}
