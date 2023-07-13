using Final_LoanAPI.Data;
using Final_LoanAPI.Domain;
using FluentValidation.Results;
using FluentValidation;
using Final_LoanAPI.Helpers;
using Final_LoanAPI.Models;
using Final_LoanAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System;
using Microsoft.Extensions.Logging;

namespace Final_LoanAPI.Controllers
{
    [Authorize]
    [Route("api/[Controller]")]
    public class UserController : Controller
    {
        private readonly UserContext _context;
        private readonly IUserService _userService;
        private readonly AppSettings _appSettings;
        private readonly ILogger<UserController> _logger;
        private readonly ITokenGen _tokenGen;

        public UserController(IUserService userService, UserContext context, IOptions<AppSettings> appSettings, ILogger<UserController> logger, ITokenGen tokenGen)
        {
            _context = context;
            _userService = userService;
            _appSettings = appSettings.Value;
            _logger = logger;
            _tokenGen = tokenGen;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login(UserLoginModel user)
        {
            var userLogin = _userService.Login(user);
            if (userLogin == null)
            {
                _logger.LogError("Error! UserName or Password is empty!");
                return BadRequest("Error! UserName or Password is empty!");
            }

            // Save log to the database
            Loggs log = new Loggs
            {
                Values = $"User {userLogin.Username} logged in.",
                Created = DateTime.Now
            };
            _context.Loggs.Add(log);
            _context.SaveChanges();

            var token = _tokenGen.GenerateToken(userLogin);
            return Ok(new { Username = userLogin.Username, UserRole = userLogin.Role, Token = token });
        }


        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<User>> AddUser(UserRegisterModel registerData)
        {
            var validation = new UserValidation(_context);
            ValidationResult result = validation.Validate(registerData);
            if (result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    _logger.LogError(error.ErrorMessage);
                }

                _userService.Register(registerData);
                await _context.SaveChangesAsync();
                return Ok("User successfully created!");
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }
    }
}
