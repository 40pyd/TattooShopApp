namespace BLL.Models
{
    public class AddressDTO
    {
        public int AddressId { get; set; }
        public string AddressLine { get; set; }
        public string ContactName { get; set; }
        public int? CountryId { get; set; }
        public string CountryName { get; set; }
        public string City { get; set; }
    }
}
