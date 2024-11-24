namespace DA.Results.Extensions;

/// <summary>
/// Tap:
/// Applies an Action on the Result{T}, but only if the Result is a success result.
/// </summary>
public static class TapResultExtensions
{
    #region Result without value to action
    /// <summary>
    /// Applies an Action on the Result{T}, but only if the Result is a success result.
    /// </summary>
    /// <param name="result">The result this function is called from.</param>
    /// <param name="action">The action to perform if the result is a success.</param>
    /// <typeparam name="TValue">The type of the Result.</typeparam>
    /// <returns>The original result for fluent chaining.</returns>
    public static Result<TValue> Tap<TValue>(this Result<TValue> result, Action action)
    {
        if (result.IsSuccess)
        {
            action();
        }
        return result;
    }

    /// <summary>
    /// Applies an Action on the Result{T}, but only if the Result is a success result.
    /// </summary>
    /// <param name="result">The result this function is called from.</param>
    /// <param name="task">The task to perform if the result is a success.</param>
    /// <typeparam name="TValue">The type of the Result.</typeparam>
    /// <returns>The original result for fluent chaining.</returns>
    public static async Task<Result<TValue>> TapAsync<TValue>(this Result<TValue> result, Func<Task> task)
    {
        if (result.IsSuccess)
            await task();
        return result;
    }

    /// <summary>
    /// Applies an Action on the Result{T}, but only if the Result is a success result.
    /// </summary>
    /// <param name="resultTask">The Task of a Result this function is called from.</param>
    /// <param name="action">The action to perform if the result is a success.</param>
    /// <typeparam name="TValue">The type of the Result.</typeparam>
    /// <returns>The original result for fluent chaining.</returns>
    public static async Task<Result<TValue>> Tap<TValue>(this Task<Result<TValue>> resultTask, Action action)
    {
        var result = await resultTask;
        return result.Tap(action);
    }

    /// <summary>
    /// Applies an Action on the Result{T}, but only if the Result is a success result.
    /// </summary>
    /// <param name="resultTask">The Task of a Result this function is called from.</param>
    /// <param name="task">The task to perform if the result is a success.</param>
    /// <typeparam name="TValue">The type of the Result.</typeparam>
    /// <returns>The original result for fluent chaining.</returns>
    public static async Task<Result<TValue>> TapAsync<TValue>(this Task<Result<TValue>> resultTask, Func<Task> task)
    {
        var result = await resultTask;
        return await result.TapAsync(task);
    }

    #endregion

    #region Result{T} to action(T)

    /// <summary>
    /// Applies an Action{T} on the Result{T}, but only if the Result is a success result.
    /// </summary>
    /// <param name="result">The result this function is called from.</param>
    /// <param name="action">The action to perform if the result is a success.</param>
    /// <typeparam name="TValue">The type of the Result.</typeparam>
    /// <returns>The original result for fluent chaining.</returns>
    public static Result<TValue> Tap<TValue>(this Result<TValue> result, Action<TValue> action)
    {
        if (result.TryGetValue(out var value))
        {
            action(value);
        }

        return result;
    }

    /// <summary>
    /// Applies an Action{T} on the Result{T}, but only if the Result is a success result.
    /// </summary>
    /// <param name="result">The result this function is called from.</param>
    /// <param name="actionTask">The Task to perform if the result is a success.</param>
    /// <typeparam name="TValue">The type of the Result.</typeparam>
    /// <returns>The original result for fluent chaining.</returns>
    public static async Task<Result<TValue>> TapAsync<TValue>(this Result<TValue> result, Func<TValue, Task> actionTask)
    {
        if (result.TryGetValue(out var value))
        {
            await actionTask(value);
        }

        return result;
    }

    /// <summary>
    /// Applies an Action{T} on the Result{T}, but only if the Result is a success result.
    /// </summary>
    /// <param name="resultTask">The Task of a Result this function is called from.</param>
    /// <param name="action">The action to perform if the result is a success.</param>
    /// <typeparam name="TValue">The type of the Result.</typeparam>
    /// <returns>The original result for fluent chaining.</returns>
    public static async Task<Result<TValue>> Tap<TValue>(this Task<Result<TValue>> resultTask, Action<TValue> action)
    {
        var result = await resultTask;
        return result.Tap(action);
    }
    
    /// <summary>
    /// Applies an Action{T} on the Result{T}, but only if the Result is a success result.
    /// </summary>
    /// <param name="resultTask">The Task of a Result this function is called from.</param>
    /// <param name="actionTask">The Task to perform if the result is a success.</param>
    /// <typeparam name="TValue">The type of the Result.</typeparam>
    /// <returns>The original result for fluent chaining.</returns>
    public static async Task<Result<TValue>> TapAsync<TValue>(this Task<Result<TValue>> resultTask, Func<TValue, Task> actionTask)
    {
        var result = await resultTask;
        return await result.TapAsync(actionTask);
    }
    #endregion

    #region Result{(T1,T2)} to action(T1,T2)
    
    /// <summary>
    /// Applies an Action{T1, T2} on the Result{T1, T2}, but only if the Result is a success result.
    /// </summary>
    /// <param name="result">The result this function is called from.</param>
    /// <param name="action">The action to perform if the result is a success.</param>
    /// <typeparam name="T1">The first type of the Result.</typeparam>
    /// <typeparam name="T2">The second type of the Result.</typeparam>
    /// <returns>The original result for fluent chaining.</returns>
    public static Result<(T1, T2)> Tap<T1, T2>(this Result<(T1, T2)> result, Action<T1, T2> action)
    {
        if (result.TryGetValue(out var value))
        {
            action(value.Item1, value.Item2);
        }
        return result;
    }

    /// <summary>
    /// Applies an Action{T1, T2} on the Result{T1, T2}, but only if the Result is a success result.
    /// </summary>
    /// <param name="result">The result this function is called from.</param>
    /// <param name="actionTask">The Task to perform if the result is a success.</param>
    /// <typeparam name="T1">The first type of the Result.</typeparam>
    /// <typeparam name="T2">The second type of the Result.</typeparam>
    /// <returns>The original result for fluent chaining.</returns>
    public static async Task<Result<(T1, T2)>> TapAsync<T1, T2>(this Result<(T1, T2)> result, Func<T1, T2, Task> actionTask)
    {
        if (result.TryGetValue(out var value))
        {
            await actionTask(value.Item1, value.Item2);
        }
        return result;
    }

    /// <summary>
    /// Applies an Action{T1, T2} on the Result{T1, T2}, but only if the Result is a success result.
    /// </summary>
    /// <param name="resultTask">The Task of a Result this function is called from.</param>
    /// <param name="action">The action to perform if the result is a success.</param>
    /// <typeparam name="T1">The first type of the Result.</typeparam>
    /// <typeparam name="T2">The second type of the Result.</typeparam>
    /// <returns>The original result for fluent chaining.</returns>
    public static async Task<Result<(T1, T2)>> Tap<T1, T2>(this Task<Result<(T1, T2)>> resultTask, Action<T1, T2> action)
    {
        var result = await resultTask;
        return result.Tap(action);
    }

    /// <summary>
    /// Applies an Action{T1, T2} on the Result{T1, T2}, but only if the Result is a success result.
    /// </summary>
    /// <param name="resultTask">The Task of a Result this function is called from.</param>
    /// <param name="actionTask">The Task to perform if the result is a success.</param>
    /// <typeparam name="T1">The first type of the Result.</typeparam>
    /// <typeparam name="T2">The second type of the Result.</typeparam>
    /// <returns>The original result for fluent chaining.</returns>
    public static async Task<Result<(T1, T2)>> TapAsync<T1, T2>(this Task<Result<(T1, T2)>> resultTask, Func<T1, T2, Task> actionTask)
    {
        var result = await resultTask;
        return await result.TapAsync(actionTask);
    }
    #endregion
}