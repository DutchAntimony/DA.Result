namespace DA.Results.Extensions;

/// <summary>
/// Check:
/// Checks a predicate(which returns a result for the messages) on the T of the Result { T }. Result is the same T
/// </summary>
public static class CheckResultExtensions
{
    /// <summary>
    /// Check if the second result is a success or an ignorable warning.
    /// </summary>
    /// <param name="result">The result where check is called.</param>
    /// <param name="check">The second result of which the status is checked.</param>
    /// <typeparam name="TValue">The type of the original result.</typeparam>
    /// <returns>
    /// A Result{<typeparam name="TValue"/>} that is:
    /// - The original failure result.
    /// - A failure with the second issue if the second result was failure with an error issue or a warning issue that could not be ignored.
    /// - The original success result.
    /// </returns>
    internal static Result<TValue> Check<TValue>(this Result<TValue> result, IResult check)
    {
        return (!result.IsSuccess || !check.TryGetIssue(out var secondIssue) || 
                secondIssue.IsWarning && result.IgnoreWarnings)
            ? result
            : new Result<TValue>(secondIssue);
    }

    /// <summary>
    /// Check if the function that produces the second result is a success or an ignorable warning.
    /// </summary>
    /// <param name="result">The result where check is called.</param>
    /// <param name="checkFunc">The function that produces the check of which the status is checked.</param>
    /// <typeparam name="TValue">The type of the original result.</typeparam>
    /// <returns>
    /// A Result{<typeparam name="TValue"/>} that is:
    /// - The original failure result.
    /// - A failure with the second issue if the second result was failure with an error issue or a warning issue that could not be ignored.
    /// - The original success result.
    /// </returns>
    public static Result<TValue> Check<TValue>(this Result<TValue> result, Func<TValue, IResult> checkFunc)
    {
        if (!result.TryGetValue(out var value))
        {
            return result;
        }
        var check = checkFunc(value);
        return result.Check(check);
    }

    /// <summary>
    /// Check if the task that produces the second result is a success or an ignorable warning.
    /// </summary>
    /// <param name="result">The result where check is called.</param>
    /// <param name="checkTask">The Task that produces the check of which the status is checked.</param>
    /// <typeparam name="TValue">The type of the original result.</typeparam>
    /// <returns>
    /// A Result{<typeparam name="TValue"/>} that is:
    /// - The original failure result.
    /// - A failure with the second issue if the second result was failure with an error issue or a warning issue that could not be ignored.
    /// - The original success result.
    /// </returns>
    public static async Task<Result<TValue>> CheckAsync<TValue>(this Result<TValue> result, Func<Task<IResult>> checkTask)
    {
        if (!result.IsSuccess)
        {
            return result;
        }
        var check = await checkTask();
        return result.Check(check);
    }

    /// <summary>
    /// Check if the FuncTask that produces the second result is a success or an ignorable warning.
    /// </summary>
    /// <param name="result">The result where check is called.</param>
    /// <param name="checkFuncTask">The Task that produces the check of which the status is checked.</param>
    /// <typeparam name="TValue">The type of the original result.</typeparam>
    /// <returns>
    /// A Result{<typeparam name="TValue"/>} that is:
    /// - The original failure result.
    /// - A failure with the second issue if the second result was failure with an error issue or a warning issue that could not be ignored.
    /// - The original success result.
    /// </returns>
    public static async Task<Result<TValue>> CheckAsync<TValue>(this Result<TValue> result, Func<TValue, Task<IResult>> checkFuncTask)
    {
        if (!result.TryGetValue(out var value))
        {
            return result;
        }
        var check = await checkFuncTask(value);
        return result.Check(check);
    }

    /// <summary>
    /// Check if the function that produces the second result is a success or an ignorable warning.
    /// </summary>
    /// <param name="resultTask">The result task where check is called.</param>
    /// <param name="checkFunc">The function that produces the check of which the status is checked.</param>
    /// <typeparam name="TValue">The type of the original result.</typeparam>
    /// <returns>
    /// A Result{<typeparam name="TValue"/>} that is:
    /// - The original failure result.
    /// - A failure with the second issue if the second result was failure with an error issue or a warning issue that could not be ignored.
    /// - The original success result.
    /// </returns>
    public static async Task<Result<TValue>> Check<TValue>(this Task<Result<TValue>> resultTask, Func<TValue, IResult> checkFunc)
    {
        var result = await resultTask;
        return Check(result, checkFunc);
    }
    
    /// <summary>
    /// Check if the task that produces the second result is a success or an ignorable warning.
    /// </summary>
    /// <param name="resultTask">The result task where check is called.</param>
    /// <param name="checkTask">The Task that produces the check of which the status is checked.</param>
    /// <typeparam name="TValue">The type of the original result.</typeparam>
    /// <returns>
    /// A Result{<typeparam name="TValue"/>} that is:
    /// - The original failure result.
    /// - A failure with the second issue if the second result was failure with an error issue or a warning issue that could not be ignored.
    /// - The original success result.
    /// </returns>
    public static async Task<Result<TValue>> CheckAsync<TValue>(this Task<Result<TValue>> resultTask, Func<Task<IResult>> checkTask)
    {
        var result = await resultTask;
        return await CheckAsync(result, checkTask);
    }

    /// <summary>
    /// Check if the FuncTask that produces the second result is a success or an ignorable warning.
    /// </summary>
    /// <param name="resultTask">The result task where check is called.</param>
    /// <param name="checkFuncTask">The Task that produces the check of which the status is checked.</param>
    /// <typeparam name="TValue">The type of the original result.</typeparam>
    /// <returns>
    /// A Result{<typeparam name="TValue"/>} that is:
    /// - The original failure result.
    /// - A failure with the second issue if the second result was failure with an error issue or a warning issue that could not be ignored.
    /// - The original success result.
    /// </returns>
    public static async Task<Result<TValue>> CheckAsync<TValue>(this Task<Result<TValue>> resultTask, Func<TValue, Task<IResult>> checkFuncTask)
    {
        var result = await resultTask;
        return await CheckAsync(result, checkFuncTask);
    }

    /// <summary>
    /// Check if the function that produces the second result from all values of a combined result is a success or an ignorable warning.
    /// </summary>
    /// <param name="result">The result where check is called.</param>
    /// <param name="checkFunc">The function that produces the check of which the status is checked.</param>
    /// <typeparam name="T1">The first type of the original combined result.</typeparam>
    /// <typeparam name="T2">The second type of the original combined result.</typeparam>
    /// <returns>
    /// A Result{<typeparam name="T1"/>,<typeparam name="T2"/>} that is:
    /// - The original failure result.
    /// - A failure with the second issue if the second result was failure with an error issue or a warning issue that could not be ignored.
    /// - The original success result.
    /// </returns>
    public static Result<(T1, T2)> Check<T1, T2>(this Result<(T1, T2)> result, Func<T1, T2, IResult> checkFunc)
    {
        if (!result.TryGetValue(out var value))
        {
            return result;
        }
        var check = checkFunc(value.Item1, value.Item2);
        return result.Check(check);
    }

    /// <summary>
    /// Check if the function that produces the second result from all values of a combined result is a success or an ignorable warning.
    /// </summary>
    /// <param name="resultTask">The result task where check is called.</param>
    /// <param name="checkFunc">The function that produces the check of which the status is checked.</param>
    /// <typeparam name="T1">The first type of the original combined result.</typeparam>
    /// <typeparam name="T2">The second type of the original combined result.</typeparam>
    /// <returns>
    /// A Result{<typeparam name="T1"/>,<typeparam name="T2"/>} that is:
    /// - The original failure result.
    /// - A failure with the second issue if the second result was failure with an error issue or a warning issue that could not be ignored.
    /// - The original success result.
    /// </returns>
    public static async Task<Result<(T1, T2)>> Check<T1, T2>(this Task<Result<(T1, T2)>> resultTask, Func<T1, T2, IResult> checkFunc)
    {
        var result = await resultTask;
        return result.Check(checkFunc);
    }
}