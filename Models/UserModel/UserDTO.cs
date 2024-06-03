using System.ComponentModel.DataAnnotations;
using WGO_API.Utils;

namespace WGO_API.Models.UserModel
{
    [RequireUserIdentifier]
    public class UserDTO
    {
        [MinMaxLengthorEmpty(4, 16)]
        public string? UserName { get; set; }
        [EmailAddressorEmpty]
        public string? Email { get; set; }
        [Required]
        [MinLength(9)]
        public required string Password { get; set; }
    }
}
