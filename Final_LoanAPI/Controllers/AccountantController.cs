using Final_LoanAPI.Data;
using Final_LoanAPI.Domain;
using Final_LoanAPI.Helpers;
using Final_LoanAPI.Models;
using Final_LoanAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace Final_LoanAPI.Controllers
{
    [Authorize(Roles = Roles.Accountant)]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountantController : ControllerBase
    {
        private readonly UserContext _context;
        private readonly IAccountantService _accountantService;
        private readonly ILogger<AccountantController> _loggs;
        private readonly ITokenGen _token;

        public AccountantController(UserContext context, IAccountantService accountantService, ILogger<AccountantController> loggs, ITokenGen token)
        {
            _context = context;
            _accountantService = accountantService;
            _loggs = loggs;
            _token = token;
        }

        [AllowAnonymous]
        [HttpPost("generateaccountant")]
        public async Task<IActionResult> OpenAccountantAccount()
        {

            var accountant = await _accountantService.OpenAccountantAccount();
            var tokenString = _token.GenerateToken(accountant);
            accountant.Token = tokenString;
            _context.Users.Update(accountant);
            _context.SaveChanges();
            return Ok($"Accountant Credentials: Username: admin123 " +
                $"Password: admin123" +
                $"Token: {accountant.Token}");
        }

        [HttpGet("GetLoanByID")]
        public async Task<ActionResult<Loan>> GetLoanByID(UserIdModel UserId)
        {
            if (_context.Users.Find(UserId.UserId) == null)
            {
                _loggs.LogError("Not Found! 404");
                return NotFound("Not Found! 404!");
            }
            var loans = _accountantService.GetLoanByID(UserId.UserId);

            return Ok(loans);
        }

        [HttpPut("ModifyLoan")]
        public async Task<ActionResult<Loan>> ModifyLoan(ModifyLoanModel modifyLoan)
        {
            var loanId = modifyLoan.LoanId;
            var loan = _accountantService.ModifyLoan(loanId, modifyLoan); // Pass modifyLoan as an argument
            if (loan == null)
            {
                _loggs.LogError("Not Found! 404");
                return NotFound("Not Found! 404");
            }

            return Ok("This Loan is Updated");
        }
        [HttpPut("ModifyLoanStatus")]
        public async Task<ActionResult<Loan>> ModifyLoanStatus(ModifyLoanStatusModel modifyLoanStatus)
        {
            var loanId = modifyLoanStatus.LoanId;
            var loanStatus = modifyLoanStatus.LoanStatus;

            var loan = _accountantService.ModifyLoanStatus(loanId, loanStatus);
            if (loan == null)
            {
                _loggs.LogError("Not Found! 404");
                return NotFound("Not Found! 404");
            }

            return Ok("Loan status updated successfully");
        }



        [HttpDelete("DeleteLoanById")]
        public async Task<ActionResult<Loan>> DeleteLoanById(LoanIdModel loanId)
        {
            var loan = _context.Loans.Find(loanId.LoanId);
            if (loan == null)
            {
                _loggs.LogError("Not Found! 404");
                return NotFound("Not Found! 404");
            }
            _accountantService.DeleteLoanById(loanId.LoanId);
            return Ok("This Loan is Deleted");
        }


        [HttpPut("BlockUserForNewLoan")]
        public async Task<ActionResult<User>> BlockUserForLoan(UserIdModel userId)
        {
            if (_context.Users.Find(userId.UserId) == null)
            {
                _loggs.LogError("Not Found! 404");
                return NotFound("Not Found! 404");
            }
            var blockedUser = _accountantService.BlockUserForLoan(userId.UserId);
            _context.Users.Update(blockedUser);
            _context.SaveChanges();
            return Ok("This User is Blocked!");
        }

        [HttpPut("UnblockUser")]
        public async Task<ActionResult<User>> UnblockUser(UserIdModel userId)
        {
            if (_context.Users.Find(userId.UserId) == null)
            {
                _loggs.LogError("Not Found! 404");
                return NotFound("Not Found! 404");
            }
            var unblockedUser = _accountantService.UnblockUser(userId.UserId);
            _context.Users.Update(unblockedUser);
            _context.SaveChanges();
            return Ok("This User is Unblocked!");
        }
    }
}
