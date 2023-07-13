using Final_LoanAPI.Models;
using Final_LoanAPI_Tests.FakeServices;
using System.Collections.Generic;
using System;
using System.Linq;
using Xunit;
using Final_LoanAPI.Domain;

namespace Loan_WebApi.tests
{
    public class LoanServiceTest
    {
        private readonly LoanServiceFake _loanService;

        public LoanServiceTest()
        {
            _loanService = new LoanServiceFake();
        }

        [Fact]
        public void LoanAdd()
        {
            var addLoan = new AddLoanModel { Amount = 1500, Currency = "gel", LoanTime = 350, LoanType = "FastLoan" };
            var userId = 2;

            var result = _loanService.AddLoan(addLoan, userId);

            Assert.NotNull(result);
        }

        public Loan DeleteLoan(int loanId)
        {
            var loans = new List<Loan>() { new Loan() { Id = 2 }, new Loan() { Id = 3 } };
            var loanToDelete = loans.FirstOrDefault(loan => loan.Id == loanId);

            if (loanToDelete == null)
            {
                throw new Exception($"Loan with ID {loanId} not found.");
            }

            loans.Remove(loanToDelete);
            return loanToDelete;
        }

        [Fact]
        public void DeleteLoan_ExistingLoan_ReturnsDeletedLoan()
        {
            // Arrange
            var loanId = 2;
            _loanService.AddLoan(new AddLoanModel(), 1);
            _loanService.AddLoan(new AddLoanModel(), 2);
            _loanService.AddLoan(new AddLoanModel(), 3);

            // Act
            var deletedLoan = _loanService.DeleteLoan(loanId);

            // Assert
            Assert.NotNull(deletedLoan);
            Assert.Equal(loanId, deletedLoan.Id);
        }

        [Fact]
        public void DeleteLoan_NonExistingLoan_ReturnsNull()
        {
            // Arrange
            var loanId = 5;
            _loanService.AddLoan(new AddLoanModel(), 1);
            _loanService.AddLoan(new AddLoanModel(), 2);
            _loanService.AddLoan(new AddLoanModel(), 3);

            // Act
            var deletedLoan = _loanService.DeleteLoan(loanId);

            // Assert
            Assert.Null(deletedLoan);
        }
    



    [Fact]
        public void GetOwnLoans()
        {
            var loanCount = 1;
            var gotLoan = _loanService.GetOwnLoans(1);

            Assert.Equal(loanCount, gotLoan.Count());
        }

    }
}
