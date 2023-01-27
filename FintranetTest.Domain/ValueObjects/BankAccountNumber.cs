using FluentResults;
using Framework.Domain;
using Framework.Extensions;
using System.Text.RegularExpressions;

namespace FintranetTest.Domain.ValueObjects;

public sealed class BankAccountNumber : ValueObject
{
    #region Static Member(s)
    public const int MaxLenght = 18;
    public const int MinLenght = 9;
    public const string ValidBankAccountNumberRegex = @"^\d{9,18}$";

    public static Result<BankAccountNumber> Create(string value)
    {
        value = value.Fix();

        if (value.IsNullOrWhiteSpace())
            return Result.Fail<BankAccountNumber>("Bank account number value must not be empty");

        if (Regex.IsMatch(value, ValidBankAccountNumberRegex) == false)
            return Result.Fail<BankAccountNumber>("Bank account number value is not valid");

        return Result.Ok(new BankAccountNumber(value));
    }
    #endregion

    //For EF Core!
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private BankAccountNumber()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
    }

    private BankAccountNumber(string value)
    {
        Value = value;
    }

    public string Value { get; private set; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
