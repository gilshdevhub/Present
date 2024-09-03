using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FormsAPI.Dtos
{
    public class IsraeliIdAuthenticationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string idstring = Convert.ToString(value);
            if (idstring.Length != 9)
                return new ValidationResult("Length over 9!");
            int[] idNumber = idstring.Select(x => int.Parse(x.ToString())).ToArray();
            int total = 0;
            for (int i = 0; i < idNumber.Length; i++)
            {
                if (i % 2 != 0)
                    idNumber[i] *= 2;
                if (idNumber[i] >= 10)
                    idNumber[i] = (idNumber[i] % 10) + (idNumber[i] / 10);
                total += idNumber[i];
            }
            if (total % 10 == 0)
                return ValidationResult.Success;
            return new ValidationResult("Israeli ID Authentication failed!");
        }
    }
}
