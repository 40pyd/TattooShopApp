namespace BLL.Models
{
    public class TattooDTO
    {
        public int TattoId { get; set; }
        public string Name { get; set; }
        public int? StyleId { get; set; }
        public string StyleName { get; set; }
        public decimal Price { get; set; }
        public int? AuthorId { get; set; }
        public string AuthorName { get; set; }
        public byte[] Image { get; set; }
    }
}
