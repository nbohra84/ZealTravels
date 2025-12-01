using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Common.DataAnnotations
{
    public class BooleanRequiredAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is bool && !(bool)value)
            {
                return new ValidationResult(ErrorMessage ?? "This checkbox is required");
            }

            return ValidationResult.Success;
        }
    }
}
