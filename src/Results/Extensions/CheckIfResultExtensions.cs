namespace DA.Results.Extensions;

/// <summary>
/// CheckIf:
/// Checks a predicate(which returns a result for the messages) on the T of the Result { T }. Result is the same T
/// Check is only done if a certain condition on the value of the result if met.
/// </summary>
public static class CheckIfResultExtensions
{
    #region Predicate independend of TValue
    /// <summary>
    /// Check this result with another result, but only if an initial predicate is true. 
    /// </summary>
    /// <param name="result">The result where CheckIf is called.</param>
    /// <param name="predicate">The predicate that must first me match, otherwise the check is not performed.</param>
    /// <param name="checkFunc">The function that produces the check of which the status is checked.</param>
    /// <typeparam name="TValue">The type of the original result.</typeparam>
    /// <returns>
    /// A Result{<typeparam name="TValue"/>} that is:
    /// - The original failure result.
    /// - The original success result if the predicate is false.
    /// - A failure with the second issue if the second result was failure with an error issue or a warning issue that could not be ignored.
    /// - The original success result.
    /// </returns>
    public static Result<TValue> CheckIf<TValue>(this Result<TValue> result, Func<bool> predicate, Func<TValue, IResult> checkFunc)
    {
        if (!result.IsSuccess)
        {
            return result;
        }
        
        return predicate() == false 
            ? result 
            : result.Check(checkFunc);
    }
    
    /// <summary>
    /// Check this result with another result, but only if an initial predicate is true. 
    /// </summary>
    /// <param name="resultTask">The task of a result where CheckIf is called.</param>
    /// <param name="predicate">The predicate that must first me match, otherwise the check is not performed.</param>
    /// <param name="checkFunc">The function that produces the check of which the status is checked.</param>
    /// <typeparam name="TValue">The type of the original result.</typeparam>
    /// <returns>
    /// A Result{<typeparam name="TValue"/>} that is:
    /// - The original failure result.
    /// - The original success result if the predicate is false.
    /// - A failure with the second issue if the second result was failure with an error issue or a warning issue that could not be ignored.
    /// - The original success result.
    /// </returns>
    public static async Task<Result<TValue>> CheckIf<TValue>(this Task<Result<TValue>> resultTask, Func<bool> predicate, Func<TValue, IResult> checkFunc)
    {
        var result = await resultTask;
        return result.CheckIf(predicate, checkFunc);
    }

    /// <summary>
    /// Check this result with another result, but only if an initial predicate is true. 
    /// </summary>
    /// <param name="result">The result where CheckIf is called.</param>
    /// <param name="predicate">The predicate that must first me match, otherwise the check is not performed.</param>
    /// <param name="checkTask">The task that produces the check of which the status is checked.</param>
    /// <typeparam name="TValue">The type of the original result.</typeparam>
    /// <returns>
    /// A Result{<typeparam name="TValue"/>} that is:
    /// - The original failure result.
    /// - The original success result if the predicate is false.
    /// - A failure with the second issue if the second result was failure with an error issue or a warning issue that could not be ignored.
    /// - The original success result.
    /// </returns>
    public static async Task<Result<TValue>> CheckIfAsync<TValue>(this Result<TValue> result, Func<bool> predicate, Func<Task<IResult>> checkTask)
    {
        if (!result.IsSuccess) return result;
        return predicate() == false ? result : result.Check(await checkTask());
    }

    /// <summary>
    /// Check this result with another result, but only if an initial predicate is true. 
    /// </summary>
    /// <param name="result">The result where CheckIf is called.</param>
    /// <param name="predicate">The predicate that must first me match, otherwise the check is not performed.</param>
    /// <param name="checkFuncTask">The FuncTask that produces the check of which the status is checked.</param>
    /// <typeparam name="TValue">The type of the original result.</typeparam>
    /// <returns>
    /// A Result{<typeparam name="TValue"/>} that is:
    /// - The original failure result.
    /// - The original success result if the predicate is false.
    /// - A failure with the second issue if the second result was failure with an error issue or a warning issue that could not be ignored.
    /// - The original success result.
    /// </returns>
    public static async Task<Result<TValue>> CheckIfAsync<TValue>(this Result<TValue> result, Func<bool> predicate, Func<TValue, Task<IResult>> checkFuncTask)
    {
        if (!result.TryGetValue(out var value)) return result;
        return predicate() == false ? result : result.Check(await checkFuncTask(value));
    }
    
    /// <summary>
    /// Check this result with another result, but only if an initial predicate is true. 
    /// </summary>
    /// <param name="resultTask">The task of a result where CheckIf is called.</param>
    /// <param name="predicate">The predicate that must first me match, otherwise the check is not performed.</param>
    /// <param name="checkTask">The task that produces the check of which the status is checked.</param>
    /// <typeparam name="TValue">The type of the original result.</typeparam>
    /// <returns>
    /// A Result{<typeparam name="TValue"/>} that is:
    /// - The original failure result.
    /// - The original success result if the predicate is false.
    /// - A failure with the second issue if the second result was failure with an error issue or a warning issue that could not be ignored.
    /// - The original success result.
    /// </returns>
    public static async Task<Result<TValue>> CheckIfAsync<TValue>(this Task<Result<TValue>> resultTask, Func<bool> predicate, Func<Task<IResult>> checkTask)
    {
        var result = await resultTask;
        return await result.CheckIfAsync(predicate, checkTask);
    }

    /// <summary>
    /// Check this result with another result, but only if an initial predicate is true. 
    /// </summary>
    /// <param name="resultTask">The task of a result where CheckIf is called.</param>
    /// <param name="predicate">The predicate that must first me match, otherwise the check is not performed.</param>
    /// <param name="checkFuncTask">The FuncTask that produces the check of which the status is checked.</param>
    /// <typeparam name="TValue">The type of the original result.</typeparam>
    /// <returns>
    /// A Result{<typeparam name="TValue"/>} that is:
    /// - The original failure result.
    /// - The original success result if the predicate is false.
    /// - A failure with the second issue if the second result was failure with an error issue or a warning issue that could not be ignored.
    /// - The original success result.
    /// </returns>
    public static async Task<Result<TValue>> CheckIfAsync<TValue>(this Task<Result<TValue>> resultTask, Func<bool> predicate, Func<TValue, Task<IResult>> checkFuncTask)
    {
        var result = await resultTask;
        return await result.CheckIfAsync(predicate, checkFuncTask);
    }
    #endregion
    
    #region Predicate dependend of TValue
    /// <summary>
    /// Check this result with another result, but only if an initial predicate is true. 
    /// </summary>
    /// <param name="result">The result where CheckIf is called.</param>
    /// <param name="predicate">The predicate that must first me match, otherwise the check is not performed.</param>
    /// <param name="checkFunc">The function that produces the check of which the status is checked.</param>
    /// <typeparam name="TValue">The type of the original result.</typeparam>
    /// <returns>
    /// A Result{<typeparam name="TValue"/>} that is:
    /// - The original failure result.
    /// - The original success result if the predicate is false.
    /// - A failure with the second issue if the second result was failure with an error issue or a warning issue that could not be ignored.
    /// - The original success result.
    /// </returns>
    public static Result<TValue> CheckIf<TValue>(this Result<TValue> result, Func<TValue, bool> predicate, Func<TValue, IResult> checkFunc)
    {
        if (!result.TryGetValue(out var value))
        {
            return result;
        }
        
        return predicate(value) == false 
            ? result 
            : result.Check(checkFunc);
    }
    
    /// <summary>
    /// Check this result with another result, but only if an initial predicate is true. 
    /// </summary>
    /// <param name="resultTask">The task of a result where CheckIf is called.</param>
    /// <param name="predicate">The predicate that must first me match, otherwise the check is not performed.</param>
    /// <param name="checkFunc">The function that produces the check of which the status is checked.</param>
    /// <typeparam name="TValue">The type of the original result.</typeparam>
    /// <returns>
    /// A Result{<typeparam name="TValue"/>} that is:
    /// - The original failure result.
    /// - The original success result if the predicate is false.
    /// - A failure with the second issue if the second result was failure with an error issue or a warning issue that could not be ignored.
    /// - The original success result.
    /// </returns>
    public static async Task<Result<TValue>> CheckIf<TValue>(this Task<Result<TValue>> resultTask, Func<TValue, bool> predicate, Func<TValue, IResult> checkFunc)
    {
        var result = await resultTask;
        return result.CheckIf(predicate, checkFunc);
    }

    /// <summary>
    /// Check this result with another result, but only if an initial predicate is true. 
    /// </summary>
    /// <param name="result">The result where CheckIf is called.</param>
    /// <param name="predicate">The predicate that must first me match, otherwise the check is not performed.</param>
    /// <param name="checkTask">The task that produces the check of which the status is checked.</param>
    /// <typeparam name="TValue">The type of the original result.</typeparam>
    /// <returns>
    /// A Result{<typeparam name="TValue"/>} that is:
    /// - The original failure result.
    /// - The original success result if the predicate is false.
    /// - A failure with the second issue if the second result was failure with an error issue or a warning issue that could not be ignored.
    /// - The original success result.
    /// </returns>
    public static async Task<Result<TValue>> CheckIfAsync<TValue>(this Result<TValue> result, Func<TValue, bool> predicate, Func<Task<IResult>> checkTask)
    {
        if (!result.TryGetValue(out var value)) return result;
        return predicate(value) == false ? result : result.Check(await checkTask());
    }

    /// <summary>
    /// Check this result with another result, but only if an initial predicate is true. 
    /// </summary>
    /// <param name="result">The result where CheckIf is called.</param>
    /// <param name="predicate">The predicate that must first me match, otherwise the check is not performed.</param>
    /// <param name="checkFuncTask">The FuncTask that produces the check of which the status is checked.</param>
    /// <typeparam name="TValue">The type of the original result.</typeparam>
    /// <returns>
    /// A Result{<typeparam name="TValue"/>} that is:
    /// - The original failure result.
    /// - The original success result if the predicate is false.
    /// - A failure with the second issue if the second result was failure with an error issue or a warning issue that could not be ignored.
    /// - The original success result.
    /// </returns>
    public static async Task<Result<TValue>> CheckIfAsync<TValue>(this Result<TValue> result, Func<TValue, bool> predicate, Func<TValue, Task<IResult>> checkFuncTask)
    {
        if (!result.TryGetValue(out var value)) return result;
        return predicate(value) == false ? result : result.Check(await checkFuncTask(value));
    }
    
    /// <summary>
    /// Check this result with another result, but only if an initial predicate is true. 
    /// </summary>
    /// <param name="resultTask">The task of a result where CheckIf is called.</param>
    /// <param name="predicate">The predicate that must first me match, otherwise the check is not performed.</param>
    /// <param name="checkTask">The task that produces the check of which the status is checked.</param>
    /// <typeparam name="TValue">The type of the original result.</typeparam>
    /// <returns>
    /// A Result{<typeparam name="TValue"/>} that is:
    /// - The original failure result.
    /// - The original success result if the predicate is false.
    /// - A failure with the second issue if the second result was failure with an error issue or a warning issue that could not be ignored.
    /// - The original success result.
    /// </returns>
    public static async Task<Result<TValue>> CheckIfAsync<TValue>(this Task<Result<TValue>> resultTask, Func<TValue, bool> predicate, Func<Task<IResult>> checkTask)
    {
        var result = await resultTask;
        return await result.CheckIfAsync(predicate, checkTask);
    }

    /// <summary>
    /// Check this result with another result, but only if an initial predicate is true. 
    /// </summary>
    /// <param name="resultTask">The task of a result where CheckIf is called.</param>
    /// <param name="predicate">The predicate that must first me match, otherwise the check is not performed.</param>
    /// <param name="checkFuncTask">The FuncTask that produces the check of which the status is checked.</param>
    /// <typeparam name="TValue">The type of the original result.</typeparam>
    /// <returns>
    /// A Result{<typeparam name="TValue"/>} that is:
    /// - The original failure result.
    /// - The original success result if the predicate is false.
    /// - A failure with the second issue if the second result was failure with an error issue or a warning issue that could not be ignored.
    /// - The original success result.
    /// </returns>
    public static async Task<Result<TValue>> CheckIfAsync<TValue>(this Task<Result<TValue>> resultTask, Func<TValue, bool> predicate, Func<TValue, Task<IResult>> checkFuncTask)
    {
        var result = await resultTask;
        return await result.CheckIfAsync(predicate, checkFuncTask);
    }
    #endregion
}