using System;

namespace Final_LoanAPI.Models
{
    public class ModifyLoanModel
    {
        public int LoanId { get; set; }
        public int Amount { get; set; }
        public string Currency { get; set; }
        public string LoanType { get; set; }
        public int LoanTime { get; set; }
    }
}
