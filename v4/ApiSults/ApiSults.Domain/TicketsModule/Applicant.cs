using ApiSults.Domain.Shared;

namespace ApiSults.Domain.TicketsModule;

public class Applicant : Entity
{
    public string Name { get; protected set; }

    protected Applicant() { }

    public Applicant(long id, string name)
    {
        Id = id;
        Name = name;
    }
}
