using ApiSults.Domain.ConfigurationModule.Repositories;
using ApiSults.Domain.Shared.Repositories;
using ApiSults.Domain.TicketsModule.Repositories;
using ApiSults.Infrastructure.Services.Logging;
using ApiSults.Infrastructure.Services.RefreshData;

namespace ApiSults.Infrastructure.Jobs.Shared;

public class ApplicationServices(
    ILoggingService loggingService,
    ILogRepository logRepository,
    IConfigurationRepository configurationRepository,
    ITicketRepository ticketRepository,
    IRefreshDataService refreshDataService,
    IUnitOfWork unitOfWork) : IApplicationServices
{
    public ILoggingService LoggingService => loggingService;

    public ILogRepository LogRepository => logRepository;

    public IConfigurationRepository ConfigurationRepository => configurationRepository;

    public ITicketRepository TicketRepository => ticketRepository;

    public IRefreshDataService RefreshDataService => refreshDataService;

    public IUnitOfWork UnitOfWork => unitOfWork;
}
