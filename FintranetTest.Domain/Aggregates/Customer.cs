using FintranetTest.Domain.Aggregates.Specifications;
using FintranetTest.Domain.Contracts;
using FintranetTest.Domain.ValueObjects;
using FluentResults;
using Framework.Domain;
namespace FintranetTest.Domain.Aggregates;

public sealed class Customer : AggregateRoot<int>
{
    #region Static Member(s)
    public static Result<Customer> Create(Name firstname,
        Name lastname,
        DateOnly dateOfBirth,
        PhoneNumber phoneNumber,
        Email email,
        BankAccountNumber bankAccountNumber,
        ICustomerRepository customerRepository)
    {
        ArgumentNullException.ThrowIfNull(email, nameof(email));
        ArgumentNullException.ThrowIfNull(lastname, nameof(lastname));
        ArgumentNullException.ThrowIfNull(firstname, nameof(firstname));
        ArgumentNullException.ThrowIfNull(phoneNumber, nameof(phoneNumber));
        ArgumentNullException.ThrowIfNull(bankAccountNumber, nameof(bankAccountNumber));
        ArgumentNullException.ThrowIfNull(customerRepository, nameof(customerRepository));

        if (dateOfBirth == default)
            return Result.Fail<Customer>("Date of birth is invalid");

        var isEmailAlreadyUsed = customerRepository
            .IsEmailAlreadyUsed(new IsEmailAlreadyUsedSpecification(email.Value));

        if (isEmailAlreadyUsed)
            return Result.Fail<Customer>("Email has already been used");

        var isCustomerExist = customerRepository
            .IsCustomerExist(new IsCustomerExistedSpecification(firstname.Value, lastname.Value, dateOfBirth));

        if (isCustomerExist)
            return Result.Fail<Customer>("Customer with this information already exists");

        return Result.Ok(new Customer(firstname, lastname, dateOfBirth, phoneNumber, email, bankAccountNumber));
    }
    #endregion

    //For EF Core
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private Customer() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    private Customer(Name firstname,
        Name lastname,
        DateOnly dateOfBirth,
        PhoneNumber phoneNumber,
        Email email,
        BankAccountNumber bankAccountNumber)
    {
        Firstname = firstname;
        Lastname = lastname;
        DateOfBirth = dateOfBirth;
        PhoneNumber = phoneNumber;
        Email = email;
        BankAccountNumber = bankAccountNumber;
    }

    public Name Firstname { get; private set; }
    public Name Lastname { get; private set; }
    public DateOnly DateOfBirth { get; private set; }
    public PhoneNumber PhoneNumber { get; private set; }
    public Email Email { get; private set; }
    public BankAccountNumber BankAccountNumber { get; private set; }

    public Result Update(Name firstname,
        Name lastname,
        DateOnly dateOfBirth,
        PhoneNumber phoneNumber,
        Email email,
        BankAccountNumber bankAccountNumber,
        ICustomerRepository customerRepository)
    {
        ArgumentNullException.ThrowIfNull(email, nameof(email));
        ArgumentNullException.ThrowIfNull(lastname, nameof(lastname));
        ArgumentNullException.ThrowIfNull(firstname, nameof(firstname));
        ArgumentNullException.ThrowIfNull(phoneNumber, nameof(phoneNumber));
        ArgumentNullException.ThrowIfNull(bankAccountNumber, nameof(bankAccountNumber));
        ArgumentNullException.ThrowIfNull(customerRepository, nameof(customerRepository));

        if (dateOfBirth == default)
            return Result.Fail("Date of birth is invalid");


        if (IsEmailModified(email))
        {
            var isEmailAlreadyUsed = customerRepository
                .IsEmailAlreadyUsed(new IsEmailAlreadyUsedSpecification(email.Value));

            if (isEmailAlreadyUsed)
                return Result.Fail("Email has already been used");
        }

        if (IsCustomerInfoModified(firstname, lastname, dateOfBirth))
        {
            var isCustomerExist = customerRepository
                .IsCustomerExist(new IsCustomerExistedSpecification(firstname.Value, lastname.Value, dateOfBirth));

            if (isCustomerExist)
                return Result.Fail("Customer with this information already exists");
        }

        Email = email;
        Lastname = lastname;
        Firstname = firstname;
        PhoneNumber = phoneNumber;
        DateOfBirth = dateOfBirth;
        BankAccountNumber = bankAccountNumber;

        return Result.Ok();
    }

    private bool IsEmailModified(Email email) => !Email.Value.Equals(email.Value);

    private bool IsCustomerInfoModified(Name firstname, Name lastname, DateOnly dateOfBirth) => Firstname.Value.Equals(firstname.Value) == false ||
            Lastname.Value.Equals(lastname.Value) == false ||
            DateOfBirth != dateOfBirth;
}
