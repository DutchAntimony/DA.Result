namespace DA.Results.Extensions;

public static class CheckForEachResultExtensions
{
    /// <summary>
    /// Check for each entry of an enumeration if the content is valid given the value of the result this method is called on. 
    /// </summary>
    /// <param name="result">The result where check for each is called.</param>
    /// <param name="enumeration">The enumeration of which the items must be checked.</param>
    /// <param name="predicate">
    /// The predicate that determines if a value is valid.
    /// This is a function that takes in every element of the enumeration, as well as the value of the result
    /// And produces a result that is then checked.
    /// </param>
    /// <typeparam name="TValue">The type of the value of the result.</typeparam>
    /// <typeparam name="TEnumeration">The type of the enumeration.</typeparam>
    /// <returns>
    /// A Result{<typeparam name="TValue"/>} that is:
    /// - The original failure result.
    /// - A failure if the predicate fails for one of the elements in the enumeration.
    /// - The original success result.
    /// </returns>
    public static Result<TValue> CheckForEach<TValue, TEnumeration>(
        this Result<TValue> result, 
        IEnumerable<TEnumeration> enumeration,
        Func<TEnumeration, TValue, IResult> predicate)
    {
        foreach (var item in enumeration)
        {
            if (!result.TryGetValue(out var value))
            {
                break; // if at any moment the result is Failure, don't do more checks.
            }
            var itemResult = predicate(item, value);
            result = result.Check(itemResult);
        }
        return result;
    }
    
    /// <summary>
    /// Check for each entry of an enumeration if the content is valid given the value of the result this method is called on. 
    /// </summary>
    /// <param name="result">The result where check for each is called.</param>
    /// <param name="enumeration">The enumeration of which the items must be checked.</param>
    /// <param name="predicateTask">
    /// The Task of a predicate that determines if a value is valid.
    /// This is a function that takes in every element of the enumeration, as well as the value of the result
    /// And produces a result that is then checked.
    /// </param>
    /// <typeparam name="TValue">The type of the value of the result.</typeparam>
    /// <typeparam name="TEnumeration">The type of the enumeration.</typeparam>
    /// <returns>
    /// A Result{<typeparam name="TValue"/>} that is:
    /// - The original failure result.
    /// - A failure if the predicate fails for one of the elements in the enumeration.
    /// - The original success result.
    /// </returns>
    public static async Task<Result<TValue>> CheckForEachAsync<TValue, TEnumeration>(
        this Result<TValue> result, 
        IEnumerable<TEnumeration> enumeration, 
        Func<TEnumeration, TValue, Task<IResult>> predicateTask)
    {
        foreach (var item in enumeration)
        {
            if (!result.TryGetValue(out var value))
                break; // if at any moment the result is Failure, don't do more checks.
            var itemResult = await predicateTask(item, value);
            result = result.Check(itemResult);
        }
        return result;
    }
    
    /// <summary>
    /// Check for each entry of an enumeration if the content is valid given the value of the result this method is called on. 
    /// </summary>
    /// <param name="resultTask">The Task of a result where check for each is called.</param>
    /// <param name="enumeration">The enumeration of which the items must be checked.</param>
    /// <param name="predicate">
    /// The predicate that determines if a value is valid.
    /// This is a function that takes in every element of the enumeration, as well as the value of the result
    /// And produces a result that is then checked.
    /// </param>
    /// <typeparam name="TValue">The type of the value of the result.</typeparam>
    /// <typeparam name="TEnumeration">The type of the enumeration.</typeparam>
    /// <returns>
    /// A Result{<typeparam name="TValue"/>} that is:
    /// - The original failure result.
    /// - A failure if the predicate fails for one of the elements in the enumeration.
    /// - The original success result.
    /// </returns>
    public static async Task<Result<TValue>> CheckForEach<TValue, TEnumeration>(
        this Task<Result<TValue>> resultTask, 
        IEnumerable<TEnumeration> enumeration, 
        Func<TEnumeration, TValue, IResult> predicate)
    {
        var result = await resultTask;
        return CheckForEach(result, enumeration, predicate);
    }
    
    /// <summary>
    /// Check for each entry of an enumeration if the content is valid given the value of the result this method is called on. 
    /// </summary>
    /// <param name="resultTask">The Task of a result where check for each is called.</param>
    /// <param name="enumeration">The enumeration of which the items must be checked.</param>
    /// <param name="predicateTask">
    /// The predicate task that determines if a value is valid.
    /// This is a function that takes in every element of the enumeration, as well as the value of the result
    /// And produces a result that is then checked.
    /// </param>
    /// <typeparam name="TValue">The type of the value of the result.</typeparam>
    /// <typeparam name="TEnumeration">The type of the enumeration.</typeparam>
    /// <returns>
    /// A Result{<typeparam name="TValue"/>} that is:
    /// - The original failure result.
    /// - A failure if the predicate fails for one of the elements in the enumeration.
    /// - The original success result.
    /// </returns>
    public static async Task<Result<TValue>> CheckForEachAsync<TValue, TEnumeration>(
        this Task<Result<TValue>> resultTask, IEnumerable<TEnumeration> enumeration, Func<TEnumeration, TValue, Task<IResult>> predicateTask)
    {
        var result = await resultTask;
        return await CheckForEachAsync(result, enumeration, predicateTask);
    }
    
    /// <summary>
    /// Check for each entry of the enumeration result  if the content is valid. 
    /// </summary>
    /// <param name="result">The result{IEnumerable{<typeparam name="TValue"/>}} where this extension method is called from.</param>
    /// <param name="predicate">
    /// The predicate that determines if a value is valid.
    /// This is a function that takes every element of the enumeration and checks whether it is valid.
    /// </param>
    /// <typeparam name="TValue">The type of the enumeration encapsulated in the result.</typeparam>
    /// <returns>
    /// A Result{<typeparam name="TValue"/>} that is:
    /// - The original failure result.
    /// - A failure if the predicate fails for one of the elements in the enumeration.
    /// - The original success result.
    /// </returns>
    public static Result<IEnumerable<TValue>> CheckForEach<TValue>(
        this Result<IEnumerable<TValue>> result,
        Func<TValue, IResult> predicate)
    {
        if (!result.TryGetValue(out var collection))
        {
            return result;
        }

        foreach (var item in collection)
        {
            var innerResult = predicate(item);
            if (!innerResult.IsSuccess)
                return result.Check(innerResult);
        }
        
        return result;
    }

    /// <summary>
    /// Check for each entry of the enumeration result  if the content is valid. 
    /// </summary>
    /// <param name="result">The result{IEnumerable{<typeparam name="TValue"/>}} where this extension method is called from.</param>
    /// <param name="predicateTask">
    /// The task with a predicate that determines if a value is valid.
    /// This is a function that takes every element of the enumeration and checks whether it is valid.
    /// </param>
    /// <typeparam name="TValue">The type of the enumeration encapsulated in the result.</typeparam>
    /// <returns>
    /// A Result{<typeparam name="TValue"/>} that is:
    /// - The original failure result.
    /// - A failure if the predicate fails for one of the elements in the enumeration.
    /// - The original success result.
    /// </returns>
    public static async Task<Result<IEnumerable<TValue>>> CheckForEachAsync<TValue>(this Result<IEnumerable<TValue>> result, Func<TValue, Task<IResult>> predicateTask)
    {
        if (!result.TryGetValue(out var collection))
        {
            return result;
        }

        foreach (var item in collection)
        {
            var innerResult = await predicateTask(item);
            if (!innerResult.IsSuccess)
                return result.Check(innerResult);
        }
        
        return result;
    }
    
    /// <summary>
    /// Check for each entry of the enumeration result  if the content is valid. 
    /// </summary>
    /// <param name="resultTask">The task of the result{IEnumerable{<typeparam name="TValue"/>}} where this extension method is called from.</param>
    /// <param name="predicate">
    /// The predicate that determines if a value is valid.
    /// This is a function that takes every element of the enumeration and checks whether it is valid.
    /// </param>
    /// <typeparam name="TValue">The type of the enumeration encapsulated in the result.</typeparam>
    /// <returns>
    /// A Result{<typeparam name="TValue"/>} that is:
    /// - The original failure result.
    /// - A failure if the predicate fails for one of the elements in the enumeration.
    /// - The original success result.
    /// </returns>
    public static async Task<Result<IEnumerable<TValue>>> CheckForEach<TValue>(this Task<Result<IEnumerable<TValue>>> resultTask, Func<TValue, IResult> predicate)
    {
        var result = await resultTask;
        return result.CheckForEach(predicate);
    }
    
    /// <summary>
    /// Check for each entry of the enumeration result  if the content is valid. 
    /// </summary>
    /// <param name="resultTask">The Task of a result{IEnumerable{<typeparam name="TValue"/>}} where this extension method is called from.</param>
    /// <param name="predicateTask">
    /// The task with a predicate that determines if a value is valid.
    /// This is a function that takes every element of the enumeration and checks whether it is valid.
    /// </param>
    /// <typeparam name="TValue">The type of the enumeration encapsulated in the result.</typeparam>
    /// <returns>
    /// A Result{<typeparam name="TValue"/>} that is:
    /// - The original failure result.
    /// - A failure if the predicate fails for one of the elements in the enumeration.
    /// - The original success result.
    /// </returns>
    public static async Task<Result<IEnumerable<TValue>>> CheckForEachAsync<TValue>(
        this Task<Result<IEnumerable<TValue>>> resultTask, 
        Func<TValue, Task<IResult>> predicateTask)
    {
        var result = await resultTask;
        return await result.CheckForEachAsync(predicateTask);
    }
}