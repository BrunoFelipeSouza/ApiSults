using ApiSults.Domain.ConfigurationModule.Repositories;
using ApiSults.Domain.Shared.Repositories;
using ApiSults.Domain.TicketsModule.Repositories;
using ApiSults.Infrastructure.Services.Logging;
using ApiSults.Infrastructure.Services.RefreshData;

namespace ApiSults.Infrastructure.Jobs.Shared;

public interface IApplicationServices
{
    ILoggingService LoggingService { get; }
    ILogRepository LogRepository { get; }
    IConfigurationRepository ConfigurationRepository { get; }
    ITicketRepository TicketRepository { get; }
    IRefreshDataService RefreshDataService { get; }
    IUnitOfWork UnitOfWork { get; }
}
