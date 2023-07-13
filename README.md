# Final_LoanAPI (By Mikheil Kharazishvili)

LOAN API is an ASP.NET CORE WEB API PROJECT that allows users to apply for and manage different types of loans. The API supports three types of loans: Car Loan, Fast Loan, and Buy With Credit. Loans can be requested in three different currencies: GEL, USD, and EUR. The loan application period ranges from 180 days to 1825 days.

## Features:

- Users can apply for loans and manage loans that are in progress.
- Only loans that are in progress can be modified by the user.
- Approved or rejected loans can only be modified by the bank operator.
- Users can take multiple loans in different currencies.
- Users can view their previously availed loans.
- The bank operator, registered as `admin123` in the database, has administrative rights.
  - The operator can log in and retrieve information about all loans based on the user ID.
  - The operator can delete and modify loans.
  - The operator can change other details such as currency amount, loan period, and status.

## Getting Started:

To use the API and access its services, you need to register as a user. Use the following registration form:

### Registration Form:

{  
  "FirstName": "string",
  "LastName": "string",
  "UserName": "string",
  "Password": "string",
  "MonthlySalary": 0,
  "age": 0
}
(FirstName must not be empty. It must contain from 1 to 50 chars!)
(LastName must not be empty. It must contain from 1 to 50 chars!)
(UserName must not be empty. It must be  between  6-15 chars!)
(Password must not be empty. It must be  between  6-15 chars!)
(Salary must be 500 or more)
(Age must be from 18 to 60 to make a loan!)

Once registered, you can log in using the following user login form:

### User Login Form:
{  
  "UserName": "string",
  "Password": "string"
}
## Loan Management:
### Adding a Loan:
Authorized users can add a loan using the following request body:
{
  "LoanType": "string",
  "Currency": "string",
  "Amount": 0,
  "LoanTime": 0
}
(Amount must be greater than 100 and less than 1 000 000)
(Loan time frame should be from minimum 180 days to 1825 days)

### Changing a Loan:
Authorized users can change a loan that is in progress and has not been approved or rejected. Use the following request body:
{
  "Amount": 0,
  "Currency": "string",
  "LoanType": "string",
  "LoanTime": 0
}
(Amount must be more than 100 and less than 1 000 000)
(Currency must not be empty and you should  write : GEL, USD or EUR)
(Loan Type must not be empty and you should  write : Fast Loan, Car Loan or Buy With Credit)
(Loan time frame should be from minimum 180 days to 1825 days)

## Getting Loan Information:
Authorized users can retrieve information about their loans that are in "processing" status. Use the following service:

### Get all Loan service: This service allows you to check and view all loans registered under your name. It provides loan information such as currency, date, amount, type, and loan ID. You can use the loan ID for further services such as the delete service or modify.

## Deleting a Loan:
Authorized users can delete a loan that they have and whose status is "processing". Use the following request body:
{
  "loanId": 0
}
(Loan Id is the loan id which has been shown once you have added a loan previously and once you will indicate that loan id to the service it will give you situation to delete that loan)

## Accountant (Admin) Services:
Accountant has the right to everything. He is an administrator and can change everything in the system, including operations that are prohibited to the user!
