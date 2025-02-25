namespace ApiSults.Domain.Shared.Extensions;

public static partial class StringExtensions
{
    public static string OnlyNumbers(this string str) =>
        string.IsNullOrWhiteSpace(str) ? string.Empty : new string(str.Where(char.IsDigit).ToArray());

    public static string OnlyLetters(this string str) =>
        string.IsNullOrWhiteSpace(str) ? string.Empty : new string(str.Where(char.IsLetter).ToArray());

    public static bool HasLetters(this string str) => str.Any(char.IsLetter);
}
