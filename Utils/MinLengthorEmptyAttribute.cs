using System.ComponentModel.DataAnnotations;

namespace WGO_API.Utils
{
    public class MinLengthorEmptyAttribute : ValidationAttribute
    {
        int minLength;
        public MinLengthorEmptyAttribute(int minlength) 
        {
            if (minlength < 0)
            {
                throw new InvalidOperationException("MinLengthorEmptyAttribute must have a Length value that is zero or greater");
            }
            minLength = minlength;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            int length;
            
            if (value == null)
            {
                return null;
            }

            if (value is string str)
            {
                length = str.Length;
            }
            else
            {
                return new ValidationResult($"Value not of type string found {value.GetType()} instead.");
            }

            return (length >= minLength || length == 0) ? null : 
                new ValidationResult($"Field must be atleast {minLength} long.");
        }
    }
}
