using DA.Results.Issues;

namespace DA.Results;

/// <summary>
/// Wrapper around a Result{NoContent}.
/// Useful to make working with results without content easier.
/// </summary>
public record NoContentResult : Result<NoContent>
{
    /// <summary>
    /// Create a new success result without content.
    /// </summary>
    /// <param name="ignoreWarnings">In subsequent bindings and checks, should warnings be ignored?</param>
    internal NoContentResult(bool ignoreWarnings = false) : base(NoContent.Instance, ignoreWarnings) {}

    /// <summary>
    /// Create a new failure result without content
    /// </summary>
    /// <param name="issue">The issue that caused this result to be a failure</param>
    internal NoContentResult(Issue issue) : base(issue) {}
    
    public static implicit operator NoContentResult(Issue issue) => new(issue);
}


/// <summary>
/// A Result with only information if something happened and no (actual) value.
/// </summary>
public record NoContent
{
    private NoContent() { /* No new instances of the NoContent class can be made */ }
        
    /// <summary>
    /// The only instance of NoContent that can ever exist.
    /// </summary>
    internal static NoContent Instance => new();
}