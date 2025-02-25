using ApiSults.Domain.Shared.Repositories;
using ApiSults.Infrastructure.Jobs.Shared;
using ApiSults.Infrastructure.Services.RefreshData.Requests;
using ApiSults.Infrastructure.Services.RefreshData.Responses;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text;

namespace ApiSults.Infrastructure.Jobs;

public class AutoRefreshDataJob(IServiceScopeFactory serviceScopeFactory) : IHostedService, IDisposable
{
    private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private StringBuilder _source;
    private Timer? _timer;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(_ => Task.Run(() => DoWork(cancellationToken)), null, TimeSpan.Zero, TimeSpan.FromSeconds(30));
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
        _semaphore?.Dispose();
        GC.SuppressFinalize(this);
    }

    private async Task DoWork(CancellationToken cancellationToken)
    {
        await _semaphore.WaitAsync(cancellationToken);

        try
        {
            _source = new StringBuilder().Append("Job Auto Refresh Data: " + StringExtensions.NewRandomNumber());
            using var scope = _serviceScopeFactory.CreateScope();
            var services = scope.ServiceProvider.GetRequiredService<IApplicationServices>();
            
            await StartRefreshDataAsync(services, cancellationToken);
        }
        finally
        {
            _semaphore.Release();
        }
    }

    private async Task StartRefreshDataAsync(IApplicationServices services, CancellationToken cancellationToken)
    {
        try
        {
            await services.LoggingService.LogInformation(_source.ToString(), "Auto Refresh Data Started.");
            await RefreshDataAsync(services, cancellationToken);
            await services.UnitOfWork.CommitAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            await services.LoggingService.LogError(_source.ToString(), ex.Message);
        }
        finally
        {
            await services.LoggingService.LogInformation(_source.ToString(), "Auto Refresh Data Finalized.");
        }
    }

    private async Task RefreshDataAsync(IApplicationServices services, CancellationToken cancellationToken)
    {
        var configuration = await services.ConfigurationRepository.GetConfigurationAsync(cancellationToken)
        ?? throw new InvalidOperationException("Configuration not found.");

        var key = configuration.Key;

        if (string.IsNullOrWhiteSpace(key))
        {
            await services.LoggingService.LogInformation(_source.ToString(), "Missing Api Key.");
            return;
        }

        DateTime lastAtualization = configuration.LastAtualization;
        DateTime nextAtualization = lastAtualization.AddMinutes(configuration.AutomaticAtualizationIntervalInMinutes);

        if (DateTime.Now < nextAtualization)
        {
            await services.LoggingService.LogInformation(_source.ToString(), "Awaiting time to refresh data.");
            return;
        }

        var tickets = await GetTicketsAsync(services, lastAtualization, key, cancellationToken);
        await UpdateDataAsync(services, tickets, cancellationToken);
        configuration.Atualize();
        services.ConfigurationRepository.Update(configuration);
        await services.UnitOfWork.CommitAsync(cancellationToken);
    }

    private async Task<List<Domain.TicketsModule.Ticket>> GetTicketsAsync(
        IApplicationServices services,
        DateTime lastAtualization,
        string key,
        CancellationToken cancellationToken)
    {
        var tickets = new List<Ticket>();
        var request = new GetTicketsByLastChangeDateRequest(lastAtualization, key);
        var responseTest = await services.RefreshDataService.GetTicketsByLastChangeDateAsync(request, cancellationToken);
        await services.LoggingService.LogInformation(_source.ToString(), $"Getting tickets from API. Page {request.Start + 1} of {responseTest.TotalPage}");

        while (true)
        {
            var response = await services.RefreshDataService.GetTicketsByLastChangeDateAsync(request, cancellationToken);

            if(response == null || response.Data == null || !response.Data.Any())
            {
                break;
            }

            tickets.AddRange(response.Data);

            if (response.Start >= response.TotalPage) break;

            request.NextPage();
            await services.LoggingService.LogInformation(_source.ToString(), $"Getting tickets from API. Page {request.Start + 1} of {response.TotalPage}");
        }

        return MapToTicketsDomain(tickets);
    }

    private async Task UpdateDataAsync(IApplicationServices services, List<Domain.TicketsModule.Ticket> tickets, CancellationToken cancellationToken)
    {
        await services.LoggingService.LogInformation(_source.ToString(), "Updating data.");

        foreach (var ticket in tickets)
        {
            var spec = new Specification<Domain.TicketsModule.Ticket>().IsEqual(x => x.Id, ticket.Id);
            var ticketEntity = await services.TicketRepository.GetTicketBySpecificationAsync(spec, cancellationToken);

            if (ticketEntity == null)
                await services.TicketRepository.AddAsync(ticket);
            else
            {
                ticketEntity.Update(ticket);
                services.TicketRepository.Update(ticket);
            }
        }

        try
        {
            await services.UnitOfWork.CommitAsync(cancellationToken);
            await services.LoggingService.LogInformation(_source.ToString(), "Data updated.");
        }
        catch (Exception ex)
        {
            await services.LoggingService.LogError(_source.ToString(), $"Error during commit: {ex.Message}");
            throw;
        }
    }   

    private static List<Domain.TicketsModule.Ticket> MapToTicketsDomain(IEnumerable<Ticket> tickets)
    {
        var ticketsDomain = new List<Domain.TicketsModule.Ticket>();

        foreach (var ticket in tickets)
        {
            var applicant = new Domain.TicketsModule.Applicant(ticket.Solicitante.Id ?? 0, ticket.Solicitante.Nome ?? string.Empty);
            var responsible = new Domain.TicketsModule.Responsible(ticket.Responsavel.Id ?? 0, ticket.Responsavel.Nome ?? string.Empty);
            var unit = new Domain.TicketsModule.Unit(ticket.Unidade.Id ?? 0, ticket.Unidade.Nome ?? string.Empty);
            var department = new Domain.TicketsModule.Department(ticket.Departamento.Id ?? 0, ticket.Departamento.Nome ?? string.Empty);
            var subject = new Domain.TicketsModule.Subject(ticket.Assunto.Id ?? 0, ticket.Assunto.Nome ?? string.Empty);
            var supports = ticket.Apoio?
                .Select(apoio => {
                    var person = new Domain.TicketsModule.Person(apoio.Pessapoiooa?.Id ?? 0, apoio.Pessapoiooa?.Nome ?? string.Empty);
                    var department = new Domain.TicketsModule.Department(apoio.Departamento?.Id ?? 0, apoio.Departamento?.Nome ?? string.Empty);
                    var personUnit = apoio.PessoaUnidade;
                    return new Domain.TicketsModule.ValueObjects.Support(person, department, personUnit);
                }).ToList();
            var tags = ticket.Etiqueta
                .Select(etiqueta => new Domain.TicketsModule.ValueObjects.Tag(etiqueta.Id, etiqueta.Nome, etiqueta.Cor))
                .ToList();

            var ticketDomain = Domain.TicketsModule.Ticket.Create()
                .WithId(ticket.Id)
                .WithTitle(ticket.Titulo)
                .WithApplicant(applicant)
                .WithResponsible(responsible)
                .WithUnit(unit)
                .WithDepartment(department)
                .WithSubject(subject)
                .WithSupports(supports)
                .WithTags(tags)
                .WithType(ticket.Tipo)
                .WithOpen(ticket.Aberto)
                .WithResolved(ticket.Resolvido)
                .WithCompleted(ticket.Concluido)
                .WithSolvePlanned(ticket.ResolverPlanejado)
                .WithResolveStipulated(ticket.ResolverEstipulado)
                .WithFirstInteraction(ticket.PrimeiraInteracao)
                .WithLastChange(ticket.UltimaAlteracao)
                .WithEvaluationNote(ticket.AvaliacaoNota)
                .WithEvaluationObservation(ticket.AvaliacaoObservacao)
                .WithStatus(ticket.Situacao)
                .WithCountPublicInteraction(ticket.CountInteracaoPublico)
                .WithCountInteractionInternal(ticket.CountInteracaoInterno)
                .Build();

            ticketsDomain.Add(ticketDomain);
        }

        return ticketsDomain;
    }
}
