namespace DA.Results.Issues;

public sealed record ValidationError(List<ValidationFailure> Failures) : Error
{
    public Dictionary<string, IEnumerable<string>> GetFailuresGroupedByProperty() =>
        Failures.GroupBy(f => f.Property)
                 .ToDictionary(group => group.Key,
                               group => group.Select(f => f.Message));

    public IEnumerable<string> GetFailuresOfProperty(string property) =>
        Failures.Where(f => f.Property == property).Select(f => f.Message);

    public override string GetMessage() => string.Join("\n", Failures.Select(f => f.ToString()));

    public override string Title => "One or more validation errors occurred:";
    
    public ValidationError(string property, string message)
        : this([new ValidationFailure(property, message)]) { }
}
