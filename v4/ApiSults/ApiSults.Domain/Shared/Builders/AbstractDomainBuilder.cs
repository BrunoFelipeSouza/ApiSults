namespace ApiSults.Domain.Shared.Builders;

public abstract class AbstractDomainBuilder<T>
    (T obj, IEnumerable<string> requiredFields) where T : class
{
    protected readonly HashSet<string> _requiredFields = new(requiredFields);
    protected T _object = obj;

    protected void MarkAsProvidedIf(bool condition, string field)
    {
        if (condition)
            MarkAsProvided(field);
    }

    protected void MarkAsProvided(string field) => _requiredFields.Remove(field);

    public virtual T Build()
    {
        VerifyCreation();
        return _object;
    }

    protected virtual void VerifyCreation()
    {
        if (_requiredFields.Count > 0)
            throw new InvalidOperationException($"The following properties were not provided: {string.Join(", ", _requiredFields)}");
    }

    public static implicit operator T(AbstractDomainBuilder<T> domainBuilder) => domainBuilder.Build();
}
