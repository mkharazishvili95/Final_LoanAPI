using Final_LoanAPI.Domain;
using Final_LoanAPI.Helpers;
using Final_LoanAPI.Models;
using Final_LoanAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_LoanAPI_Tests.FakeServices
{
    public class UserServiceFake : IUserService
    {
        public User GetUserInfo(int id)
        {
            var user = new User()
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

            return user;

        }

        public User Login(UserLoginModel login)
        {
            var user = new User()
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
            if (string.IsNullOrEmpty(login.UserName) || (string.IsNullOrEmpty(login.Password)))
            {
                return null;
            }
            if (user == null)
            {
                return null;
            }
            if (HashSettings.HashPassword(login.Password) != user.Password)
            {
                return null;
            }
            return user;
        }

        public User Register(UserRegisterModel registerData)
        {
            User user = new User();
            user.FirstName = registerData.FirstName;
            user.LastName = registerData.LastName;
            user.Username = registerData.UserName;
            user.Password = HashSettings.HashPassword(registerData.Password);
            user.Age = registerData.Age;
            user.MonthlySalary = registerData.MonthlySalary;
            return user;
        }
    }
}
