using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi.Core.Models;

namespace Final_LoanAPI.Domain
{
    public class Accountant
    {

        public int Id { get; set; }
        public string Role { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public User User { get; set; }

        public Accountant()
        {
            Role = Roles.Accountant;
        }
    }
}
