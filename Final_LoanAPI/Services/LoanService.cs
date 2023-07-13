using Final_LoanAPI.Data;
using Final_LoanAPI.Domain;
using Final_LoanAPI.Helpers;
using Final_LoanAPI.Models;
using Microsoft.Extensions.Options;
using System;
using System.Linq;

namespace Final_LoanAPI.Services
{
    public interface ILoanService
    {
        public Loan AddLoan(AddLoanModel loanModel, int userId);
        public Loan ModifyLoan(ModifyLoanModel model);
        public Loan DeleteLoan(int loanId);
        public IQueryable<Loan> GetOwnLoans(int userId);
    }
    public class LoanService : ILoanService
    {
        private UserContext _context;
        private readonly AppSettings _appSettings;
        public LoanService(UserContext context, IOptions<AppSettings> appSettings)
        {
            _context = context;
            _appSettings = appSettings.Value;
        }
        public Loan AddLoan(AddLoanModel loan, int userId)
        {
            try
            {
                var newLoan = new Loan();
                newLoan.UserId = userId;
                newLoan.LoanType = loan.LoanType;
                newLoan.LoanCurrency = loan.Currency;
                newLoan.LoanAmount = loan.Amount;
                newLoan.LoanTime = loan.LoanTime;
                _context.Loans.Add(newLoan);
                _context.SaveChanges();
                return newLoan;
            }

            catch
            {
                return null;
            }
        }



        public Loan ModifyLoan(ModifyLoanModel modify)
        {
            try
            {
                var loan = _context.Loans.Find(modify.LoanId);
                if (loan == null)
                {
                    return null;
                }

                if (modify.LoanType != null)
                {
                    loan.LoanType = modify.LoanType;
                }
                if (modify.Currency != null)
                {
                    loan.LoanCurrency = modify.Currency;
                }
                if (modify.Amount != 0)
                {
                    loan.LoanAmount = modify.Amount;
                }
                if (modify.LoanTime > 0)
                {
                    loan.LoanTime = modify.LoanTime;
                }

                _context.SaveChanges();

                return loan;
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<Loan> GetOwnLoans(int userId)
        {
            return _context.Loans.Where(loan => loan.UserId == userId);
        }

        public Loan DeleteLoan(int loanId)
        {
            try
            {
                var deleteLoan = _context.Loans.Find(loanId);
                _context.Loans.Remove(deleteLoan);
                _context.SaveChanges();
                return deleteLoan;
            }
            catch
            {
                return null;
            }
        }

    }
}
