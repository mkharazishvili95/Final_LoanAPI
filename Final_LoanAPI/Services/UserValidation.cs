using Final_LoanAPI.Data;
using FluentValidation;
using Final_LoanAPI.Models;
using System.Linq;

namespace Final_LoanAPI.Services
{
    public class UserValidation : AbstractValidator<UserRegisterModel>
    {
        UserContext _context;
        public UserValidation(UserContext context)
        {
            _context = context;
            RuleFor(UserRegisterModel => UserRegisterModel.FirstName).NotEmpty().WithMessage("FirstName must not be empty!")
            .Length(1, 50).WithMessage("FirstName must be between 1 and 50 chars!");

            RuleFor(UserRegisterModel => UserRegisterModel.LastName).NotEmpty().WithMessage("LastName must not be empty!")
                .Length(1, 50).WithMessage("LastName length must be between 1 and 50 chars!");

            RuleFor(UserRegisterModel => UserRegisterModel.UserName).Length(6, 15)
                .WithMessage("UserName length must be between 6 and 15 chars!").NotEmpty()
                .WithMessage("UserName must not be empty!")
                .Must(distinctUserName).WithMessage("Username already exists!");

            RuleFor(UserRegisterModel => UserRegisterModel.Password).Length(6, 15).NotEmpty()
                .WithMessage("Password must not be empty and it must be between 6 and 16 Chars!");

            RuleFor(UserRegisterModel => UserRegisterModel.MonthlySalary).GreaterThanOrEqualTo(500)
                .WithMessage("Your monthly income must be 500 or more to qualify for a loan!");

            RuleFor(RegistrationModel => RegistrationModel.Age).GreaterThanOrEqualTo(18)
                .WithMessage("You must be more than 17 years old to apply for a loan").LessThan(60).WithMessage
                ("Your age should be less than 60 to apply for a loan!");
        }
        private bool distinctUserName(string userName)
        {
            try
            {
                var userNameCheck = _context.Users.Where(x => x.Username.ToUpper() == userName.ToUpper()).FirstOrDefault();
                if (userNameCheck == null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
