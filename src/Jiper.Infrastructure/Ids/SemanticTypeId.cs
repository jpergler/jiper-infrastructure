namespace Jiper.Infrastructure.Ids;

[Serializable]
public abstract record SemanticTypeId
{
    protected abstract string DocumentPrefix { get; }

    private const char Divider = '/';
    public string Value { get; private set; }
    public string Prefix { get; private set; }
    public string ShortId { get; private set; }

    protected SemanticTypeId(string value)
    {
        var dividerCount = value.Count(x => x == Divider);
        if (dividerCount is not (1 or 0) || string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException($"Semantic type id can contain only one divider but contains {dividerCount}",
                value);
        }

        if (dividerCount is 0)
        {
            Prefix = DocumentPrefix;
            ShortId = value;
            Value = Prefix + Divider + ShortId;
        }
        else
        {
            var parts = value.Split('/');
            var prefix = parts[0];
            if (prefix != DocumentPrefix)
            {
                throw new ArgumentException(
                    $"Prefix '{prefix}' does not match the document prefix '{DocumentPrefix}' specified in ID class",
                    value);
            }

            Value = value;
            Prefix = prefix;
            ShortId = parts[1];
        }
    }

    public override string ToString()
    {
        return Value;
    }

    public virtual bool Equals(SemanticTypeId? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Value == other.Value;
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
}