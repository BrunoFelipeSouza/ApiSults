namespace ApiSults.Domain.Shared;

public abstract class Entity
{
    public long Id { get; set; }

    public override bool Equals(object? obj)
        => obj is Entity other && Id.Equals(other.Id);

    public override int GetHashCode()
        => Id.GetHashCode();
}
