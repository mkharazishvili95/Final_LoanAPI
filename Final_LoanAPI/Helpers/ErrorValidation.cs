using FluentValidation.Results;
using System.Collections.Generic;
using System.Linq;

namespace Final_LoanAPI.Helpers
{
    public class ErrorValidation
    {
        public static List<string> GettingError(ValidationResult result)
        {
            var fullErrorList = result.Errors.ToList<ValidationFailure>();
            var errorMessageList = new List<string>();
            foreach (var i in fullErrorList)
            {
                errorMessageList.Add(i.ErrorMessage);
            }
            return errorMessageList;
        }
    }
}
