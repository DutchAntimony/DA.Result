namespace DA.Results.ResultTypes;

/// <summary>
/// Interface for any result type, either a success or a failure.
/// </summary>
public interface IResultType
{
    /// <summary>
    /// Boolean indicating if the resulttype is for a success or for a failure.
    /// </summary>
    bool IsSuccess { get; }

    IResultType Combine(IResultType resultType);
}
