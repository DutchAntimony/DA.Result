using System.Diagnostics.CodeAnalysis;
using DA.Results.Issues;

namespace DA.Results;

/// <summary>
/// Monad around a value.
/// The result monad can either be a success with a value, or a failure with an <see cref="Issue"/>
/// </summary>
/// <typeparam name="TValue">The type this monad is wrapping.</typeparam>
public record Result<TValue> : IResult
{
    private readonly TValue? _value;
    private readonly Issue? _issue;

    /// <summary> Internal constructor for a success result. </summary>
    internal Result(TValue value, bool ignoreWarnings = false) =>
        (IsSuccess, _value, IgnoreWarnings) = (true, value, ignoreWarnings);

    /// <summary> Internal constructor for a failure result. </summary>
    internal Result(Issue issue, bool ignoreWarnings = false) =>
        (IsSuccess, _issue, IgnoreWarnings) = (false, issue, ignoreWarnings);

    /// <inheritdoc />
    public bool IsSuccess { get; }

    /// <inheritdoc />
    public bool IgnoreWarnings { get; }

    /// <summary>
    /// Try to get the value of the result, as an out parameter.
    /// </summary>
    /// <param name="value">The value encapsulated by this result.</param>
    /// <returns>True if result is a success, false if not.</returns>
    public bool TryGetValue([NotNullWhen(true)] out TValue? value)
    {
        value = _value;
        return IsSuccess;
    }

    /// <summary>
    /// Try to get the value or the issue of the result, both as out parameter.
    /// </summary>
    /// <param name="value">The value encapsulated by this result.</param>
    /// <param name="issue">The issue that occured that caused this result to be a failure</param>
    /// <returns>True if result is a success, false if not.</returns>
    public bool TryGetValue(
        [MaybeNullWhen(false)] out TValue value,
        [MaybeNullWhen(true)] out Issue issue)
    {
        value = _value;
        issue = _issue;
        return IsSuccess;
    }

    /// <inheritdoc />
    public bool TryGetIssue([NotNullWhen(true)] out Issue? issue)
    {
        issue = _issue;
        return !IsSuccess;
    }
    
    /// <summary>
    /// Drop the value information from the result
    /// </summary>
    /// <returns>A new instance of a <see cref="NoContentResult"/></returns>
    public NoContentResult WithoutContent() => 
        TryGetIssue(out var issue) ? new NoContentResult(issue) : new NoContentResult(IgnoreWarnings);
    
    public static implicit operator Result<TValue>(Issue issue) => new(issue);
    public static implicit operator Result<TValue>(TValue value) => new(value);
    public static explicit operator NoContentResult(Result<TValue> result) => result.WithoutContent();
    
    /// <inheritdoc />
    public Type GetValueType() => typeof(TValue);
}