namespace DA.Results.Issues;

public sealed record InternalServerError(IEnumerable<Exception> Exceptions) : Error
{
    public InternalServerError(Exception exception) : this([exception]) {}
    
    public override string GetMessage() => string.Join("\n", Exceptions.Select(e => e.Message));
    public override string Title => "One or more errors occurred:";
    public override int StatusCode => 500;
    
    public static implicit operator InternalServerError(Exception ex) => new(ex);
}

