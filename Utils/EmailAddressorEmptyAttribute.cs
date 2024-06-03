using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace WGO_API.Utils
{
    public class EmailAddressorEmptyAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty((string?)value))
            {
                return null;
            }

            if (!(value is string valueAsString))
            {
                return new ValidationResult($"Field not of type string found {value.GetType()} instead.");
            }

            string regexPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            Regex regex = new Regex(regexPattern);
            return regex.IsMatch((string)value) ? null :
                new ValidationResult($"'{value.ToString()}' is not a valid email.");
        }
    }
}