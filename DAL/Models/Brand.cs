using System.Collections.Generic;

namespace DAL.Models
{
    public class Brand
    {
        public int BrandId { get; set; }
        public string Name { get; set; }
        public byte[] Image { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
