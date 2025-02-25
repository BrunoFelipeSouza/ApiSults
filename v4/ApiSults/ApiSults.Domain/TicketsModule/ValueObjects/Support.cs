using ApiSults.Domain.Shared;

namespace ApiSults.Domain.TicketsModule.ValueObjects;

public class Support : ValueObject<Support>
{
    public Person Person { get; protected set; }
    public Department Department { get; protected set; }
    public bool PersonUnit { get; protected set; }

    protected Support() { }

    public Support(Person person, Department department, bool personUnit)
    {
        Person = person;
        Department = department;
        PersonUnit = personUnit;
    }
}
