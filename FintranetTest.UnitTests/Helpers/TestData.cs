namespace FintranetTest.UnitTests.Helpers;

public static class TestData
{
    public static IEnumerable<object[]> NullOrEmpty =>
        new List<object[]>
        {
            new object[] { null },
            new object[] { string.Empty },
            new object[] { "       " }
        };


    public static IEnumerable<object[]> IncorrectValuesForPhoneNumber =>
        new List<object[]>
        {
            new object[] { StringGenerator.Create(9,StringGeneratorType.Number) },
            new object[] { StringGenerator.Create(12,StringGeneratorType.Number) },
            new object[] { StringGenerator.Create(11,StringGeneratorType.Complex) },
            new object[] { StringGenerator.Create(11,StringGeneratorType.NumberAndCharacter) },
        };

    public static IEnumerable<object[]> IncorrectValuesForEmail =>
    new List<object[]>
    {
            new object[] { StringGenerator.Create(9,StringGeneratorType.NumberAndCharacter) },
            new object[] { StringGenerator.Create(151,StringGeneratorType.NumberAndCharacter) },
            new object[] { StringGenerator.Create(8,StringGeneratorType.Complex) },
    };

    public static IEnumerable<object[]> IncorrectValuesForName =>
        new List<object[]>
        {
            new object[] { StringGenerator.Create(2,StringGeneratorType.Character) },
            new object[] { StringGenerator.Create(101,StringGeneratorType.Character) },
        };

    public static IEnumerable<object[]> IncorrectValuesForBankAccountNumber =>
        new List<object[]>
        {
            new object[] { StringGenerator.Create(19,StringGeneratorType.Number) },
            new object[] { StringGenerator.Create(8,StringGeneratorType.Number) },
            new object[] { StringGenerator.Create(15,StringGeneratorType.Complex) },
            new object[] { StringGenerator.Create(14,StringGeneratorType.NumberAndCharacter) },
        };
}
