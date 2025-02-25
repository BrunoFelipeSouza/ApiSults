using ApiSults.Domain.Shared;

namespace ApiSults.Domain.TicketsModule;

public class Department : Entity
{
    public string Name { get; protected set; }

    protected Department() { }

    public Department(long id, string name)
    {
        Id = id;
        Name = name;
    }
}