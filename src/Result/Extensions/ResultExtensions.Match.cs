namespace DA.Results.Extensions;
public static partial class ResultExtensions
{
    /// <summary>
    /// Match the result depending on whether the result is a success or a failure.
    /// </summary>
    /// <typeparam name="TOut">The type of the resulting action.</typeparam>
    /// <param name="result">This result to match on.</param>
    /// <param name="onSuccess">The value to return when the result is a success.</param>
    /// <param name="onFailure">The value to return when the result is a fialure.</param>
    /// <returns>The value of either the success or the failure value.</returns>
    public static TOut Match<TOut>(this Result result, TOut onSuccess, TOut onFailure) =>
        result.IsSuccess ? onSuccess : onFailure;

    /// <summary>
    /// Match the result depending on whether the result is a success or a failure.
    /// </summary>
    /// <typeparam name="TOut">The type of the resulting action.</typeparam>
    /// <param name="result">This result to match on.</param>
    /// <param name="onSuccess">The function to perform when the result is a success.</param>
    /// <param name="onFailure">The function to perform when the result is a fialure.</param>
    /// <returns>The result of either the success or the failure function.</returns>
    public static TOut Match<TOut>(this Result result, Func<TOut> onSuccess, Func<TOut> onFailure) =>
        result.IsSuccess ? onSuccess() : onFailure();

    /// <summary>
    /// Match the result depending on whether the result is a success or a failure.
    /// </summary>
    /// <typeparam name="TOut">The type of the resulting action.</typeparam>
    /// <param name="result">This result to match on.</param>
    /// <param name="onSuccess">The task to perform when the result is a success.</param>
    /// <param name="onFailure">The task to perform when the result is a fialure.</param>
    /// <returns>The result of either the success or the failure task.</returns>
    public static async Task<TOut> MatchAsync<TOut>(this Result result, Task<TOut> onSuccess, Task<TOut> onFailure) =>
        await result.Match(async () => await onSuccess, async () => await onFailure);

    /// <summary>
    /// Match the result depending on whether the result is a success or a failure.
    /// </summary>
    /// <param name="result">This result to match on.</param>
    /// <param name="onSuccess">The task to perform when the result is a success.</param>
    /// <param name="onFailure">The task to perform when the result is a fialure.</param>
    public static async Task<TOut> MatchAsync<TOut>(this Result result, Func<Task<TOut>> onSuccess, Func<Task<TOut>> onFailure) =>
        await result.MatchAsync(onSuccess(), onFailure());

    /// <summary>
    /// Match the result depending on whether the result is a success or a failure.
    /// </summary>
    /// <param name="resultTask">This task to evaluate the match on.</param>
    /// <param name="onSuccess">The task to perform when the result is a success.</param>
    /// <param name="onFailure">The task to perform when the result is a fialure.</param>
    public static async Task<TOut> MatchAsync<TOut>(this Task<Result> resultTask, Task<TOut> onSuccess, Task<TOut> onFailure)
    {
        var result = await resultTask;
        return await result.MatchAsync(onSuccess, onFailure);
    }

    /// <summary>
    /// Match the result depending on whether the result is a success or a failure.
    /// </summary>
    /// <param name="resultTask">This task to evaluate the match on.</param>
    /// <param name="onSuccess">The task to perform when the result is a success.</param>
    /// <param name="onFailure">The task to perform when the result is a fialure.</param>
    public static async Task<TOut> MatchAsync<TOut>(this Task<Result> resultTask, Func<Task<TOut>> onSuccess, Func<Task<TOut>> onFailure)
    {
        var result = await resultTask;
        return await result.MatchAsync(onSuccess(), onFailure());
    }

    /// <summary>
    /// Match the result depending on whether the result is a success or a failure.
    /// </summary>
    /// <param name="result">This result to match on.</param>
    /// <param name="onSuccess">The action to perform when the result is a success.</param>
    /// <param name="onFailure">The action to perform when the result is a fialure.</param>
    public static void Match(this Result result, Action onSuccess, Action onFailure)
    {
        if (result.IsSuccess) onSuccess();
        else onFailure();
    }

    /// <summary>
    /// Match the result depending on whether the result is a success or a failure.
    /// </summary>
    /// <param name="result">This result to match on.</param>
    /// <param name="onSuccess">The action to perform when the result is a success.</param>
    /// <param name="onFailure">The action to perform when the result is a fialure.</param>
    public static void Match(this Result result, Action<IEnumerable<string>> onSuccess, Action<IEnumerable<string>> onFailure)
    {
        if (result.IsSuccess) onSuccess(result.Messages);
        else onFailure(result.Messages);
    }

    /// <summary>
    /// Match the result depending on whether the result is a success or a failure.
    /// </summary>
    /// <param name="result">This result to match on.</param>
    /// <param name="onSuccess">The action to perform when the result is a success.</param>
    /// <param name="onFailure">The action to perform when the result is a fialure.</param>
    public static async Task<Result> MatchAsync(this Task<Result> resultTask, Action<IEnumerable<string>> onSuccess, Action<IEnumerable<string>> onFailure)
    {
        var result = await resultTask;
        result.Match(onSuccess, onFailure);
        return result;
    }

    /// <summary>
    /// Match the result depending on whether the result is a success or a failure.
    /// </summary>
    /// <param name="result">This result to match on.</param>
    /// <param name="onSuccess">The task to perform when the result is a success.</param>
    /// <param name="onFailure">The task to perform when the result is a fialure.</param>
    public static async Task MatchAsync(this Result result, Task onSuccess, Task onFailure)
    {
        await result.Match(async () => await onSuccess, async () => await onFailure);
    }

    /// <summary>
    /// Match the result depending on whether the result is a success or a failure.
    /// </summary>
    /// <param name="result">This result to match on.</param>
    /// <param name="onSuccess">The task to perform when the result is a success.</param>
    /// <param name="onFailure">The task to perform when the result is a fialure.</param>
    public static async Task MatchAsync(this Result result, Func<Task> onSuccess, Func<Task> onFailure)
    {
        await result.MatchAsync(onSuccess(), onFailure());
    }

    /// <summary>
    /// Match the result depending on whether the result is a success or a failure.
    /// </summary>
    /// <param name="resultTask">This task to evaluate the match on.</param>
    /// <param name="onSuccess">The task to perform when the result is a success.</param>
    /// <param name="onFailure">The task to perform when the result is a fialure.</param>
    public static async Task MatchAsync(this Task<Result> resultTask, Task onSuccess, Task onFailure)
    {
        var result = await resultTask;
        await result.MatchAsync(onSuccess, onFailure);
    }

    /// <summary>
    /// Match the result depending on whether the result is a success or a failure.
    /// </summary>
    /// <param name="resultTask">This task to evaluate the match on.</param>
    /// <param name="onSuccess">The task to perform when the result is a success.</param>
    /// <param name="onFailure">The task to perform when the result is a fialure.</param>
    public static async Task MatchAsync(this Task<Result> resultTask, Func<Task> onSuccess, Func<Task> onFailure)
    {
        var result = await resultTask;
        await result.MatchAsync(onSuccess(), onFailure());
    }

    /// <summary>
    /// Match the result depending on whether the result is a success or a failure.
    /// </summary>
    /// <typeparam name="TOut">The type of the resulting action.</typeparam>
    /// <param name="result">This result to match on.</param>
    /// <param name="onSuccess">The function to perform when the result is a success.</param>
    /// <param name="onFailure">The function to perform when the result is a fialure.</param>
    /// <returns>The result of either the success or the failure function.</returns>
    public static TOut Match<TIn, TOut>(this Result<TIn> result, Func<TIn, TOut> onSuccess, Func<TOut> onFailure)
    {
        if (!result.IsSuccess(out var value)) return onFailure();
        return onSuccess(value);
    }


    /// <summary>
    /// Match the result depending on whether the result is a success or a failure.
    /// </summary>
    /// <param name="resultTask">This task to evaluate the match on.</param>
    /// <param name="onSuccess">The task to perform when the result is a success.</param>
    /// <param name="onFailure">The task to perform when the result is a fialure.</param>
    public static async Task<TOut> MatchAsync<TIn, TOut>(this Task<Result<TIn>> resultTask, Func<TIn, TOut> onSuccess, Func<TOut> onFailure)
    {
        var result = await resultTask;
        return result.Match(onSuccess, onFailure);
    }
}