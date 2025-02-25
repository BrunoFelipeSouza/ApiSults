namespace ApiSults.Infrastructure.Services.RefreshData.Requests;

public sealed record class GetTicketsByLastChangeDateRequest(
    DateTime UltimaAlteracaoStart,
    string Key,
    int Limit = 100)
{
    public int Start { get; private set; } = 0;

    public void NextPage()
    {
        Start += 1;
    }
}
