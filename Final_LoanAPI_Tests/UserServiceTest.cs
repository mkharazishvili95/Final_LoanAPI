using Final_LoanAPI.Domain;
using Final_LoanAPI.Helpers;
using Final_LoanAPI.Models;
using Final_LoanAPI_Tests.FakeServices;
using Xunit;

namespace Loan_WebApi.tests
{
    public class UserServiceTest
    {
        private readonly UserServiceFake _userService;

        public UserServiceTest()
        {
            _userService = new UserServiceFake();
        }

        [Fact]
        public void GetUserInfo()
        {
            var userId = 1;
            var expectedUser = new User
            {
                Id = 1,
                FirstName = "Test",
                LastName = "Test",
                Age = 22,
                MonthlySalary = 3500,
                Username = "Test",
                Password = HashSettings.HashPassword("Test"),
                Role = "User",
                IsBlocked = false
            };

            var result = _userService.GetUserInfo(userId);

            Assert.Equal(expectedUser.Id, result.Id);
            Assert.Equal(expectedUser.FirstName, result.FirstName);
            Assert.Equal(expectedUser.LastName, result.LastName);
            Assert.Equal(expectedUser.Age, result.Age);
            Assert.Equal(expectedUser.MonthlySalary, result.MonthlySalary);
            Assert.Equal(expectedUser.Username, result.Username);
            Assert.Equal(expectedUser.Password, result.Password);
            Assert.Equal(expectedUser.Role, result.Role);
            Assert.Equal(expectedUser.IsBlocked, result.IsBlocked);
        }

        [Fact]
        public void Login_ValidCredentials()
        {
            var loginModel = new UserLoginModel
            {
                UserName = "Test",
                Password = "Test"
            };

            var expectedUser = new User
            {
                Id = 1,
                FirstName = "Test",
                LastName = "Test",
                Age = 22,
                MonthlySalary = 3500,
                Username = "Test",
                Password = HashSettings.HashPassword("Test"),
                Role = "User",
                IsBlocked = false
            };

            var result = _userService.Login(loginModel);

            Assert.Equal(expectedUser.Id, result.Id);
            Assert.Equal(expectedUser.FirstName, result.FirstName);
            Assert.Equal(expectedUser.LastName, result.LastName);
            Assert.Equal(expectedUser.Age, result.Age);
            Assert.Equal(expectedUser.MonthlySalary, result.MonthlySalary);
            Assert.Equal(expectedUser.Username, result.Username);
            Assert.Equal(expectedUser.Password, result.Password);
            Assert.Equal(expectedUser.Role, result.Role);
            Assert.Equal(expectedUser.IsBlocked, result.IsBlocked);
        }

        [Fact]
        public void Login_InvalidCredentials_NullUser()
        {
            var loginModel = new UserLoginModel
            {
                UserName = "Invalid",
                Password = "Credentials"
            };

            var result = _userService.Login(loginModel);

            Assert.Null(result);
        }

        [Fact]
        public void Register()
        {
            var registerModel = new UserRegisterModel
            {
                FirstName = "New",
                LastName = "User",
                UserName = "newuser",
                Password = "password123",
                Age = 30,
                MonthlySalary = 5000
            };

            var expectedUser = new User
            {
                Id = 0,
                FirstName = "New",
                LastName = "User",
                Age = 30,
                MonthlySalary = 5000,
                Username = "newuser",
                Password = HashSettings.HashPassword("password123"),
                Role = "User",
                IsBlocked = false
            };

            var result = _userService.Register(registerModel);

            Assert.Equal(expectedUser.Id, result.Id);
            Assert.Equal(expectedUser.FirstName, result.FirstName);
            Assert.Equal(expectedUser.LastName, result.LastName);
            Assert.Equal(expectedUser.Age, result.Age);
            Assert.Equal(expectedUser.MonthlySalary, result.MonthlySalary);
            Assert.Equal(expectedUser.Username, result.Username);
            Assert.Equal(expectedUser.Password, result.Password);
            Assert.Equal(expectedUser.Role, result.Role);
            Assert.Equal(expectedUser.IsBlocked, result.IsBlocked);
        }
    }
}
