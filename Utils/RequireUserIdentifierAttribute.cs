using System;
using System.ComponentModel.DataAnnotations;
using WGO_API.Models.UserModel;

namespace WGO_API.Models.Validation
{
    public class RequireUserIdentifierAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var userDTO = (UserDTO) validationContext.ObjectInstance;

            return (string.IsNullOrEmpty(userDTO.UserName) 
                && string.IsNullOrEmpty(userDTO.Email))
                ? new ValidationResult("Either UserName or Email must be provided.")
                : null;
        }
    }
}
