using DA.Results.ResultTypes.Failure;
using DA.Options;
using DA.Results.ResultTypes.Success;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace DA.Results;

/// <summary>
/// A result with a value attached. 
/// The value is only present if the result is a success.
/// The value is only retrievable using the IsSuccess function that returns the value as an <see langword="out"/> parameter.
/// </summary>
public sealed record Result<TValue> : Result
{
    private readonly Option<TValue> _valueOption;

    /// <summary>
    /// Create a new success result of <typeparamref name="TValue"/>.
    /// </summary>
    /// <param name="value">The value of the result.</param>
    /// <param name="successResult">The type of success result.</param>
    /// <param name="message">Optional a message describing the state of the result.</param>
    public Result(TValue value, ISuccessResultType successResult, string? message = null) : base(successResult, message)
    {
        _valueOption = Option.From(value);
    }

    /// <summary>
    /// Create a new failure result of <typeparamref name="TValue"/>
    /// </summary>
    /// <param name="failureResult">The type of the failure result.</param>
    /// <param name="message">Optional a message describing the state of the result.</param>
    public Result(IFailureResultType failureResult, string? message = null) : base(failureResult, message)
    {
        _valueOption = Option.None;
    }

    /// <summary>
    /// Check the status of another result type and update the result type if other takes priority.
    /// </summary>
    /// <param name="other">The other result that must be checked.</param>
    /// <returns>This result back with the resulttype updated depending on the incoming result.</returns>
    public Result<TValue> Check(Result other)
    {
        return this with 
        { 
            ResultType = ResultType.Combine(other.ResultType),
            MetaData = [.. MetaData, .. other.MetaData]
        };
    }

    /// <summary>
    /// Combine two results, both with values to a new result with both values combined as tuple.
    /// </summary>
    /// <typeparam name="TOther">The type of the other value.</typeparam>
    /// <param name="other">The other result to create a tuple value type with.</param>
    /// <returns>
    /// A new result with a tuple value type and combined messages.
    /// The resulttype depends on the priority of ours and that of the incoming:
    /// - If the incoming result had a heigher priority resulttype, the resulttype of the other is taken.
    /// - If the incoming result is the same as our type, the data of the resulttype is combined.
    /// - If the incoming result is of a lower priority, our resulttype is preserved.
    /// </returns>
    public Result<(TValue, TOther)> Combine<TOther>(Result<TOther> other)
    {
        var newResultType = ResultType.Combine(other.ResultType);
        if (newResultType is IFailureResultType failure)
        {
            return new Result<(TValue, TOther)>(failure) { MetaData = [.. MetaData, .. other.MetaData] };
        }

        if (newResultType is ISuccessResultType success && IsSuccess(out var value) && other.IsSuccess(out var otherValue))
        {
            return new Result<(TValue, TOther)>((value, otherValue), success) { MetaData = [.. MetaData, .. other.MetaData] };
        }

        return Exception<(TValue, TOther)>(new UnreachableException("Invalid implementation of ResultType"));
    }

    /// <summary>
    /// Combine the current result with another value
    /// </summary>
    /// <typeparam name="TOther">The type of the other value.</typeparam>
    /// <param name="other">The other value to create a tuple value type with.</param>
    /// <returns>A new result with the existing resulttype and a tuple value type.</returns>
    public Result<(TValue, TOther)> Combine<TOther>(TOther other)
    {
        if (IsSuccess(out var value) && ResultType is ISuccessResultType success)
        {
            return new Result<(TValue, TOther)>((value, other), success) { MetaData = MetaData };
        }

        if (ResultType is IFailureResultType failure)
        {
            return new Result<(TValue, TOther)>(failure) { MetaData = MetaData };
        }

        return Exception<(TValue, TOther)>(new UnreachableException("Invalid implementation of ResultType"));
    }

    /// <summary>
    /// Flatten the Result{TValue} to a Result without value type.
    /// </summary>
    /// <returns>A new instance of a Result.</returns>
    public Result Flatten()
    {
        return new Result(ResultType) { MetaData = MetaData };
    }

    /// <summary>
    /// Function to determine if the result is a success and retreive the value this result is wrapping.
    /// </summary>
    /// <param name="value">The value of type <typeparamref name="TValue"/> that is present if the result is a success.</param>
    /// <returns>Boolean indicating if the result is a success.</returns>
    public new bool IsSuccess([MaybeNullWhen(false), NotNullWhen(true)] out TValue value)
    {
        if (!_valueOption.TryGetValue(out value) && ResultType.IsSuccess)
        {
            throw new UnreachableException("A success result was initialized without a value.");
        }

        return ResultType.IsSuccess;
    }
}
