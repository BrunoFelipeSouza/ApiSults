using ApiSults.Infrastructure.Services.RefreshData.Requests;
using ApiSults.Infrastructure.Services.RefreshData.Responses;
using ApiSults.Infrastructure.Services.Shared;
using System.Text;

namespace ApiSults.Infrastructure.Services.RefreshData;

public class RefreshDataService(IHttpClientFactory httpClientFactory) : ApiHttpClient(httpClientFactory, Source), IRefreshDataService
{
    private const string Source = "RefreshDataService";

    public async Task<GetTicketsResponse> GetTicketsByLastChangeDateAsync(GetTicketsByLastChangeDateRequest request, CancellationToken cancellationToken)
    {
        var queryParams = new StringBuilder();
        queryParams.Append($"?start={request.Start}");
        queryParams.Append($"&limit={request.Limit}");
        queryParams.Append($"&ultimaAlteracaoStart={request.UltimaAlteracaoStart.ToString("yyyy-MM-ddTHH:mm:ssZ")}");
        var uri = new Uri(RefreshDataEndPoints.UrlBase + queryParams);
        var requestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
        requestMessage.Headers.TryAddWithoutValidation("Authorization", request.Key);

        var response = await _httpClient.SendAsync(requestMessage, cancellationToken);

        response.EnsureSuccessStatusCode();

        var content = await DeserializarAsync<GetTicketsResponse>(response)
        ?? throw new HttpRequestException("Erro ao deserializar a resposta da API.");

        return content;
    }
}