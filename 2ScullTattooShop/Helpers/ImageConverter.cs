using Microsoft.AspNetCore.Http;
using System.IO;

namespace _2ScullTattooShop.Helpers
{
    public class ImageConverter
    {
        public static byte[] GetBytes(IFormFile file)
        {
            using (var binaryReader = new BinaryReader(file.OpenReadStream()))
            {
                return binaryReader.ReadBytes((int)file.Length);
            }
        }
    }
}
