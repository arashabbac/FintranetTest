using FluentResults;
using Framework.Domain;
using Framework.Extensions;

namespace FintranetTest.Domain.ValueObjects;

public sealed class Name : ValueObject
{
    #region Static Member(s)
    public const int MaxLenght = 100;
    public const int MinLenght = 3;

    public static Result<Name> Create(string value)
    {
        value = value.Fix();

        if (value.IsNullOrWhiteSpace())
            return Result.Fail<Name>("Name value must not be empty");

        if (value.Length < MinLenght)
            return Result.Fail<Name>($"Name value must be greater than {MinLenght} characters");

        if (value.Length > MaxLenght)
            return Result.Fail<Name>($"Name value must be less than {MaxLenght} characters");

        return Result.Ok(new Name(value.ToLower()));
    }
    #endregion

    //For EF Core!
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private Name()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
    }

    private Name(string value)
    {
        Value = value;
    }


    public string Value { get; private set; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
