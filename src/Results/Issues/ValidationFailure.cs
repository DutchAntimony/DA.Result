namespace DA.Results.Issues;

public sealed record ValidationFailure(string Property, string Message)
{
    public override string ToString() => $"{Property}: {Message}";
}

