using System;

namespace Final_LoanAPI.Models
{
    public class AddLoanModel
    {
        public string LoanType { get; set; }
        public string Currency { get; set; }
        public int Amount { get; set; }
        public int LoanTime { get; set; }
    }
}
