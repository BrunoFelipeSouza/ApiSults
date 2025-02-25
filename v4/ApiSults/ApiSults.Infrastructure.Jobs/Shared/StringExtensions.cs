namespace ApiSults.Infrastructure.Jobs.Shared;

public static class StringExtensions
{
    public static string NewRandomNumber()
        => string
            .Concat(Guid
            .NewGuid()
            .ToString()
            .Where(char.IsLetterOrDigit))[..12]
            .PadLeft(12, '0');
}
