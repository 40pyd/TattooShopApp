using System.ComponentModel.DataAnnotations;

namespace DAL.Models
{
    public class Tattoo
    {
        [Key]
        public int TattoId { get; set; }
        public string Name { get; set; }
        public int? StyleId { get; set; }
        public decimal Price { get; set; }
        public int? AuthorId { get; set; }
        public byte[] Image { get; set; }

        public virtual Style Style { get; set; }
        public virtual Author Author { get; set; }
    }
}
