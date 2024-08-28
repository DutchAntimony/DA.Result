namespace DA.Results.Extensions;

public static partial class ResultExtensions
{
    public static Result CheckIf(this Result result, bool predicate, Result checkFunc)
    {
        if (!result.IsSuccess) return result;
        if (predicate == false) return result;
        return result.Bind(checkFunc); // Bind and check in the context of a result without value are the same.
    }

    public static Result CheckIf(this Result result, Func<bool> predicate, Func<Result> checkFunc)
    {
        if (!result.IsSuccess) return result;
        if (predicate() == false) return result;
        return result.Bind(checkFunc()); // Bind and check in the context of a result without value are the same.
    }

    public static Result<TValue> CheckIf<TValue>(this Result<TValue> result, bool predicate, Result checkFunc)
    {
        if (!result.IsSuccess(out var _)) return result;
        if (predicate == false) return result;
        return result.Check(checkFunc);
    }

    public static Result<TValue> CheckIf<TValue>(this Result<TValue> result, Func<TValue, bool> predicate, Func<TValue, Result> checkFunc)
    {
        if (!result.IsSuccess(out var value)) return result;
        if (predicate(value) == false) return result;
        return result.Check(checkFunc);
    }

    //todo: add async variants
}