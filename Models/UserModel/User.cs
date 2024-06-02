using Microsoft.AspNetCore.Identity;

namespace WGO_API.Models.UserModel
{
    public class User : IdentityUser
    {
        public Boolean UserBanned { get; set; }
    }
}
