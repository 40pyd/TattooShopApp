using _2ScullTattooShop.Models.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace _2ScullTattooShop.Models.BasketModels
{
    public class Basket
    {
        private List<BasketItem> itemCollection = new List<BasketItem>();

        public virtual void AddItem(ProductViewModel product,int quantity)
        {
            BasketItem item = itemCollection
                .Where(p => p.Product.Id == product.Id).FirstOrDefault();

            if(item==null)
            {
                itemCollection.Add(new BasketItem
                {
                    Product = product,
                    Quantity = quantity
                });
            }
            else
            {
                item.Quantity += quantity;
            }
        }

        public virtual void RemoveItem(ProductViewModel product)
        {
            itemCollection.RemoveAll(i => i.Product.Id == product.Id);
        }

        public virtual decimal ComputeTotalValue()=>itemCollection.Sum(e => e.Product.Price * e.Quantity);
        
        public virtual void Clear() => itemCollection.Clear();

        public virtual IEnumerable<BasketItem> Items => itemCollection;
    }
}
