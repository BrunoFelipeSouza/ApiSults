using ApiSults.Domain.Shared;

namespace ApiSults.Domain.TicketsModule;

public class Person : Entity
{
    public string Name { get; protected set; }

    protected Person() { }

    public Person(long id, string name)
    {
        Id = id;
        Name = name;
    }
}
