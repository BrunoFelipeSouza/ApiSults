using ApiSults.Domain.Shared;

namespace ApiSults.Domain.TicketsModule;

public class Responsible : Entity
{
    public string Name { get; protected set; }

    protected Responsible() { }

    public Responsible(long id, string name)
    {
        Id = id;
        Name = name;
    }
}