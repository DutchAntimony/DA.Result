using DA.Results.ResultTypes;
using DA.Results.ResultTypes.Failure;
using DA.Results.ResultTypes.Success;
using System.Diagnostics;

namespace DA.Results;

/// <summary>
/// A result without a value attached. 
/// </summary>
/// <remarks> This class is <see langword="partial"/> such that factory methods for results can be in a different file.</remarks>
public partial record Result
{
    internal Result(IResultType metadata, string? message = null)
    {
        ResultType = metadata;
        if (message != null)
        {
            MetaData.Add(message);
        }
    }

    /// <inheritdoc />
    public IResultType ResultType { get; protected init; }

    /// <inheritdoc />
    public List<string> MetaData { get; internal init; } = [];

    /// <inheritdoc />
    public IEnumerable<string> Messages => MetaData.AsReadOnly();

    /// <summary>
    /// Boolean indicating if the result was successful or not.
    /// </summary>
    public bool IsSuccess => ResultType.IsSuccess;

    /// <summary>
    /// Bind this result to another result.
    /// </summary>
    /// <param name="result">The incoming result.</param>
    /// <returns>
    /// The original result, with the messages combined and resultType updated.
    /// - If the incoming result had a heigher priority resulttype, the resulttype of the other is taken.
    /// - If the incoming result is the same as our type, the data of the resulttype is combined.
    /// - If the incoming result is of a lower priority, our resulttype is preserved.
    /// </returns>
    public Result Bind(Result result)
    {
        if (!IsSuccess)
        {
            return this;
        }

        return this with
        {
            ResultType = ResultType.Combine(result.ResultType),
            MetaData = [.. MetaData, .. result.MetaData]
        };
    }

    /// <summary>
    /// Bind this result to another result with a value.
    /// </summary>
    /// <typeparam name="TValue">The type of the value of the other result.</typeparam>
    /// <param name="result">The incoming result.</param>
    /// <returns>
    /// The incoming result (because that determines the type) with
    /// Messages combined with ours
    /// A resulttype that depends on the priority of ours and that of the incoming:
    /// - If the incoming result had a heigher priority resulttype, the resulttype of the other is taken.
    /// - If the incoming result is the same as our type, the data of the resulttype is combined.
    /// - If the incoming result is of a lower priority, our resulttype is preserved.
    /// </returns>
    public Result<TValue> Bind<TValue>(Result<TValue> result)
    {
        var resultType = ResultType.Combine(result.ResultType);
        var metadata = MetaData.Concat(result.MetaData).ToList();
        return result with
        {
            ResultType = resultType,
            MetaData = metadata
        };
    }

    /// <summary>
    /// Map this result to a result of a different value type.
    /// </summary>
    /// <typeparam name="TValue">The type of the new value.</typeparam>
    /// <param name="value">The actual value that must be used.</param>
    /// <returns>
    /// A new result of the appropriate value type.
    /// The resulttype of the new result will be the same as ours.
    /// </returns>
    public Result<TValue> Map<TValue>(TValue value)
    {
        return ResultType switch
        {
            IFailureResultType failure => new Result<TValue>(failure) { MetaData = MetaData },
            ISuccessResultType success => new Result<TValue>(value, success) { MetaData = MetaData },
            _ => Exception<TValue>(new UnreachableException($"Discriminated union is broken by introducing a {ResultType.GetType().Name}"))
        };
    }

    /// <summary>
    /// Map this result to a result of a different value type without a value.
    /// This should only be done when we know the result is a failure.
    /// </summary>
    /// <typeparam name="TValue">The type of the new value.</typeparam>
    /// <returns>
    /// A new result of the appropriate value type.
    /// The resulttype of the new result will be the same as ours.
    /// </returns>
    /// <exception cref="InvalidOperationException">Thrown when called on a success result type.</exception>
    internal Result<TValue> MapFailure<TValue>()
    {
        return ResultType switch
        {
            IFailureResultType failure => new Result<TValue>(failure) { MetaData = MetaData },
            ISuccessResultType => throw new InvalidOperationException("Can't use MapFailure on a non failing result."),
            _ => Exception<TValue>(new UnreachableException($"Discriminated union is broken by introducing a {ResultType.GetType().Name}"))
        };
    }
}