namespace DA.Results.Extensions;

/// <summary>
/// Bind: Convert a Result{TIn} to a Result{TOut} by supplying a Result{TOut}.
/// It returns:
/// - A failure with the original issue if the original result was a failure.
/// - A failure with the second issue if the second result was a failure.
/// - A success with the second value and the original IgnoreWarnings configuration.
/// </summary>
public static class BindResultExtensions
{
    /// <summary>
    /// Bind a second result to the first result.
    /// </summary>
    /// <param name="first">The result where bind is called.</param>
    /// <param name="second">The second result that is bind to the first.</param>
    /// <typeparam name="TIn">The type of the original result.</typeparam>
    /// <typeparam name="TOut">The type of the incoming result.</typeparam>
    /// <returns>
    /// A new result{<typeparam name="TOut"/>} that has:
    /// - A failure with the original issue if the original result was a failure.
    /// - A failure with the second issue if the second result was a failure.
    /// - A success with the second value and the original IgnoreWarnings configuration.
    /// </returns>
    private static Result<TOut> Bind<TIn, TOut>(this Result<TIn> first, Result<TOut> second)
    {
        return first.TryGetIssue(out var firstIssue) switch
        {
            true => new Result<TOut>(firstIssue),
            false => second.TryGetValue(out var secondValue) 
                ? new Result<TOut>(secondValue, first.IgnoreWarnings)
                : second
        };
    }

    /// <summary>
    /// Bind a function to produces a second result to the first result.
    /// </summary>
    /// <param name="first">The result where bind is called.</param>
    /// <param name="secondFunc">Function to create the second result.</param>
    /// <typeparam name="TIn">The type of the original result.</typeparam>
    /// <typeparam name="TOut">The type of the incoming result.</typeparam>
    /// <returns>
    /// A new result{<typeparam name="TOut"/>} that has:
    /// - A failure with the original issue if the original result was a failure.
    /// - A failure with the second issue if the second result was a failure.
    /// - A success with the second value and the original IgnoreWarnings configuration.
    /// </returns>
    public static Result<TOut> Bind<TIn, TOut>(this Result<TIn> first, Func<TIn, Result<TOut>> secondFunc)
    {
        if (!first.TryGetValue(out var value, out var issue))
        {
            return new Result<TOut>(issue);
        }
        var second = secondFunc(value);
        return first.Bind(second);
    }

    /// <summary>
    /// Bind a task that produces a second result to the first result.
    /// </summary>
    /// <param name="first">The result where bind is called.</param>
    /// <param name="secondTask">Task to create the second result.</param>
    /// <typeparam name="TIn">The type of the original result.</typeparam>
    /// <typeparam name="TOut">The type of the incoming result.</typeparam>
    /// <returns>
    /// A new result{<typeparam name="TOut"/>} that has:
    /// - A failure with the original issue if the original result was a failure.
    /// - A failure with the second issue if the second result was a failure.
    /// - A success with the second value and the original IgnoreWarnings configuration.
    /// </returns>
    public static async Task<Result<TOut>> BindAsync<TIn, TOut>(this Result<TIn> first, Func<Task<Result<TOut>>> secondTask)
    {
        if (first.TryGetIssue(out var issue))
        {
            return new Result<TOut>(issue);
        }
        var second = await secondTask();
        return first.Bind(second);
    }

    /// <summary>
    /// Bind a function that produces (asynchronously) a second result to the first result.
    /// </summary>
    /// <param name="first">The result where bind is called.</param>
    /// <param name="secondTaskFunc">FuncTask to create the second result.</param>
    /// <typeparam name="TIn">The type of the original result.</typeparam>
    /// <typeparam name="TOut">The type of the incoming result.</typeparam>
    /// <returns>
    /// A new result{<typeparam name="TOut"/>} that has:
    /// - A failure with the original issue if the original result was a failure.
    /// - A failure with the second issue if the second result was a failure.
    /// - A success with the second value and the original IgnoreWarnings configuration.
    /// </returns>
    public static async Task<Result<TOut>> BindAsync<TIn, TOut>(this Result<TIn> first, Func<TIn, Task<Result<TOut>>> secondTaskFunc)
    {
        if (!first.TryGetValue(out var value, out var issue))
        {
            return new Result<TOut>(issue);
        }
        var second = await secondTaskFunc(value);
        return first.Bind(second);
    }

    /// <summary>
    /// Bind a function to produces a second result to the first result.
    /// </summary>
    /// <param name="firstTask">The task of a result where bind is called.</param>
    /// <param name="secondFunc">Function to create the second result.</param>
    /// <typeparam name="TIn">The type of the original result.</typeparam>
    /// <typeparam name="TOut">The type of the incoming result.</typeparam>
    /// <returns>
    /// A new result{<typeparam name="TOut"/>} that has:
    /// - A failure with the original issue if the original result was a failure.
    /// - A failure with the second issue if the second result was a failure.
    /// - A success with the second value and the original IgnoreWarnings configuration.
    /// </returns>
    public static async Task<Result<TOut>> Bind<TIn, TOut>(this Task<Result<TIn>> firstTask, Func<TIn, Result<TOut>> secondFunc)
    {
        var first = await firstTask;
        return Bind(first, secondFunc);
    }

    /// <summary>
    /// Bind a task that produces a second result to the first result.
    /// </summary>
    /// <param name="firstTask">The task of a result where bind is called.</param>
    /// <param name="secondTask">Task to create the second result.</param>
    /// <typeparam name="TIn">The type of the original result.</typeparam>
    /// <typeparam name="TOut">The type of the incoming result.</typeparam>
    /// <returns>
    /// A new result{<typeparam name="TOut"/>} that has:
    /// - A failure with the original issue if the original result was a failure.
    /// - A failure with the second issue if the second result was a failure.
    /// - A success with the second value and the original IgnoreWarnings configuration.
    /// </returns>
    public static async Task<Result<TOut>> BindAsync<TIn, TOut>(this Task<Result<TIn>> firstTask, Func<Task<Result<TOut>>> secondTask)
    {
        var first = await firstTask;
        return await BindAsync(first, secondTask);
    }

    /// <summary>
    /// Bind a function that produces (asynchronously) a second result to the first result.
    /// </summary>
    /// <param name="firstTask">The task of a result where bind is called.</param>
    /// <param name="secondTaskFunc">FuncTask to create the second result.</param>
    /// <typeparam name="TIn">The type of the original result.</typeparam>
    /// <typeparam name="TOut">The type of the incoming result.</typeparam>
    /// <returns>
    /// A new result{<typeparam name="TOut"/>} that has:
    /// - A failure with the original issue if the original result was a failure.
    /// - A failure with the second issue if the second result was a failure.
    /// - A success with the second value and the original IgnoreWarnings configuration.
    /// </returns>
    public static async Task<Result<TOut>> BindAsync<TIn, TOut>(this Task<Result<TIn>> firstTask, Func<TIn, Task<Result<TOut>>> secondTaskFunc)
    {
        var first = await firstTask;
        return await BindAsync(first, secondTaskFunc);
    }
}