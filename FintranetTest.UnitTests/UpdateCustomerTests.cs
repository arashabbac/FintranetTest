using FintranetTest.Domain.Aggregates;
using FintranetTest.Domain.Contracts;
using FintranetTest.Domain.ValueObjects;
using FintranetTest.UnitTests.Doubles;

namespace FintranetTest.UnitTests;

public class UpdateCustomerTests
{
    private Name _firstname;
    private Name _lastname;
    private BankAccountNumber _bankAccountNumber;
    private Email _email;
    private PhoneNumber _phoneNumber;
    private DateOnly _dateOfBirth;
    private ICustomerRepository _customerRepository;
    private Customer _customer;

    public UpdateCustomerTests()
    {
        _firstname = Name.Create("Arash").Value;
        _lastname = Name.Create("Abbasi").Value;
        _phoneNumber = PhoneNumber.Create("09351008895").Value;
        _bankAccountNumber = BankAccountNumber.Create("12354411558841").Value;
        _email = Email.Create("arash@gmail.com").Value;
        _dateOfBirth = DateOnly.FromDateTime(new DateTime(1993, 05, 17));
        _customerRepository = new FakeCustomerRepository();

        _customer = Customer.Create(_firstname, _lastname, _dateOfBirth, _phoneNumber, _email, _bankAccountNumber, _customerRepository).Value;
    }

    [Fact]
    public void CanNot_Update_Customer_With_Null_Firstname()
    {
        //Arrange
        _firstname = null;

        //Act
        Action action = () => _customer.Update(_firstname, _lastname,
            _dateOfBirth,
            _phoneNumber,
            _email,
            _bankAccountNumber,
            _customerRepository);

        //Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void CanNot_Update_Customer_With_Null_Lastname()
    {
        //Arrange
        _lastname = null;

        //Act
        Action action = () => _customer.Update(_firstname, _lastname,
            _dateOfBirth,
            _phoneNumber,
            _email,
            _bankAccountNumber,
            _customerRepository);

        //Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void CanNot_Update_Customer_With_Null_PhoneNumber()
    {
        //Arrange
        _phoneNumber = null;

        //Act
        Action action = () => _customer.Update(_firstname, _lastname,
            _dateOfBirth,
            _phoneNumber,
            _email,
            _bankAccountNumber,
            _customerRepository);

        //Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void CanNot_Update_Customer_With_Null_Email()
    {
        //Arrange
        _email = null;

        //Act
        Action action = () => _customer.Update(_firstname, _lastname,
            _dateOfBirth,
            _phoneNumber,
            _email,
            _bankAccountNumber,
            _customerRepository);

        //Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void CanNot_Update_Customer_With_Null_BankAccountNumber()
    {
        //Arrange
        _bankAccountNumber = null;

        //Act
        Action action = () => _customer.Update(_firstname, _lastname,
            _dateOfBirth,
            _phoneNumber,
            _email,
            _bankAccountNumber,
            _customerRepository);

        //Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void CanNot_Update_Customer_With_Null_CustomerRepository()
    {
        //Arrange
        _customerRepository = null;

        //Act
        Action action = () => _customer.Update(_firstname, _lastname,
            _dateOfBirth,
            _phoneNumber,
            _email,
            _bankAccountNumber,
            _customerRepository);

        //Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void CanNot_Update_Customer_With_Invalid_DateOfBirth()
    {
        //Arrange
        _dateOfBirth = default;

        //Act
        var result = _customer.Update(_firstname, _lastname,
            _dateOfBirth,
            _phoneNumber,
            _email,
            _bankAccountNumber,
            _customerRepository);

        //Assert
        result.IsFailed.Should().BeTrue();
    }

    [Fact]
    public void CanNot_Update_Customer_If_His_Email_Is_Already_Existed()
    {
        //Arrange
        var customer1 = Customer.Create(Name.Create("John").Value,
            Name.Create("Doe").Value,
            DateOnly.FromDateTime(new DateTime(1993, 01, 01)),
            PhoneNumber.Create("09121112255").Value,
            Email.Create("johndoe@gmail.com").Value,
            BankAccountNumber.Create("113215442131").Value,
            _customerRepository).Value;

        _customerRepository.AddAsync(customer1);

        //Act
        var result = _customer.Update(_firstname,
            _lastname,
            _dateOfBirth,
            _phoneNumber,
            Email.Create("Johndoe@gmail.com").Value,
            _bankAccountNumber,
            _customerRepository);

        //Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Select(c => c.Message).Should().Contain("Email has already been used");
    }

    [Fact]
    public void CanNot_Update_Customer_If_His_Information_Are_Already_Existed()
    {
        //Arrange
        var customer1 = Customer.Create(Name.Create("John").Value,
            Name.Create("Doe").Value,
            DateOnly.FromDateTime(new DateTime(1993, 01, 01)),
            PhoneNumber.Create("09121112255").Value,
            Email.Create("johndoe@gmail.com").Value,
            BankAccountNumber.Create("113215442131").Value,
            _customerRepository).Value;

        _customerRepository.AddAsync(customer1);

        //Act
        var result = _customer.Update(Name.Create("John").Value,
            Name.Create("Doe").Value,
            DateOnly.FromDateTime(new DateTime(1993, 01, 01)),
            _phoneNumber,
            _email,
            _bankAccountNumber,
            _customerRepository);

        //Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Select(c => c.Message).Should().Contain("Customer with this information already exists");
    }
}
