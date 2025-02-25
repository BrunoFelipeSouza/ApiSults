using ApiSults.Domain.Shared.Entities;
using ApiSults.Domain.Shared.Repositories;
using System.Diagnostics;

namespace ApiSults.Infrastructure.Services.Logging;

public class LoggingService(ILogRepository logRepositoriy, IUnitOfWork unitOfWork) : ILoggingService
{
    private const string LevelInformation = "INFO";
    private const string LevelError = "ERROR";
    private const string LevelWarning = "WARNING";

    private readonly ILogRepository _logRepositoriy = logRepositoriy;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task LogError(string source, string message)
    {
        await LogAsync(source, message, LevelError);
        Debug.WriteLine($"[{LevelError}] {DateTime.Now:dd/MM/yyyy HH:mm:ss} - {message}");
    }

    public async Task LogInformation(string source, string message)
    {
        await LogAsync(source, message, LevelInformation);
        Debug.WriteLine($"[{LevelInformation}] {DateTime.Now:dd/MM/yyyy HH:mm:ss} - {message}");
    }

    public async Task LogWarning(string source, string message)
    {
        await LogAsync(source, message, LevelWarning);
        Debug.WriteLine($"[{LevelWarning}] {DateTime.Now:dd/MM/yyyy HH:mm:ss} - {message}");
    }

    private async Task LogAsync(string source, string message, string level)
    {
        var log = new Log(message, level, source);
        await _logRepositoriy.AddAsync(log);
        await _unitOfWork.CommitAsync(CancellationToken.None);
    }
}
