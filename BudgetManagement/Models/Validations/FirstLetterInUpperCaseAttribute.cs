using System.ComponentModel.DataAnnotations;

namespace BudgetManagement.Models.Validations
{
    public class FirstLetterInUpperCaseAttribute:ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            //si es nulo el valor
            if (value==null || string.IsNullOrEmpty(value.ToString())) 
            {
                return ValidationResult.Success;
            }
            var firstLetter = value.ToString()[0].ToString();
            //si no es mayuscula la primera letra
            if (firstLetter != firstLetter.ToUpper()) 
            {
                return new ValidationResult("The first letter must be uppercase");
            }

            return ValidationResult.Success;
        }
    }
}
