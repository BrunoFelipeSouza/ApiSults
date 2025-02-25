namespace ApiSults.Presentation.Api.Shared;

public class ApiKeyOptions
{
    public const string Secao = "ApiKeyOptions";
    public required string Key { get; set; }
    public required string HeaderName { get; set; }
}
