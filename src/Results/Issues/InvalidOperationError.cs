namespace DA.Results.Issues;

public sealed record InvalidOperationError(string Message) : Error
{
    public override string Title => "Something was wrong with the request:";
    public override string GetMessage() => Message;
}

