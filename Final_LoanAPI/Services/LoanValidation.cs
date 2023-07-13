using Final_LoanAPI.Data;
using Final_LoanAPI.Domain;
using FluentValidation;
using Final_LoanAPI.Models;
using System.Collections.Generic;

namespace Final_LoanAPI.Services
{
    public class LoanValidation : AbstractValidator<AddLoanModel>
    {
        UserContext _context;
        public LoanValidation(UserContext context)
        {
            _context = context;


            RuleFor(AddLoanModel => AddLoanModel.LoanType)
                    .NotEmpty().NotNull().WithMessage("Loan Type is required")
                    .Must(LoanType).WithMessage($"Invalid Loan Type. Loan  must be one of the following: {Type.FastLoan},{Type.CarLoan}{Type.BuyWithCredit}");
            RuleFor(AddLoanModel => AddLoanModel.Currency).NotEmpty().NotNull().Must(choosecurrency).WithMessage
                ($"Invalid Loan Currency. Loan Currency must be one of the following: {Currency.GEL},{Currency.USD} , {Currency.EUR}");
            RuleFor(AddLoanModel => AddLoanModel.Amount).NotEmpty().NotNull().LessThan(1000000).GreaterThan(100).WithMessage
                ("Amount must be greater than 100 and less than 1 000 000");
            RuleFor(AddLoanModel => AddLoanModel.LoanTime).NotEmpty().NotNull().LessThan(1825).GreaterThan(180).WithMessage
                ("Loan time frame should be from minimum 6 month to 5 years maximum");
        }

        private bool LoanType(string type)
        {
            try
            {
                List<string> loanTypes = new List<string>()
                {
                    Type.FastLoan.ToLower(),
                    Type.CarLoan.ToLower(),
                    Type.BuyWithCredit.ToLower(),

                };
                var loanTypeLower = type.ToLower();
                if (loanTypes.Contains(loanTypeLower))
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
        public AddLoanModel ConvertTovalidation(Loan loan)
        {
            try
            {
                AddLoanModel loanModel = new AddLoanModel();
                loanModel.Currency = loan.LoanCurrency;
                loanModel.Amount = loan.LoanAmount;
                loanModel.LoanType = loan.LoanType;
                loanModel.LoanTime = loan.LoanTime;

                return loanModel;
            }
            catch
            {
                return null;
            }
        }
        private bool choosecurrency(string currency)
        {
            try
            {
                List<string> choosecurrency = new List<string>()
                {
                    Currency.GEL.ToLower(),
                    Currency.USD.ToLower(),
                    Currency.EUR.ToLower()
                };
                var loanTypeLower = currency.ToLower();
                if (choosecurrency.Contains(loanTypeLower))
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
