using System.Diagnostics.CodeAnalysis;
using DA.Results.Issues;

namespace DA.Results;

public interface IResult
{
    /// <summary>
    /// Is the result a success?
    /// </summary>
    bool IsSuccess { get; }
    
    /// <summary>
    /// Should warnings be ignored?
    /// </summary>
    public bool IgnoreWarnings { get; }
    
    /// <summary>
    /// Try to get the issue of the result, as an out parameter.
    /// </summary>
    /// <param name="issue">The issue that causes this result to fail.</param>
    /// <returns>True if result is a failure, false if not.</returns>
    bool TryGetIssue([NotNullWhen(true)] out Issue? issue);

    /// <summary>
    /// Get the type of the value that is encapsulated in this Result object.
    /// </summary>
    /// <returns>A Type, even when the result is a failure, and or has the <see cref="Result.NoContent"/> type.</returns>
    Type GetValueType();
}