namespace ApiSults.Domain.Shared.Entities;

public class Log : Entity
{
    public string Message { get; protected set; }
    public DateTime Date { get; protected set; }
    public string Level { get; protected set; }
    public string Source { get; protected set; }

    protected Log() { }

    public Log(string message, string level, string source)
    {
        Message = message;
        Date = DateTime.Now;
        Level = level;
        Source = source;
    }
}
