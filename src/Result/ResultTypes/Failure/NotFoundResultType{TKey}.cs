namespace DA.Results.ResultTypes.Failure;

/// <summary>
/// Failure result that contains information about which data is not found in a db.
/// </summary>
/// <typeparam name="TKey">The type of the key that did not return any values.</typeparam>
public record NotFoundResultType<TKey> : IFailureResultType
    where TKey : notnull
{
    /// <summary>
    /// The identifier that did not return any results.
    /// </summary>
    public required TKey Id { get; init; }

    /// <inheritdoc/>
    public bool IsSuccess => false;

    /// <inheritdoc/>
    public int Weight => 4;

    /// <inheritdoc/>
    public IResultType Combine(IResultType resultType) =>
        resultType is IFailureResultType failure && failure.Weight > Weight ? resultType : this;
}