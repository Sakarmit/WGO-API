using System.ComponentModel.DataAnnotations;
using WGO_API.Models.Validation;
using WGO_API.Utils;

namespace WGO_API.Models.UserModel
{
    [RequireUserIdentifier]
    public class UserDTO
    {
        [MinLengthorEmpty(4)]
        public string? UserName { get; set; }
        [EmailAddressorEmpty]
        public string? Email { get; set; }
        [Required]
        [MinLength(9)]
        public required string Password { get; set; }
    }
}
