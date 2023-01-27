using FintranetTest.Domain.ValueObjects;
using FintranetTest.UnitTests.Helpers;

namespace FintranetTest.UnitTests;

public class EmailTests
{
    [Theory]
    [MemberData(nameof(TestData.NullOrEmpty), MemberType = typeof(TestData))]
    public void Email_Must_Have_Value(string value)
    {
        //Act
        var result = Email.Create(value);

        //Assert
        result.IsFailed.Should().BeTrue();
    }

    [Theory]
    [MemberData(nameof(TestData.IncorrectValuesForEmail), MemberType = typeof(TestData))]
    public void Email_CanNot_Created_With_Incorrect_Value(string value)
    {
        //Act
        var result = Email.Create(value);

        //Assert
        result.IsFailed.Should().BeTrue();
    }

    [Fact]
    public void Email_Created_Successfully()
    {
        //Arrange
        var input = "arash@gmail.com";

        //Act
        var result = Email.Create(input);

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be(input);
    }
}
