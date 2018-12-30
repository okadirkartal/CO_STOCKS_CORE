using System.Security.Claims;
using System.Security.Principal;

namespace Application.Web.Extensions
{
    public static class ExtendedUser
    {
        public static string GetToken(this IPrincipal user)
        {
            var claim = ((ClaimsIdentity)user.Identity).FindFirst("Token");
            return claim?.Value;
        }
        public static string GetUserId(this IPrincipal user)
        {
            var claim = ((ClaimsIdentity)user.Identity).FindFirst("UserId");
            return claim?.Value;
        }
        
        public static string GetUserName(this IPrincipal user)
        {
            var claim = ((ClaimsIdentity)user.Identity).FindFirst("UserName");
            return claim?.Value;
        }
        
        public static string GetPassword(this IPrincipal user)
        {
            var claim = ((ClaimsIdentity)user.Identity).FindFirst("Password");
            return claim?.Value;
        }
    }
}