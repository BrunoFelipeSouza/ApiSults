using ApiSults.Domain.Shared;

namespace ApiSults.Domain.TicketsModule;

public class Subject : Entity 
{
    public string Name { get; protected set; }

    protected Subject() { }

    public Subject(long id, string name)
    {
        Id = id;
        Name = name;
    }
}