using ApiSults.Application.Shared;
using ApiSults.Application.Shared.Extensions;
using ApiSults.Application.TicketModule.Shared;
using ApiSults.Domain.TicketsModule;
using ApiSults.Domain.TicketsModule.Repositories;
using CsvHelper;
using CsvHelper.Configuration;
using MediatR;
using System.Globalization;
using System.Text;

namespace ApiSults.Application.TicketModule.Queries.ExportTickets;

public class ExportTicketsRequestHandler(ITicketRepository ticketRepository) : IRequestHandler<ExportTicketsRequest, byte[]>
{
    private readonly ITicketRepository _ticketRepository = ticketRepository;

    public async Task<byte[]> Handle(ExportTicketsRequest request, CancellationToken cancellationToken)
    {
        var specs = new TicketSpecsBuilder()
            .WithRequest(request)
            .Build();

        var tickets = await _ticketRepository.GetTicketsBySpecificationAsync(specs, cancellationToken);

        if (tickets == null || !tickets.Any())
            return [];

        return await CreateFileAsync(tickets, cancellationToken);
    }

    private static async Task<byte[]> CreateFileAsync(IEnumerable<Ticket> tickets, CancellationToken cancellationToken)
    {
        using var memoryStream = new MemoryStream();
        using var writer = new StreamWriter(memoryStream, Encoding.UTF8);
        using var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = ";"
        });

        await WriteHeadersAsync(csv);
        await WriteRecordsAsync(csv, tickets);

        await writer.FlushAsync(cancellationToken);
        return memoryStream.ToArray();
    }

    private static async Task WriteHeadersAsync(CsvWriter csv)
    {
        csv.WriteField("id");
        csv.WriteField("titulo");
        csv.WriteField("solicitante_id");
        csv.WriteField("solicitante_nome");
        csv.WriteField("responsavel_id");
        csv.WriteField("responsavel_nome");
        csv.WriteField("unidade_id");
        csv.WriteField("unidade_nome");
        csv.WriteField("assunto_id");
        csv.WriteField("assunto_nome");
        csv.WriteField("etiqueta_id");
        csv.WriteField("etiqueta_nome");
        csv.WriteField("etiqueta_cor");
        csv.WriteField("apoio_pessoa_id");
        csv.WriteField("apoio_pessoa_nome");
        csv.WriteField("apoio_departamento_id");
        csv.WriteField("apoio_departamento_nome");
        csv.WriteField("apoio_possui_unidade");
        csv.WriteField("tipo_id");
        csv.WriteField("tipo_descricao");
        csv.WriteField("aberto");
        csv.WriteField("resolvido");
        csv.WriteField("concluido");
        csv.WriteField("resolver_planejado");
        csv.WriteField("resolver_estipulado");
        csv.WriteField("avaliacao_nota");
        csv.WriteField("avaliacao_observacao");
        csv.WriteField("situacao_id");
        csv.WriteField("situacao_nome");
        csv.WriteField("primeira_interacao");
        csv.WriteField("ultima_interacao");
        csv.WriteField("count_interacao_publico");
        csv.WriteField("count_interacao_interno");

        await csv.NextRecordAsync();
    }

    public static async Task WriteRecordsAsync(CsvWriter csv, IEnumerable<Ticket> tickets)
    {
        var allRecords = tickets
         .SelectMany(ticket =>
             ticket.Tags.DefaultIfEmpty().SelectMany(etiqueta =>
                 ticket.Supports.DefaultIfEmpty().Select(apoio => new
                 {
                     Ticket = ticket,
                     Etiqueta = etiqueta,
                     Apoio = apoio
                 })
             )
         )
         .ToList();

        foreach (var record in allRecords)
        {
            var ticket = record.Ticket;
            var etiqueta = record.Etiqueta;
            var apoio = record.Apoio;

            csv.WriteField(ticket.Id);
            csv.WriteField(ticket.Title);
            csv.WriteField(ticket.Applicant.Id);
            csv.WriteField(ticket.Applicant.Name);
            csv.WriteField(ticket.Responsible.Id);
            csv.WriteField(ticket.Responsible.Name);
            csv.WriteField(ticket.Unit.Id);
            csv.WriteField(ticket.Unit.Name);
            csv.WriteField(ticket.Subject.Id);
            csv.WriteField(ticket.Subject.Name);
            csv.WriteField(etiqueta?.Id);
            csv.WriteField(etiqueta?.Name);
            csv.WriteField(etiqueta?.Color);
            csv.WriteField(apoio?.Person.Id);
            csv.WriteField(apoio?.Person.Name);
            csv.WriteField(apoio?.Department.Id);
            csv.WriteField(apoio?.Department.Name);
            csv.WriteField(apoio?.PersonUnit);
            csv.WriteField(ticket.Type);
            csv.WriteField(((TicketType)ticket.Type).GetDescription());
            csv.WriteField(ticket.Open);
            csv.WriteField(ticket.Resolved);
            csv.WriteField(ticket.Completed);
            csv.WriteField(ticket.SolvePlanned);
            csv.WriteField(ticket.ResolveStipulated);
            csv.WriteField(ticket.EvaluationNote);
            csv.WriteField(ticket.EvaluationObservation);
            csv.WriteField(ticket.Status);
            csv.WriteField(((TicketStatus)ticket.Status).GetDescription());
            csv.WriteField(ticket.FirstInteraction);
            csv.WriteField(ticket.LastChange);
            csv.WriteField(ticket.CountPublicInteraction);
            csv.WriteField(ticket.CountInteractionInternal);

            await csv.NextRecordAsync();
        }
    }
}