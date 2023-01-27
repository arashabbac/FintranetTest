using FintranetTest.Domain.ValueObjects;
using FintranetTest.Presentation.Server.Commands;
using FluentValidation;

namespace FintranetTest.Presentation.Server.Validators;

public class UpdateCustomerCommandValidator : AbstractValidator<UpdateCustomerCommand>
{
    public UpdateCustomerCommandValidator()
    {
        RuleFor(c => c.Firstname)
            .NotEmpty()
                .WithMessage("First name is required");

        RuleFor(c => c.Lastname)
            .NotEmpty()
                .WithMessage("Last name is required");

        RuleFor(c => c.DateOfBirth)
            .NotNull()
                .WithMessage("Date of birth is required");

        RuleFor(c => c.Email)
            .NotEmpty()
                .WithMessage("Email name is required")
            .Matches(Email.ValidEmailRegex)
                .WithMessage("Email is invalid");

        RuleFor(c => c.PhoneNumber)
            .NotEmpty()
                .WithMessage("Phone number is required")
            .Matches(PhoneNumber.ValidPhoneNumberRegex)
                .WithMessage("Phone number is invalid");

        RuleFor(c => c.BankAccountNumber)
            .NotEmpty()
                .WithMessage("Bank account number is required")
            .Matches(BankAccountNumber.ValidBankAccountNumberRegex)
                .WithMessage("Bank account number is invalid");

    }
}
