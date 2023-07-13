using Final_LoanAPI.Data;
using Final_LoanAPI.Domain;
using Final_LoanAPI.Models;
using Final_LoanAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using FluentValidation;
using FluentValidation.Results;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Logging;
using Final_LoanAPI.Helpers;

namespace Final_LoanAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LoanController : ControllerBase
    {
        private readonly UserContext _context;
        private readonly ILoanService _loanService;
        private readonly ILogger<LoanController> _logger;

        public LoanController(UserContext context, ILoanService loanService, ILogger<LoanController> logger)
        {
            _context = context;
            _loanService = loanService;
            _logger = logger;
        }

        [Authorize(Roles = "User")]
        [HttpPost("addloan")]
        public IActionResult AddLoan([FromBody] AddLoanModel addloan)
        {
            LoanValidation validation = new LoanValidation(_context);
            var result = validation.Validate(addloan);

            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    _logger.LogError(error.ErrorMessage);
                }
                return BadRequest(result.Errors);
            }

            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var userId = GetUserIdFromToken(token);

            if (_context.Users.Find(userId).IsBlocked)
            {
                _logger.LogError("Error, User is Blocked");
                return Unauthorized("User is blocked");
            }

            _loanService.AddLoan(addloan, userId);
            return Ok(addloan);
        }

        [Authorize(Roles = Roles.User)]
        [HttpGet("getAllLoans")]
        public IActionResult GetOwnLoans()
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var userId = GetUserIdFromToken(token);
            var loans = _context.Loans.Where(x => x.UserId == userId);
            return Ok(loans);
        }

        [Authorize(Roles = Roles.User)]
        [HttpDelete("DeleteLoan")]
        public async Task<IActionResult> DeleteLoan(LoanIdModel model)
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var userId = GetUserIdFromToken(token);
            var ownLoans = _loanService.GetOwnLoans(userId);

            var loanToCheck = ownLoans.FirstOrDefault(loan => loan.Id == model.LoanId);
            if (loanToCheck == null)
            {
                _logger.LogError("Loan 404 Not Found");
                return NotFound("404 Not Found");
            }

            if (loanToCheck.LoanStatus != Status.Processing)
            {
                _logger.LogError("Loan is already processed!");
                return Unauthorized("You can't modify, Loan is already processed!");
            }

            _loanService.DeleteLoan(model.LoanId);
            return Ok("Loan Deleted");
        }

        [Authorize(Roles = Roles.User)]
        [HttpPut("ModifyLoan")]
        public IActionResult UpdateOwnLoan(ModifyLoanModel modify)
        {
            LoanValidation validation = new LoanValidation(_context);
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var userId = GetUserIdFromToken(token);
            var tempLoan = _loanService.ModifyLoan(modify);
            tempLoan.UserId = userId;

            var existingLoan = _context.Loans.Find(modify.LoanId);
            if (tempLoan.UserId != existingLoan?.UserId)
            {
                _logger.LogError("Not your loan");
                return Unauthorized("You can't modify, this is not your Loan!");
            }

            if (tempLoan.LoanStatus != Status.Processing)
            {
                _logger.LogError("Loan is already processed");
                return Unauthorized("You can't modify, Loan is already processed!");
            }

            var loanVerified = validation.ConvertTovalidation(tempLoan);
            var results = validation.Validate(loanVerified);
            if (results.IsValid)
            {
                _context.Loans.Update(tempLoan);
                _context.SaveChanges();
                return Ok("Loan Updated");
            }

            foreach (var error in results.Errors)
            {
                _logger.LogError(error.ErrorMessage);
            }
            return BadRequest(results.Errors);
        }

        private int GetUserIdFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadJwtToken(token);
            var userId = Convert.ToInt32(jsonToken.Claims.First(claim => claim.Type == "nameid").Value);
            return userId;
        }
    }
}
