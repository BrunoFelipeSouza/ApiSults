using ApiSults.Domain.Shared;

namespace ApiSults.Domain.TicketsModule.ValueObjects;

public class Tag : ValueObject<Tag>
{
    public long Id { get; protected set; }
    public string Name { get; protected set; }
    public string Color { get; protected set; }

    protected Tag() { }

    public Tag(long id, string name, string color)
    {
        Id = id;
        Name = name;
        Color = color;
    }
}
