namespace DA.Results.ResultTypes.Failure;

/// <summary>
/// Failure result type that wraps an exception.
/// </summary>
public record ExceptionResultType : IFailureResultType
{
    /// <summary>
    /// Wrapping the exception that occured with causes this exception result.
    /// </summary>
    public required Exception Exception { get; init; }

    /// <inheritdoc/>
    public bool IsSuccess => false;

    /// <inheritdoc/>
    public int Weight => 16;

    /// <inheritdoc/>
    public IResultType Combine(IResultType resultType) => this;
}
