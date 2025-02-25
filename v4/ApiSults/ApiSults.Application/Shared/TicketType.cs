using System.ComponentModel;

namespace ApiSults.Application.Shared;

public enum TicketType
{
    [Description("Nenhum")]
    Null = 0,
    [Description("Chamado de administrador para administrador")]
    TicketFromAdministratorToAdministrator = 1,
    [Description("Chamado de unidade para administrador")]
    TicketFromUnitToAdministrator = 2,
    [Description("Chamado de administrador para unidade")]
    TicketFromAdministratorToUnit = 3,
}