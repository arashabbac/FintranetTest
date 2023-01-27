using FintranetTest.Domain.ValueObjects;
using FintranetTest.UnitTests.Helpers;

namespace FintranetTest.UnitTests;

public class PhoneNumberTests
{
    [Theory]
    [MemberData(nameof(TestData.NullOrEmpty), MemberType = typeof(TestData))]
    public void PhoneNumber_Must_Have_Value(string value)
    {
        //Act
        var result = PhoneNumber.Create(value);

        //Assert
        result.IsFailed.Should().BeTrue();
    }

    [Theory]
    [MemberData(nameof(TestData.IncorrectValuesForPhoneNumber), MemberType = typeof(TestData))]
    public void PhoneNumber_CanNot_Created_With_Incorrect_Value(string value)
    {
        //Act
        var result = PhoneNumber.Create(value);

        //Assert
        result.IsFailed.Should().BeTrue();
    }

    [Fact]
    public void PhoneNumber_Created_Successfully()
    {
        //Arrange
        var input = "09351008895";

        //Act
        var result = PhoneNumber.Create(input);

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be(input);
    }
}
