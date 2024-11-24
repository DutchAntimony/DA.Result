namespace DA.Results.Extensions;

/// <summary>
/// Compensate:
/// If the current result is invalid, compensate by generating an alternative result.
/// Looses information about the current failure.
/// </summary>
public static class CompensateResultExtensions
{
    /// <summary>
    /// If the current result is invalid, compensate by generating an alternative result.
    /// Looses information about the current failure.
    /// </summary>
    /// <param name="result">The result this function is called from.</param>
    /// <param name="compensator">The function to generate an alternative result if the current result is a failure.</param>
    /// <typeparam name="TValue">The type of the result that is compensated.</typeparam>
    /// <returns>The original result if that is a success, or the compensated result if not.</returns>
    public static Result<TValue> Compensate<TValue>(this Result<TValue> result, Func<Result<TValue>> compensator) => 
        result.IsSuccess ? result : compensator();

    /// <summary>
    /// If the current result is invalid, compensate by generating an alternative result.
    /// Looses information about the current failure.
    /// </summary>
    /// <param name="result">The result this function is called from.</param>
    /// <param name="compensator">The Task to generate an alternative result if the current result is a failure.</param>
    /// <typeparam name="TValue">The type of the result that is compensated.</typeparam>
    /// <returns>The original result if that is a success, or the compensated result if not.</returns>
    public static async Task<Result<TValue>> CompensateAsync<TValue>(this Result<TValue> result, Func<Task<Result<TValue>>> compensator) =>
        result.IsSuccess ? result : await compensator();

    /// <summary>
    /// If the current result is invalid, compensate by generating an alternative result.
    /// Looses information about the current failure.
    /// </summary>
    /// <param name="resultTask">The Task of a Result this function is called from.</param>
    /// <param name="compensator">The function to generate an alternative result if the current result is a failure.</param>
    /// <typeparam name="TValue">The type of the result that is compensated.</typeparam>
    /// <returns>The original result if that is a success, or the compensated result if not.</returns>
    public static async Task<Result<TValue>> Compensate<TValue>(this Task<Result<TValue>> resultTask, Func<Result<TValue>> compensator)
    {
        var result = await resultTask;
        return result.Compensate(compensator);
    }

    /// <summary>
    /// If the current result is invalid, compensate by generating an alternative result.
    /// Looses information about the current failure.
    /// </summary>
    /// <param name="resultTask">The Task of a Result this function is called from.</param>
    /// <param name="compensator">The Task to generate an alternative result if the current result is a failure.</param>
    /// <typeparam name="TValue">The type of the result that is compensated.</typeparam>
    /// <returns>The original result if that is a success, or the compensated result if not.</returns>
    public static async Task<Result<TValue>> CompensateAsync<TValue>(this Task<Result<TValue>> resultTask, Func<Task<Result<TValue>>> compensator)
    {
        var result = await resultTask;
        return await result.CompensateAsync(compensator);
    }
}