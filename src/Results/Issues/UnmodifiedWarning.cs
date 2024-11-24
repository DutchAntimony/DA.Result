namespace DA.Results.Issues;

public sealed record UnmodifiedWarning(IEnumerable<Type> Types) : Warning
{
    public UnmodifiedWarning(Type type) : this([type]) { }
    public override string Title => "Actie heeft geen wijzigingen opgeleverd";
    public override string GetMessage() => string.Join("\n", Types.Select(t => t.Name));
}