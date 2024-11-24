namespace DA.Results.Extensions;

/// <summary>
/// Map: Convert a Result{TIn} to a Result{TOut} by supplying a TOut.
/// It returns:
/// - A failure with the original issue if the original result was a failure.
/// - A success with the supplied value and the original IgnoreWarnings configuration.
/// </summary>
public static class MapResultExtensions
{
    #region Map from a Result{TIn} to a Result{TOut} by providing a value.

    /// <summary>
    /// Convert a Result{TIn} to a Result{TOut} by supplying a TOut.
    /// </summary>
    /// <param name="result">The result this function is called from.</param>
    /// <param name="value">The value of the new result.</param>
    /// <typeparam name="TIn">The type of the original result.</typeparam>
    /// <typeparam name="TOut">The type of the outgoing result.</typeparam>
    /// <returns>
    /// - A failure with the original issue if the original result was a failure.
    /// - A success with the supplied value and the original IgnoreWarnings configuration.
    /// </returns>
    public static Result<TOut> Map<TIn, TOut>(this Result<TIn> result, TOut value)
    {
        return result.TryGetIssue(out var issue) 
            ? new Result<TOut>(issue) 
            : new Result<TOut>(value, result.IgnoreWarnings);
    }
    
    /// <summary>
    /// Convert a Result{TIn} to a Result{TOut} by supplying a TOut.
    /// </summary>
    /// <param name="result">The result this function is called from.</param>
    /// <param name="valueFunc">The function to generate the value of the new result.</param>
    /// <typeparam name="TIn">The type of the original result.</typeparam>
    /// <typeparam name="TOut">The type of the outgoing result.</typeparam>
    /// <returns>
    /// - A failure with the original issue if the original result was a failure.
    /// - A success with the supplied value and the original IgnoreWarnings configuration.
    /// </returns>
    public static Result<TOut> Map<TIn, TOut>(this Result<TIn> result, Func<TOut> valueFunc)
    {
        return result.TryGetIssue(out var issue) 
            ? new Result<TOut>(issue) 
            : new Result<TOut>(valueFunc(), result.IgnoreWarnings);
    }

    /// <summary>
    /// Convert a Result{TIn} to a Result{TOut} by supplying a TOut.
    /// </summary>
    /// <param name="result">The result this function is called from.</param>
    /// <param name="valueTask">The task to generate the value of the new result.</param>
    /// <typeparam name="TIn">The type of the original result.</typeparam>
    /// <typeparam name="TOut">The type of the outgoing result.</typeparam>
    /// <returns>
    /// - A failure with the original issue if the original result was a failure.
    /// - A success with the supplied value and the original IgnoreWarnings configuration.
    /// </returns>
    public static async Task<Result<TOut>> MapAsync<TIn, TOut>(this Result<TIn> result, Func<Task<TOut>> valueTask)
    {
        return result.TryGetIssue(out var issue) 
            ? new Result<TOut>(issue) 
            : new Result<TOut>(await valueTask(), result.IgnoreWarnings);
    }

    /// <summary>
    /// Convert a Result{TIn} to a Result{TOut} by supplying a TOut.
    /// </summary>
    /// <param name="resultTask">The Task of a Result this function is called from.</param>
    /// <param name="value">The value of the new result.</param>
    /// <typeparam name="TIn">The type of the original result.</typeparam>
    /// <typeparam name="TOut">The type of the outgoing result.</typeparam>
    /// <returns>
    /// - A failure with the original issue if the original result was a failure.
    /// - A success with the supplied value and the original IgnoreWarnings configuration.
    /// </returns>
    public static async Task<Result<TOut>> Map<TIn, TOut>(this Task<Result<TIn>> resultTask, TOut value)
    {
        var result = await resultTask;
        return Map(result, value);
    }
    
    /// <summary>
    /// Convert a Result{TIn} to a Result{TOut} by supplying a TOut.
    /// </summary>
    /// <param name="resultTask">The Task of a Result this function is called from.</param>
    /// <param name="valueFunc">The function to generate the value of the new result.</param>
    /// <typeparam name="TIn">The type of the original result.</typeparam>
    /// <typeparam name="TOut">The type of the outgoing result.</typeparam>
    /// <returns>
    /// - A failure with the original issue if the original result was a failure.
    /// - A success with the supplied value and the original IgnoreWarnings configuration.
    /// </returns>
    public static async Task<Result<TOut>> Map<TIn, TOut>(this Task<Result<TIn>> resultTask, Func<TOut> valueFunc)
    {
        var result = await resultTask;
        return Map(result,valueFunc);
    }

    /// <summary>
    /// Convert a Result{TIn} to a Result{TOut} by supplying a TOut.
    /// </summary>
    /// <param name="resultTask">The Task of a Result this function is called from.</param>
    /// <param name="valueTask">The task to generate the value of the new result.</param>
    /// <typeparam name="TIn">The type of the original result.</typeparam>
    /// <typeparam name="TOut">The type of the outgoing result.</typeparam>
    /// <returns>
    /// - A failure with the original issue if the original result was a failure.
    /// - A success with the supplied value and the original IgnoreWarnings configuration.
    /// </returns>
    public static async Task<Result<TOut>> MapAsync<TIn, TOut>(this Task<Result<TIn>> resultTask, Func<Task<TOut>> valueTask)
    {
        var result = await resultTask;
        return await MapAsync(result, valueTask);
    }

    #endregion

    #region Map a Result{TIn} to Result{TOut} where TOut is Func(TIn)

    /// <summary>
    /// Convert a Result{TIn} to a Result{TOut} by supplying a function from TIn to TOut.
    /// </summary>
    /// <param name="result">The result this function is called from.</param>
    /// <param name="valueFunc">The function to calculate of the new result.</param>
    /// <typeparam name="TIn">The type of the original result.</typeparam>
    /// <typeparam name="TOut">The type of the outgoing result.</typeparam>
    /// <returns>
    /// - A failure with the original issue if the original result was a failure.
    /// - A success with the supplied value and the original IgnoreWarnings configuration.
    /// </returns>
    public static Result<TOut> Map<TIn, TOut>(this Result<TIn> result, Func<TIn, TOut> valueFunc)
    {
        return result.TryGetValue(out var value1, out var issue) 
            ? new Result<TOut>(valueFunc(value1), result.IgnoreWarnings)
            : new Result<TOut>(issue);
    }

    /// <summary>
    /// Convert a Result{TIn} to a Result{TOut} by supplying a function from TIn to TOut.
    /// </summary>
    /// <param name="result">The result this function is called from.</param>
    /// <param name="valueTaskFunc">The function to calculate of the new result.</param>
    /// <typeparam name="TIn">The type of the original result.</typeparam>
    /// <typeparam name="TOut">The type of the outgoing result.</typeparam>
    /// <returns>
    /// - A failure with the original issue if the original result was a failure.
    /// - A success with the supplied value and the original IgnoreWarnings configuration.
    /// </returns>
    public static async Task<Result<TOut>> MapAsync<TIn, TOut>(this Result<TIn> result, Func<TIn, Task<TOut>> valueTaskFunc)
    {
        return result.TryGetValue(out var value1, out var issue) 
            ? new Result<TOut>(await valueTaskFunc(value1), result.IgnoreWarnings)
            : new Result<TOut>(issue);
    }

    /// <summary>
    /// Convert a Result{TIn} to a Result{TOut} by supplying a function from TIn to TOut.
    /// </summary>
    /// <param name="resultTask">The Task of a Result this function is called from.</param>
    /// <param name="valueFunc">The function to calculate of the new result.</param>
    /// <typeparam name="TIn">The type of the original result.</typeparam>
    /// <typeparam name="TOut">The type of the outgoing result.</typeparam>
    /// <returns>
    /// - A failure with the original issue if the original result was a failure.
    /// - A success with the supplied value and the original IgnoreWarnings configuration.
    /// </returns>
    public static async Task<Result<TOut>> Map<TIn, TOut>(this Task<Result<TIn>> resultTask, Func<TIn, TOut> valueFunc)
    {
        var result = await resultTask;
        return Map(result, valueFunc);
    }

    /// <summary>
    /// Convert a Result{TIn} to a Result{TOut} by supplying a function from TIn to TOut.
    /// </summary>
    /// <param name="resultTask">The Task of a Result this function is called from.</param>
    /// <param name="valueTaskFunc">The function to calculate of the new result.</param>
    /// <typeparam name="TIn">The type of the original result.</typeparam>
    /// <typeparam name="TOut">The type of the outgoing result.</typeparam>
    /// <returns>
    /// - A failure with the original issue if the original result was a failure.
    /// - A success with the supplied value and the original IgnoreWarnings configuration.
    /// </returns>
    public static async Task<Result<TOut>> MapAsync<TIn, TOut>(this Task<Result<TIn>> resultTask, Func<TIn, Task<TOut>> valueTaskFunc)
    {
        var result = await resultTask;
        return await MapAsync(result, valueTaskFunc);
    }
    #endregion

    #region Map a Result{(T1, T2)} to a Result{TOut} where TOut if Func(T1, T2)

    /// <summary>
    /// Convert a Result{T1, T2} to a Result{TOut} by supplying a function from T1 and T2 to TOut.
    /// </summary>
    /// <param name="result">The result this function is called from.</param>
    /// <param name="valueFunc">The function to calculate of the new result.</param>
    /// <typeparam name="T1">The first type of the original result.</typeparam>
    /// <typeparam name="T2">The second type of the original result.</typeparam>
    /// <typeparam name="TOut">The type of the outgoing result.</typeparam>
    /// <returns>
    /// - A failure with the original issue if the original result was a failure.
    /// - A success with the supplied value and the original IgnoreWarnings configuration.
    /// </returns>
    public static Result<TOut> Map<T1, T2, TOut>(this Result<(T1, T2)> result, Func<T1, T2, TOut> valueFunc)
    {
        return result.TryGetValue(out var values, out var issue) 
            ? new Result<TOut>(valueFunc(values.Item1, values.Item2), result.IgnoreWarnings)
            : new Result<TOut>(issue);
    }

    /// <summary>
    /// Convert a Result{T1, T2} to a Result{TOut} by supplying a function from T1 and T2 to TOut.
    /// </summary>
    /// <param name="result">The result this function is called from.</param>
    /// <param name="valueTaskFunc">The FuncTask to calculate of the new result.</param>
    /// <typeparam name="T1">The first type of the original result.</typeparam>
    /// <typeparam name="T2">The second type of the original result.</typeparam>
    /// <typeparam name="TOut">The type of the outgoing result.</typeparam>
    /// <returns>
    /// - A failure with the original issue if the original result was a failure.
    /// - A success with the supplied value and the original IgnoreWarnings configuration.
    /// </returns>
    public static async Task<Result<TOut>> MapAsync<T1, T2, TOut>(this Result<(T1, T2)> result, Func<T1, T2, Task<TOut>> valueTaskFunc)
    {
        return result.TryGetValue(out var values, out var issue) 
            ? new Result<TOut>(await valueTaskFunc(values.Item1, values.Item2), result.IgnoreWarnings)
            : new Result<TOut>(issue);
    }

    /// <summary>
    /// Convert a Result{T1, T2} to a Result{TOut} by supplying a function from T1 and T2 to TOut.
    /// </summary>
    /// <param name="resultTask">The Task of a Result this function is called from.</param>
    /// <param name="valueFunc">The function to calculate of the new result.</param>
    /// <typeparam name="T1">The first type of the original result.</typeparam>
    /// <typeparam name="T2">The second type of the original result.</typeparam>
    /// <typeparam name="TOut">The type of the outgoing result.</typeparam>
    /// <returns>
    /// - A failure with the original issue if the original result was a failure.
    /// - A success with the supplied value and the original IgnoreWarnings configuration.
    /// </returns>
    public static async Task<Result<TOut>> Map<T1, T2, TOut>(this Task<Result<(T1, T2)>> resultTask, Func<T1, T2, TOut> valueFunc)
    {
        var result = await resultTask;
        return Map(result, valueFunc);
    }

    /// <summary>
    /// Convert a Result{T1, T2} to a Result{TOut} by supplying a function from T1 and T2 to TOut.
    /// </summary>
    /// <param name="resultTask">The Task of a Result this function is called from.</param>
    /// <param name="valueTaskFunc">The FuncTask to calculate of the new result.</param>
    /// <typeparam name="T1">The first type of the original result.</typeparam>
    /// <typeparam name="T2">The second type of the original result.</typeparam>
    /// <typeparam name="TOut">The type of the outgoing result.</typeparam>
    /// <returns>
    /// - A failure with the original issue if the original result was a failure.
    /// - A success with the supplied value and the original IgnoreWarnings configuration.
    /// </returns>
    public static async Task<Result<TOut>> MapAsync<T1, T2, TOut>(this Task<Result<(T1, T2)>> resultTask, Func<T1, T2, Task<TOut>> valueTaskFunc)
    {
        var result = await resultTask;
        return await MapAsync(result, valueTaskFunc);
    }

    #endregion
}