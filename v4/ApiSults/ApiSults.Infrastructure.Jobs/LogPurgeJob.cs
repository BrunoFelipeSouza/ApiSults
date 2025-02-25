using ApiSults.Domain.Shared.Entities;
using ApiSults.Domain.Shared.Repositories;
using ApiSults.Infrastructure.Jobs.Shared;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text;

namespace ApiSults.Infrastructure.Jobs;

public class LogPurgeJob(IServiceScopeFactory serviceScopeFactory) : IHostedService, IDisposable
{
    private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;
    private StringBuilder _source;
    private Timer? _timer;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(_ => Task.Run(() => DoWork(cancellationToken)), null, TimeSpan.Zero, TimeSpan.FromHours(2));
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }

    private async Task DoWork(CancellationToken cancellationToken)
    {
        _source = new StringBuilder().Append("Job Purge Log: " + StringExtensions.NewRandomNumber());
        using var scope = _serviceScopeFactory.CreateScope();
        var services = scope.ServiceProvider.GetRequiredService<IApplicationServices>();

        try
        {
            await services.LoggingService.LogInformation(_source.ToString(), "Log Purge started.");

            var specification = new Specification<Log>()
                                        .IsLessThanOrEqual(x => x.Date, DateTime.Now.AddDays(-7))
                                        .IsEqual(x => x.Level, "INFO");

            var logs = await services.LogRepository.GetLogsBySpecification(specification, cancellationToken);

            var logsCount = logs?.Count() ?? 0;

            await services.LoggingService.LogInformation(_source.ToString(), $"Logs found: {logsCount}");

            if (logs != null)
            {
                services.LogRepository.RemoveRange(logs);
                await services.UnitOfWork.CommitAsync(cancellationToken);
            }
        }
        catch
        {
            await services.LoggingService.LogError(_source.ToString(), "Error occurred while purging logs.");
        }
        finally
        {
            await services.LoggingService.LogInformation(_source.ToString(), "Log Purge completed.");
        }
    }
}
