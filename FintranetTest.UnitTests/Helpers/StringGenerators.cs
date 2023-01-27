namespace FintranetTest.UnitTests.Helpers;

internal enum StringGeneratorType
{
    Number,
    Character,
    NumberAndCharacter,
    Complex
}

internal static class StringGenerator
{
    private static readonly Random _random = new();
    const string _characters = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz ";
    const string _allowedNumbers = "0123456789";
    const string _symbols = @"!@#$%^&*()_+-=/\,|<>{}[]";

    internal static string Create(int stringLength, StringGeneratorType stringGeneratorType = StringGeneratorType.Complex)
    {
        char[] chars = new char[stringLength];

        var typeOfOutput = stringGeneratorType switch
        {
            StringGeneratorType.Number => _allowedNumbers,
            StringGeneratorType.Character => _characters,
            StringGeneratorType.NumberAndCharacter => _allowedNumbers + _characters,
            _ => _allowedNumbers + _characters + _symbols,
        };

        for (int i = 0; i < stringLength; i++)
            chars[i] = typeOfOutput[_random.Next(0, typeOfOutput.Length)];

        return new string(chars);
    }
}
