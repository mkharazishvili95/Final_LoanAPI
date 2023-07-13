using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Tweetinvi.Core.Models;

namespace Final_LoanAPI.Domain
{
    public class Loan
    {
        public Loan()
        {
            LoanStatus = Status.Processing;
        }
        public int Id { get; set; }
        public int UserId { get; set; }
        public int LoanAmount { get; set; }
        public string LoanCurrency { get; set; }
        public string LoanStatus { get; set; }
        public string LoanType { get; set; }
        public int LoanTime { get; set; }
        [JsonIgnore]
        public User User { get; set; }

    }


    public class Type
    {
        public const string BuyWithCredit = "Buy with Credit";
        public const string FastLoan = "Fast Loan";
        public const string CarLoan = "Car Loan";


    }
    public class Status
    {
        public const string Processing = "processing";
        public const string Approved = "Approved";
        public const string Declined = "Declined";
    }
    public class Currency
    {
        public const string GEL = "GEL";
        public const string USD = "USD";
        public const string EUR = "EUR";
    }
}
