using ExpenseManager.Application.Abstractions;
using ExpenseManager.Application.Commands;
using ExpenseManager.Application.Exceptions;
using ExpenseManager.Domain.Entities;
using FluentValidation;
using MediatR;
using Serilog;


namespace ExpenseManager.Application.CommandHandlers
{
    /// <summary>
    /// This code snippet represents the implementation of the AddExpenseHandler class, which is responsible for handling the AddExpense command. 
    /// It implements the IRequestHandler interface and is used to add a new expense to the system. 
    /// The Handle method is called when the command is received and performs the necessary validations and operations to add the expense. 
    /// It validates the request using the provided validator, checks if the user exists, checks for duplicate expenses, and verifies the currency. 
    /// If any validation or unexpected error occurs, it logs the error and throws the corresponding exception. 
    /// Finally, it adds the expense to the repository and returns the added expense.
    /// </summary>
    public class AddExpenseHandler(IExpenseRepository expenseRepository, IUserRepository userRepository, IValidator<AddExpense> validator) : IRequestHandler<AddExpense, Expense>
    {
        public async Task<Expense> Handle(AddExpense request, CancellationToken cancellationToken)
        {
            try
            {
                validator.ValidateAndThrow(request);

                var userFound = await userRepository.VerifyUserExistById(request.UserId);

                if (!userFound)
                {
                    throw new UserNotFoundException("User not found");
                }

                var duplicatedExpense = await expenseRepository.GetExpensesByUserIdDateAmount(request.UserId, request.Date, request.Amount);

                if(duplicatedExpense)
                {
                    throw new DuplicatedExpenseException("Duplicate expense detected");
                }
                

                var currencyUser = await userRepository.GetCurrencyByUserId(request.UserId);

                if (currencyUser != request.Currency) 
                {
                    throw new NotSameCurrencyException("User did not have same expense currency");
                }

                var expense = new Expense
                {
                    Amount = request.Amount,
                    Comment = request.Comment,
                    Currency = request.Currency,
                    Date = request.Date,
                    Nature = request.Nature,
                    UserId = request.UserId
                };

                return await expenseRepository.AddExpense(expense);
            }
            catch (ValidationException ex)
            {
                // Log the validation exception
                Log.Error(ex, "Validation error occurred: {ErrorMessage}", ex.Message);
                throw; 
            }
            catch (Exception ex)
            {
                // Log the exception
                Log.Error(ex, "An unexpected error occurred: {ErrorMessage}", ex.Message);
                throw;
            }
        }
    };
}