namespace DA.Results.Issues;

public abstract record Warning : Issue
{
    public sealed override bool IsWarning => true;
    public sealed override int StatusCode => 400;
}
