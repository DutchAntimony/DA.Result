using System.Reflection.Metadata.Ecma335;

namespace DA.Results.ResultTypes.Success;

/// <summary>
/// ResultType that is successful.
/// </summary>
public record SuccessResultType : ISuccessResultType
{
    /// <inheritDoc />
    public bool IsSuccess => true;

    /// <inheritdoc/>
    public IResultType Combine(IResultType resultType) => resultType;
}
