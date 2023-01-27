using FluentResults;
using Framework.Domain;
using Framework.Extensions;
using System.Text.RegularExpressions;

namespace FintranetTest.Domain.ValueObjects;

public sealed class PhoneNumber : ValueObject
{
    #region Static Member(s)
    public const int FixedLenght = 11;
    public const string ValidPhoneNumberRegex = @"^\d{11}$";

    public static Result<PhoneNumber> Create(string value)
    {
        value = value.Fix();

        if (value.IsNullOrWhiteSpace())
            return Result.Fail<PhoneNumber>("Phone number value must not be empty");

        if (Regex.IsMatch(value, ValidPhoneNumberRegex) == false)
            return Result.Fail<PhoneNumber>("Phone number value is not valid");

        return Result.Ok(new PhoneNumber(value));
    }
    #endregion

    //For EF Core!
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private PhoneNumber()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
    }

    private PhoneNumber(string value)
    {
        Value = value;
    }


    public string Value { get; private set; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
