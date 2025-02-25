using System.Reflection;

namespace ApiSults.Domain.Shared;

public abstract class ValueObject<T> : IEquatable<T>
  where T : ValueObject<T>
{
    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (obj is not T other) return false;

        return Equals(other);
    }

    public override int GetHashCode()
    {
        var fields = GetFields(this);

        var startValue = 17;
        var multiplier = 59;

        return fields
            .Select(field => field.GetValue(this))
            .Where(value => value != null)
            .Aggregate(
                startValue,
                    (current, value) => current * multiplier + value?.GetHashCode() ?? 0);
    }

    public virtual bool Equals(T? other)
    {
        if (other is null)
            return false;

        var t = GetType();
        var otherType = other.GetType();

        if (t != otherType)
            return false;

        var fields = GetFields(this);

        foreach (var field in fields)
        {
            var value1 = field.GetValue(other);
            var value2 = field.GetValue(this);

            if (value1 is null)
            {
                if (value2 != null)
                    return false;
            }
            else if (!value1.Equals(value2))
                return false;
        }

        return true;
    }

    private static List<FieldInfo> GetFields(object obj)
    {
        var t = obj.GetType();

        var fields = new List<FieldInfo>();

        while (t != typeof(object))
        {
            if (t is null) continue;
            fields.AddRange(t.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public));

            t = t.BaseType;
        }

        return fields;
    }

    public static bool operator ==(ValueObject<T> x, ValueObject<T> y)
    {
        if (x is null && y is null) return true;

        if (x is null || y is null) return false;

        return x.Equals(y);
    }

    public static bool operator !=(ValueObject<T> x, ValueObject<T> y) => !(x == y);
}