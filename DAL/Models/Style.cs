using System.Collections.Generic;

namespace DAL.Models
{
    public class Style
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public byte[] Image { get; set; }

        public virtual ICollection<Tattoo> Tattoos { get; set; }
    }
}
