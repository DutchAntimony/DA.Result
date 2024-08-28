namespace DA.Results.ResultTypes.Failure;

/// <summary>
/// Base ResultType that is not successful, a failure.
/// </summary>
public record FailureResultType : IFailureResultType
{
    /// <inheritdoc/>
    public bool IsSuccess => false;

    /// <inheritdoc/>
    public int Weight => 1;

    /// <inheritdoc/>
    public IResultType Combine(IResultType resultType) => resultType is IFailureResultType ? resultType : this;
}
