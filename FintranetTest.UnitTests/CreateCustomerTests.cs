using FintranetTest.Domain.Aggregates;
using FintranetTest.Domain.Contracts;
using FintranetTest.Domain.ValueObjects;
using FintranetTest.UnitTests.Doubles;

namespace FintranetTest.UnitTests;

public class CreateCustomerTests
{
    private Name _firstname;
    private Name _lastname;
    private BankAccountNumber _bankAccountNumber;
    private Email _email;
    private PhoneNumber _phoneNumber;
    private DateOnly _dateOfBirth;
    private ICustomerRepository _customerRepository;

    public CreateCustomerTests()
    {
        _firstname = Name.Create("Arash").Value;
        _lastname = Name.Create("Abbasi").Value;
        _phoneNumber = PhoneNumber.Create("09351008895").Value;
        _bankAccountNumber = BankAccountNumber.Create("12354411558841").Value;
        _email = Email.Create("arash@gmail.com").Value;
        _dateOfBirth = DateOnly.FromDateTime(new DateTime(1993, 05, 17));
        _customerRepository = new FakeCustomerRepository();
    }

    [Fact]
    public void CanNot_Create_Customer_With_Null_Firstname()
    {
        //Arrange
        _firstname = null;

        //Act
        Action action = () => Customer.Create(_firstname, _lastname,
            _dateOfBirth,
            _phoneNumber,
            _email,
            _bankAccountNumber,
            _customerRepository);

        //Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void CanNot_Create_Customer_With_Null_Lastname()
    {
        //Arrange
        _lastname = null;

        //Act
        Action action = () => Customer.Create(_firstname, _lastname,
            _dateOfBirth,
            _phoneNumber,
            _email,
            _bankAccountNumber,
            _customerRepository);

        //Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void CanNot_Create_Customer_With_Null_PhoneNumber()
    {
        //Arrange
        _phoneNumber = null;

        //Act
        Action action = () => Customer.Create(_firstname, _lastname,
            _dateOfBirth,
            _phoneNumber,
            _email,
            _bankAccountNumber,
            _customerRepository);

        //Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void CanNot_Create_Customer_With_Null_Email()
    {
        //Arrange
        _email = null;

        //Act
        Action action = () => Customer.Create(_firstname, _lastname,
            _dateOfBirth,
            _phoneNumber,
            _email,
            _bankAccountNumber,
            _customerRepository);

        //Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void CanNot_Create_Customer_With_Null_BankAccountNumber()
    {
        //Arrange
        _bankAccountNumber = null;

        //Act
        Action action = () => Customer.Create(_firstname, _lastname,
            _dateOfBirth,
            _phoneNumber,
            _email,
            _bankAccountNumber,
            _customerRepository);

        //Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void CanNot_Create_Customer_With_Null_CustomerRepository()
    {
        //Arrange
        _customerRepository = null;

        //Act
        Action action = () => Customer.Create(_firstname, _lastname,
            _dateOfBirth,
            _phoneNumber,
            _email,
            _bankAccountNumber,
            _customerRepository);

        //Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void CanNot_Create_Customer_With_Invalid_DateOfBirth()
    {
        //Arrange
        _dateOfBirth = default;

        //Act
        var result = Customer.Create(_firstname, _lastname,
            _dateOfBirth,
            _phoneNumber,
            _email,
            _bankAccountNumber,
            _customerRepository);

        //Assert
        result.IsFailed.Should().BeTrue();
    }

    [Fact]
    public void CanNot_Create_Customer_With_Duplicate_Email()
    {
        //Arrange
        var customer1 = Customer.Create(_firstname, _lastname,
            _dateOfBirth,
            _phoneNumber,
            _email,
            _bankAccountNumber,
            _customerRepository).Value;

        _customerRepository.AddAsync(customer1);

        //Act
        var customer2 = Customer.Create(Name.Create("Arash").Value, Name.Create("Abbac").Value,
            _dateOfBirth,
            PhoneNumber.Create("09335551144").Value,
            Email.Create("Arash@gmail.com").Value,
            BankAccountNumber.Create("1222214455444").Value,
            _customerRepository);

        //Assert
        customer2.IsFailed.Should().BeTrue();
        customer2.Errors.Select(c => c.Message).Should().Contain("Email has already been used");
    }

    [Fact]
    public void CanNot_Create_Customer_With_Duplicate_FirstName_LastName_DateOfBirth()
    {
        //Arrange
        var customer1 = Customer.Create(_firstname, _lastname,
            _dateOfBirth,
            _phoneNumber,
            _email,
            _bankAccountNumber,
            _customerRepository).Value;

        _customerRepository.AddAsync(customer1);

        //Act
        var customer2 = Customer.Create(Name.Create("arash").Value, Name.Create("abbasi").Value,
            _dateOfBirth,
            PhoneNumber.Create("09335551144").Value,
            Email.Create("ArashAbbasi@gmail.com").Value,
            BankAccountNumber.Create("1222214455444").Value,
            _customerRepository);

        //Assert
        customer2.IsFailed.Should().BeTrue();
        customer2.Errors.Select(c => c.Message).Should().Contain("Customer with this information already exists");
    }
}
