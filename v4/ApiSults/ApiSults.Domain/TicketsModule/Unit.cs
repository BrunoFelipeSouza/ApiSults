using ApiSults.Domain.Shared;

namespace ApiSults.Domain.TicketsModule;

public class Unit : Entity 
{
    public string Name { get; protected set; }

    protected Unit() { }

    public Unit(long id, string name)
    {
        Id = id;
        Name = name;
    }
}