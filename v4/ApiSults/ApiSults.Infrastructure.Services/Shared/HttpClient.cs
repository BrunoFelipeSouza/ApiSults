using System.Net;
using System.Net.Http.Json;

namespace ApiSults.Infrastructure.Services.Shared;

public class ApiHttpClient
{
    protected readonly HttpClient _httpClient;
    protected readonly string _nomeHttpClient;

    public ApiHttpClient(IHttpClientFactory httpClientFactory, string nomeHttpClient)
    {
        ArgumentNullException.ThrowIfNull(httpClientFactory);
        ArgumentNullException.ThrowIfNull(nomeHttpClient);

        _nomeHttpClient = nomeHttpClient;

        _httpClient = httpClientFactory.CreateClient(_nomeHttpClient);
    }

    public static async Task<T?> DeserializarAsync<T>(HttpResponseMessage response)
    {
        if (response.StatusCode == HttpStatusCode.NoContent)
            return default;

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadFromJsonAsync<T>();
            return content
            ?? throw new HttpRequestException("A resposta foi bem-sucedida, mas ocorreu um erro ao deserializar o conteúdo para o tipo esperado.");
        }

        throw new HttpRequestException($"A resposta falhou com o StatusCode: {response.StatusCode}. Não foi possível deserializar o conteúdo.");
    }
}