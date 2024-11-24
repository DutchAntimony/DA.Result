namespace DA.Results.Issues;

public sealed record ConfirmationRequiredWarning(string Message) : Warning
{
    public override string GetMessage() => Message;
    public override string Title => "Actie moet bevestigd worden";
}
