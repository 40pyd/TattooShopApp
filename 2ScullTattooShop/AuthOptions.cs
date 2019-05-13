using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace _2ScullTattooShop
{
    public class AuthOptions
    {
        public const string ISSUER = "2ScullsAuthServer";
        public const string AUDIENCE = "https://localhost:44372/";
        const string KEY = "123456789mysupersecretkey";
        public const int LIFETIME = 1;
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
        }
    }
}
