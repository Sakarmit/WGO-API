using System.ComponentModel.DataAnnotations;

namespace WGO_API.Utils
{
    public class MinMaxLengthorEmptyAttribute : ValidationAttribute
    {
        int minLength;
        int maxLength;
        public MinMaxLengthorEmptyAttribute(int minlength, int maxlength) 
        {
            if (minlength < 0)
            {
                throw new InvalidOperationException("MinMaxLengthorEmpty must have a min length value that is zero or greater.");
            }
            else if (maxlength < 1)
            {
                throw new InvalidOperationException("MinMaxLengthorEmpty must have a max length value that is one or greater.");
            }
            else if (minLength > maxLength)
            {
                throw new InvalidOperationException("MinMaxLengthEmpty cannot have min length be greater than max length.");
            }
            else
            {
                minLength = minlength;
                maxLength = maxlength;
            } 
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

            bool rangeCheck = length >= minLength && length <= maxLength;
            return (rangeCheck || length == 0) ? null : 
                new ValidationResult($"Field must be between {minLength}-{maxLength} long.");
        }
    }
}
