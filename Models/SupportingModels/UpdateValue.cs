using System.ComponentModel.DataAnnotations;
using WGO_API.Models.Validation;
using WGO_API.Utils;

namespace WGO_API.Models.UpdateValue
{
    public class UpdatePassword
    {
        [Required]
        [MinLength(9)]
        public required string oldPassword { get; set; }
        [Required]
        [MinLength(9)]
        public required string newPassword { get; set; }
    }
}
