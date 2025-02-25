using System.ComponentModel;

namespace ApiSults.Application.Shared;

public enum TicketStatus
{
    [Description("Nenhum")]
    Null = 0,
    [Description("Novo Chamado")]
    NewTicket = 1,
    [Description("Concluído")]
    Completed = 2,
    [Description("Resolvido")]
    Resolved = 3,
    [Description("Em Andamento")]
    InProgress = 4,
    [Description("Aguardando Solicitante")]
    WaitingForRequester = 5,
    [Description("Aguardando Responsável")]
    WaitingForResponsible = 6,
}