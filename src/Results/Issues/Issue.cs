namespace DA.Results.Issues;

public abstract record Issue
{
    public abstract string Title { get; }
    public abstract int StatusCode { get; }

    public abstract bool IsWarning { get; } 

    public abstract string GetMessage();
}