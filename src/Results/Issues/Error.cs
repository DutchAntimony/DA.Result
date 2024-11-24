namespace DA.Results.Issues;

public abstract record Error : Issue
{
    public sealed override bool IsWarning => false;
    public override int StatusCode => 400;
}