using FintranetTest.UnitTests.Helpers;
using FintranetTest.Domain.ValueObjects;

namespace FintranetTest.UnitTests;

public class BankAccountNumberTests
{
    [Theory]
    [MemberData(nameof(TestData.NullOrEmpty), MemberType = typeof(TestData))]
    public void BankAccountNumber_Must_Have_Value(string value)
    {
        //Act
        var result = BankAccountNumber.Create(value);

        //Assert
        result.IsFailed.Should().BeTrue();
    }

    [Theory]
    [MemberData(nameof(TestData.IncorrectValuesForBankAccountNumber), MemberType = typeof(TestData))]
    public void BankAccountNumber_CanNot_Created_With_Incorrect_Value(string value)
    {
        //Act
        var result = BankAccountNumber.Create(value);

        //Assert
        result.IsFailed.Should().BeTrue();
    }

    [Fact]
    public void BankAccountNumber_Created_Successfully()
    {
        //Arrange
        var input = "12544122541";

        //Act
        var result = BankAccountNumber.Create(input);

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be(input);
    }
}
