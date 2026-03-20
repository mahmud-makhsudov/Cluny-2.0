using System.ComponentModel.DataAnnotations;
using PhoneNumbers;

namespace ClunyApp.Authorization
{
    public class E164PhoneAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var str = value as string;
            if (string.IsNullOrWhiteSpace(str))
            {
                return ValidationResult.Success;
            }

            try
            {
                var util = PhoneNumberUtil.GetInstance();
                var number = util.Parse(str, null);

                if (!util.IsValidNumber(number))
                {
                    return new ValidationResult(ErrorMessage ?? "Phone number is not valid.");
                }

                return ValidationResult.Success;
            }
            catch (NumberParseException)
            {
                return new ValidationResult(ErrorMessage ?? "Phone number is not valid.");
            }
        }
    }
}
