namespace DA.Results.Extensions;

/// <summary>
/// Combine:
/// Combines a Result{T1} and a Result{T2} to a Tuple Result{(T1, T2)}
/// </summary>
public static class CombineResultExtensions
{
    #region Combine Two Results
    /// <summary>
    /// Combines a Result{T1} and a Result{T2} to a Tuple Result{(T1, T2)}   /// </summary>
    /// <param name="first">The result this extension method is called on</param>
    /// <param name="second">The result that must be combined.</param>
    /// <typeparam name="T1">The type of the first result</typeparam>
    /// <typeparam name="T2">The type of the second result</typeparam>
    /// <returns>A new Result{(T1, T2)} with:
    /// - The first issue if the first result is an issue.
    /// - The second issue if the second result is an issue.
    /// - A success result with the combined values and the ignore warning status of the first result.
    /// </returns>
    private static Result<(T1, T2)> Combine<T1, T2>(this Result<T1> first, Result<T2> second)
    {
        if (!first.TryGetValue(out var value1, out var issue1))
        {
            return new Result<(T1, T2)>(issue1);
        }
        
        return !second.TryGetValue(out var value2, out var issue2)
            ? new Result<(T1, T2)>(issue2) 
            : new Result<(T1, T2)>((value1, value2), first.IgnoreWarnings);
    }
    
    /// <summary>
    /// Combines a Result{T1} and a Result{T2} to a Tuple Result{(T1, T2)}   /// </summary>
    /// <param name="first">The result this extension method is called on</param>
    /// <param name="secondFunc">Function that generates the result that must be combined.</param>
    /// <typeparam name="T1">The type of the first result</typeparam>
    /// <typeparam name="T2">The type of the second result</typeparam>
    /// <returns>A new Result{(T1, T2)} with:
    /// - The first issue if the first result is an issue.
    /// - The second issue if the second result is an issue.
    /// - A success result with the combined values and the ignore warning status of the first result.
    /// </returns>
    public static Result<(T1, T2)> Combine<T1, T2>(this Result<T1> first, Func<T1, Result<T2>> secondFunc)
    {
        if (!first.TryGetValue(out var value, out var issue))
        {
            return new Result<(T1, T2)>(issue);
        }
        var second = secondFunc(value);
        return first.Combine(second);
    }

    /// <summary>
    /// Combines a Result{T1} and a Result{T2} to a Tuple Result{(T1, T2)}   /// </summary>
    /// <param name="first">The result this extension method is called on</param>
    /// <param name="secondTask">Task that generates the result that must be combined.</param>
    /// <typeparam name="T1">The type of the first result</typeparam>
    /// <typeparam name="T2">The type of the second result</typeparam>
    /// <returns>A new Result{(T1, T2)} with:
    /// - The first issue if the first result is an issue.
    /// - The second issue if the second result is an issue.
    /// - A success result with the combined values and the ignore warning status of the first result.
    /// </returns>
    public static async Task<Result<(T1, T2)>> CombineAsync<T1, T2>(this Result<T1> first, Func<Task<Result<T2>>> secondTask)
    {
        if (first.TryGetIssue(out var issue))
        {
            return new Result<(T1, T2)>(issue);
        }
        var second = await secondTask();
        return first.Combine(second);
    }

    /// <summary>
    /// Combines a Result{T1} and a Result{T2} to a Tuple Result{(T1, T2)}   /// </summary>
    /// <param name="first">The result this extension method is called on</param>
    /// <param name="secondTaskFunc">FuncTask that generates the result that must be combined.</param>
    /// <typeparam name="T1">The type of the first result</typeparam>
    /// <typeparam name="T2">The type of the second result</typeparam>
    /// <returns>A new Result{(T1, T2)} with:
    /// - The first issue if the first result is an issue.
    /// - The second issue if the second result is an issue.
    /// - A success result with the combined values and the ignore warning status of the first result.
    /// </returns>
    public static async Task<Result<(T1, T2)>> CombineAsync<T1, T2>(this Result<T1> first, Func<T1, Task<Result<T2>>> secondTaskFunc)
    {
        if (!first.TryGetValue(out var value, out var issue))
        {
            return new Result<(T1, T2)>(issue);
        }
        var second = await secondTaskFunc(value);
        return first.Combine(second);
    }

    /// <summary>
    /// Combines a Result{T1} and a Result{T2} to a Tuple Result{(T1, T2)}   /// </summary>
    /// <param name="firstTask">The task of a result this extension method is called on</param>
    /// <param name="secondFunc">Function that generates the result that must be combined.</param>
    /// <typeparam name="T1">The type of the first result</typeparam>
    /// <typeparam name="T2">The type of the second result</typeparam>
    /// <returns>A new Result{(T1, T2)} with:
    /// - The first issue if the first result is an issue.
    /// - The second issue if the second result is an issue.
    /// - A success result with the combined values and the ignore warning status of the first result.
    /// </returns>
    public static async Task<Result<(T1, T2)>> Combine<T1, T2>(this Task<Result<T1>> firstTask, Func<T1, Result<T2>> secondFunc)
    {
        var first = await firstTask;
        return Combine(first, secondFunc);
    }

    /// <summary>
    /// Combines a Result{T1} and a Result{T2} to a Tuple Result{(T1, T2)}   /// </summary>
    /// <param name="firstTask">The task of a result this extension method is called on</param>
    /// <param name="secondTask">Task that generates the result that must be combined.</param>
    /// <typeparam name="T1">The type of the first result</typeparam>
    /// <typeparam name="T2">The type of the second result</typeparam>
    /// <returns>A new Result{(T1, T2)} with:
    /// - The first issue if the first result is an issue.
    /// - The second issue if the second result is an issue.
    /// - A success result with the combined values and the ignore warning status of the first result.
    /// </returns>
    public static async Task<Result<(T1, T2)>> CombineAsync<T1, T2>(this Task<Result<T1>> firstTask, Func<Task<Result<T2>>> secondTask)
    {
        var first = await firstTask;
        return await CombineAsync(first, secondTask);
    }

    /// <summary>
    /// Combines a Result{T1} and a Result{T2} to a Tuple Result{(T1, T2)}   /// </summary>
    /// <param name="firstTask">The task of a result this extension method is called on</param>
    /// <param name="secondTaskFunc">FuncTask that generates the result that must be combined.</param>
    /// <typeparam name="T1">The type of the first result</typeparam>
    /// <typeparam name="T2">The type of the second result</typeparam>
    /// <returns>A new Result{(T1, T2)} with:
    /// - The first issue if the first result is an issue.
    /// - The second issue if the second result is an issue.
    /// - A success result with the combined values and the ignore warning status of the first result.
    /// </returns>
    public static async Task<Result<(T1, T2)>> CombineAsync<T1, T2>(this Task<Result<T1>> firstTask, Func<T1, Task<Result<T2>>> secondTaskFunc)
    {
        var first = await firstTask;
        return await CombineAsync(first, secondTaskFunc);
    }
    #endregion
    
    #region Combine a Result with a value 
    
    /// <summary>
    /// Combines a Result{T1} with a value T2 to a Tuple Result{(T1, T2)}   /// </summary>
    /// <param name="first">The result this extension method is called on</param>
    /// <param name="value2">The value that must be combined.</param>
    /// <typeparam name="T1">The type of the first result</typeparam>
    /// <typeparam name="T2">The type of the second result</typeparam>
    /// <returns>A new Result{(T1, T2)} with:
    /// - The first issue if the first result is an issue.
    /// - A success result with the combined values and the ignore warning status of the first result.
    /// </returns>
    public static Result<(T1, T2)> Combine<T1, T2>(this Result<T1> first, T2 value2)
    {
        return !first.TryGetValue(out var value1, out var issue1) 
            ? new Result<(T1, T2)>(issue1) 
            : new Result<(T1, T2)>((value1, value2), first.IgnoreWarnings);
    }
    
    /// <summary>
    /// Combines a Result{T1} with a value T2 to a Tuple Result{(T1, T2)}   /// </summary>
    /// <param name="first">The result this extension method is called on</param>
    /// <param name="secondFunc">The func that generates the value that must be combined.</param>
    /// <typeparam name="T1">The type of the first result</typeparam>
    /// <typeparam name="T2">The type of the second result</typeparam>
    /// <returns>A new Result{(T1, T2)} with:
    /// - The first issue if the first result is an issue.
    /// - A success result with the combined values and the ignore warning status of the first result.
    /// </returns>
    public static Result<(T1, T2)> Combine<T1, T2>(this Result<T1> first, Func<T1, T2> secondFunc)
    {
        return !first.TryGetValue(out var value1, out var issue1) 
            ? new Result<(T1, T2)>(issue1) 
            : new Result<(T1, T2)>((value1, secondFunc(value1)), first.IgnoreWarnings);
    }

    /// <summary>
    /// Combines a Result{T1} with a value T2 to a Tuple Result{(T1, T2)}   /// </summary>
    /// <param name="first">The result this extension method is called on</param>
    /// <param name="secondTask">The task that generates the value that must be combined.</param>
    /// <typeparam name="T1">The type of the first result</typeparam>
    /// <typeparam name="T2">The type of the second result</typeparam>
    /// <returns>A new Result{(T1, T2)} with:
    /// - The first issue if the first result is an issue.
    /// - A success result with the combined values and the ignore warning status of the first result.
    /// </returns>
    public static async Task<Result<(T1, T2)>> CombineAsync<T1, T2>(this Result<T1> first, Func<Task<T2>> secondTask)
    {
        return !first.TryGetValue(out var value1, out var issue1) 
            ? new Result<(T1, T2)>(issue1) 
            : new Result<(T1, T2)>((value1, await secondTask()), first.IgnoreWarnings);
    }

    /// <summary>
    /// Combines a Result{T1} with a value T2 to a Tuple Result{(T1, T2)}
    /// </summary>
    /// <param name="first">The result this extension method is called on</param>
    /// <param name="secondFuncTask">The FuncTask that generates the value that must be combined.</param>
    /// <typeparam name="T1">The type of the first result</typeparam>
    /// <typeparam name="T2">The type of the second result</typeparam>
    /// <returns>A new Result{(T1, T2)} with:
    /// - The first issue if the first result is an issue.
    /// - A success result with the combined values and the ignore warning status of the first result.
    /// </returns>
    public static async Task<Result<(T1, T2)>> CombineAsync<T1, T2>(this Result<T1> first, Func<T1, Task<T2>> secondFuncTask)
    {
        return !first.TryGetValue(out var value1, out var issue1) 
            ? new Result<(T1, T2)>(issue1) 
            : new Result<(T1, T2)>((value1, await secondFuncTask(value1)), first.IgnoreWarnings);
    }
    
    /// <summary>
    /// Combines a Result{T1} with a value T2 to a Tuple Result{(T1, T2)}   /// </summary>
    /// <param name="firstTask">The Task of the Result this extension method is called on</param>
    /// <param name="value2">The value that must be combined.</param>
    /// <typeparam name="T1">The type of the first result</typeparam>
    /// <typeparam name="T2">The type of the second result</typeparam>
    /// <returns>A new Result{(T1, T2)} with:
    /// - The first issue if the first result is an issue.
    /// - A success result with the combined values and the ignore warning status of the first result.
    /// </returns>
    public static async Task<Result<(T1, T2)>> Combine<T1, T2>(this Task<Result<T1>> firstTask, T2 value2)
    {
        var first = await firstTask;
        return first.Combine(value2);
    }
    
    /// <summary>
    /// Combines a Result{T1} with a value T2 to a Tuple Result{(T1, T2)}   /// </summary>
    /// <param name="firstTask">The Task of the Result this extension method is called on</param>
    /// <param name="secondFunc">The func that generates the value that must be combined.</param>
    /// <typeparam name="T1">The type of the first result</typeparam>
    /// <typeparam name="T2">The type of the second result</typeparam>
    /// <returns>A new Result{(T1, T2)} with:
    /// - The first issue if the first result is an issue.
    /// - A success result with the combined values and the ignore warning status of the first result.
    /// </returns>
    public static async Task<Result<(T1, T2)>> Combine<T1, T2>(this Task<Result<T1>> firstTask, Func<T1, T2> secondFunc)
    {
        var first = await firstTask;
        return Combine(first, secondFunc);
    }

    /// <summary>
    /// Combines a Result{T1} with a value T2 to a Tuple Result{(T1, T2)}   /// </summary>
    /// <param name="firstTask">The Task of the Result this extension method is called on</param>
    /// <param name="secondTask">The task that generates the value that must be combined.</param>
    /// <typeparam name="T1">The type of the first result</typeparam>
    /// <typeparam name="T2">The type of the second result</typeparam>
    /// <returns>A new Result{(T1, T2)} with:
    /// - The first issue if the first result is an issue.
    /// - A success result with the combined values and the ignore warning status of the first result.
    /// </returns>
    public static async Task<Result<(T1, T2)>> CombineAsync<T1, T2>(this Task<Result<T1>> firstTask, Func<Task<T2>> secondTask)
    {
        var first = await firstTask;
        return await CombineAsync(first, secondTask);
    }

    /// <summary>
    /// Combines a Result{T1} with a value T2 to a Tuple Result{(T1, T2)}
    /// </summary>
    /// <param name="firstTask">The Task of the Result this extension method is called on</param>
    /// <param name="secondFuncTask">The FuncTask that generates the value that must be combined.</param>
    /// <typeparam name="T1">The type of the first result</typeparam>
    /// <typeparam name="T2">The type of the second result</typeparam>
    /// <returns>A new Result{(T1, T2)} with:
    /// - The first issue if the first result is an issue.
    /// - A success result with the combined values and the ignore warning status of the first result.
    /// </returns>
    public static async Task<Result<(T1, T2)>> CombineAsync<T1, T2>(this Task<Result<T1>> firstTask, Func<T1, Task<T2>> secondFuncTask)
    {
        var first = await firstTask;
        return await CombineAsync(first, secondFuncTask);
    }
    #endregion
}