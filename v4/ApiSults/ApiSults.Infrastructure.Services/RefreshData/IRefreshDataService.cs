using ApiSults.Infrastructure.Services.RefreshData.Requests;
using ApiSults.Infrastructure.Services.RefreshData.Responses;

namespace ApiSults.Infrastructure.Services.RefreshData;

public interface IRefreshDataService
{
    Task<GetTicketsResponse> GetTicketsByLastChangeDateAsync(GetTicketsByLastChangeDateRequest request, CancellationToken cancellationToken);
}
