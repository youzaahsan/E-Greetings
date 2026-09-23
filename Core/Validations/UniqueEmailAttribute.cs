using E_Greetings.Data;
using System;
using System.ComponentModel.DataAnnotations;

namespace E_Greetings.Core.Validations
{
    public class UniqueEmailAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var db = (ApplicationDbContext)context.GetService(typeof(ApplicationDbContext));

            bool exists = db.Users.Any(u => u.Email == (string)value);

            return exists
                  ? new ValidationResult("This email is already registered.")
                  : ValidationResult.Success;
        }
    }

}
