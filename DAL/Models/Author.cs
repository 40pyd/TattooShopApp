using System.Collections.Generic;

namespace DAL.Models
{
    public class Author
    {
        public int AuthorId { get; set; }
        public string Name { get; set; }
        public string Sername { get; set; }
        public string NickName { get; set; }
        public int Age { get; set; }
        public byte[] Photo { get; set; }

        public virtual ICollection<Tattoo> Tattoos { get; set; }
    }
}
