using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Application.API.Jwt
{
    public static class JwtSecurityKey
    {
        public static SymmetricSecurityKey Create(string secret)
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));
        } 
    }
}