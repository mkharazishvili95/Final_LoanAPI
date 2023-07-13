using System.ComponentModel.DataAnnotations;
using Final_LoanAPI.Models;


public class ModifyLoanStatusModel
{
    [Required]
    public int LoanId { get; set; }

    [Required]
    public string LoanStatus { get; set; }
}
