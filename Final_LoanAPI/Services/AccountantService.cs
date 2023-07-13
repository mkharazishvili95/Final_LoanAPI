using Final_LoanAPI.Data;
using Final_LoanAPI.Domain;
using Final_LoanAPI.Helpers;
using Final_LoanAPI.Models;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading.Tasks;

namespace Final_LoanAPI.Services
{
    public interface IAccountantService
    {
        Task<User> OpenAccountantAccount();
        Task<IQueryable<Loan>> GetLoanByID(int loanId);
        Loan DeleteLoanById(int loanId);
        Loan ModifyLoan(int loanId, ModifyLoanModel modifyLoan);
        Loan ModifyLoanStatus(int loanId, string loanStatus);

        User BlockUserForLoan(int userId);
        User UnblockUser(int userId);
    }

    public class AccountantService : IAccountantService
    {
        private readonly UserContext _context;

        public AccountantService(UserContext context)
        {
            _context = context;
        }

        public async Task<User> OpenAccountantAccount()
        {
            try
            {
                var accountant = new User()
                {
                    FirstName = "admin123",
                    LastName = "admin123",
                    Age = 27,
                    MonthlySalary = 9999,
                    Username = "admin123",
                    Password = HashSettings.HashPassword("admin123"),
                    Role = "Accountant",
                    IsBlocked = false
                };

                await _context.Users.AddAsync(accountant);
                await _context.SaveChangesAsync();

                return accountant;
            }
            catch
            {
                return null;
            }
        }

        public async Task<IQueryable<Loan>> GetLoanByID(int userId)
        {
            return _context.Loans.Where(x => x.UserId == userId);
        }

        public Loan DeleteLoanById(int loanId)
        {
            try
            {
                var loan = _context.Loans.Find(loanId);

                if (loan == null)
                {
                    return null;
                }

                _context.Loans.Remove(loan);
                _context.SaveChanges();

                return loan;
            }
            catch
            {
                return null;
            }
        }

        public Loan ModifyLoan(int loanId, ModifyLoanModel modifyLoan)
        {
            try
            {
                var loan = _context.Loans.Find(loanId);

                if (loan == null)
                {
                    return null;
                }

                loan.LoanAmount = modifyLoan.Amount;
                loan.LoanCurrency = modifyLoan.Currency;
                loan.LoanType = modifyLoan.LoanType;
                loan.LoanTime = modifyLoan.LoanTime;

                _context.Loans.Update(loan);
                _context.SaveChanges();

                return loan;
            }
            catch
            {
                return null;
            }
        }

        public Loan ModifyLoanStatus(int loanId, string loanStatus)
        {
            try
            {
                var loan = _context.Loans.Find(loanId);

                if (loan == null)
                {
                    return null;
                }

                loan.LoanStatus = loanStatus;

                _context.Loans.Update(loan);
                _context.SaveChanges();

                return loan;
            }
            catch
            {
                return null;
            }
        }


        public User BlockUserForLoan(int userId)
        {
            try
            {
                var user = _context.Users.Find(userId);

                if (user != null)
                {
                    user.IsBlocked = true;
                    _context.SaveChanges();
                }

                return user;
            }
            catch
            {
                return null;
            }
        }

        public User UnblockUser(int userId)
        {
            try
            {
                var user = _context.Users.Find(userId);

                if (user != null)
                {
                    user.IsBlocked = false;
                    _context.SaveChanges();
                }

                return user;
            }
            catch
            {
                return null;
            }
        }
    }
}
