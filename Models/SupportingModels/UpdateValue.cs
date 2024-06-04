using System.ComponentModel.DataAnnotations;

namespace WGO_API.Models.SupportingModels
{
    public class UpdatePassword
    {
        [Required]
        [MinLength(9)]
        public required string OldPassword { get; set; }
        [Required]
        [MinLength(9)]
        public required string NewPassword { get; set; }
    }
}
