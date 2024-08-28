using DA.Options;
using DA.Results.ResultTypes.Failure;
using DA.Results.ResultTypes.Success;

namespace DA.Results;

// Static part of the Result record to initialise Results more easily and in a unified way.
public partial record Result
{
    #region --- Ok ---
    /// <summary>
    /// Create an Ok result, indicating that the request succeeded.
    /// </summary>
    public static Result Ok() => new(new SuccessResultType());

    /// <summary>
    /// Create an Ok result, indicating that the request succeeded.
    /// </summary>
    public static Result Ok(string message) => new(new SuccessResultType(), message);

    /// <summary>
    /// Create an Ok result, indicating that the request succeeded.
    /// </summary>
    public static Result<TValue> Ok<TValue>(TValue value) => new(value, new SuccessResultType());

    /// <summary>
    /// Create an Ok result, indicating that the request succeeded.
    /// </summary>
    public static Result<TValue> Ok<TValue>(TValue value, string message) => new(value, new SuccessResultType(), message);
    #endregion

    #region --- OkIf ---
    /// <summary>
    /// Create an Ok result if the provided condition is true.
    /// </summary>
    /// <param name="condition">The condition to check</param>
    /// <param name="error">The error message if the provided condition is false.</param>
    public static Result OkIf(bool condition, string failureMessage) =>
        condition
        ? Ok()
        : Failure(failureMessage);

    /// <summary>
    /// Create an Ok result if the provided condition is true.
    /// </summary>
    /// <param name="condition">The condition to check</param>
    /// <param name="error">The error message if the provided condition is false.</param>
    public static Result OkIf(Func<bool> condition, string failureMessage) =>
        condition()
        ? Ok()
        : Failure(failureMessage);

    /// <summary>
    /// Create an Ok result if the provided condition is true.
    /// </summary>
    /// <param name="condition">The condition to check</param>
    /// <param name="error">The error message if the provided condition is false.</param>
    public static async Task<Result> OkIfAsync(Task<bool> condition, string failureMessage) =>
        await condition 
        ? Ok()
        : Failure(failureMessage);

    /// <summary>
    /// Create an Ok result if the provided condition is true.
    /// </summary>
    /// <param name="condition">The condition to check</param>
    /// <param name="error">The error message if the provided condition is false.</param>
    public static async Task<Result> OkIfAsync(Func<Task<bool>> condition, string failureMessage) =>
        await condition() 
        ? Ok()
        : Failure(failureMessage);

    /// <summary>
    /// Create an Ok result around the provided value if the provided condition is true.
    /// </summary>
    /// <param name="value">The provided value</param>
    /// <param name="condition">The condition to check</param>
    /// <param name="error">The error message if the provided condition is false.</param>
    public static Result<TValue> OkIf<TValue>(TValue value, bool condition, string failureMessage) => 
        condition 
        ? Ok(value) 
        : Failure<TValue>(failureMessage);

    /// <summary>
    /// Create an Ok result around the provided value if the provided condition is true.
    /// </summary>
    /// <param name="value">The provided value</param>
    /// <param name="condition">The condition to check</param>
    /// <param name="error">The error message if the provided condition is false.</param>
    public static Result<TValue> OkIf<TValue>(TValue value, Func<TValue, bool> condition, string failureMessage) => 
        condition(value) 
        ? Ok(value)
        : Failure<TValue>(failureMessage);

    /// <summary>
    /// Create an Ok result if the provided condition is true.
    /// </summary>
    /// <param name="condition">The condition to check</param>
    /// <param name="error">The error message if the provided condition is false.</param>
    public static async Task<Result<TValue>> OkIfAsync<TValue>(TValue value, Task<bool> condition, string failureMessage) =>
        await condition
        ? Ok(value)
        : Failure<TValue>(failureMessage);

    /// <summary>
    /// Create an Ok result if the provided condition is true.
    /// </summary>
    /// <param name="condition">The condition to check</param>
    /// <param name="error">The error message if the provided condition is false.</param>
    public static async Task<Result<TValue>> OkIfAsync<TValue>(TValue value, Func<TValue, Task<bool>> condition, string failureMessage) => 
        await condition(value) 
        ? Ok(value) 
        : Failure<TValue>(failureMessage);

    #endregion

    #region --- OkIfFound ---
    /// <summary>
    /// Create an Ok result if the entity is found for the provided key.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TEntityKey">The type of the key matching with the entity.</typeparam>
    /// <param name="entity">The found entity, as an option.</param>
    /// <param name="key">The key by which the entity was searched.</param>
    /// <returns>
    /// A result with status <see cref="SuccessResultType"/> if the entity was found.
    /// A result with status <see cref="NotFoundResultType{TEntityKey}"/> if the entity was not found.
    /// </returns>
    public static Result<TEntity> OkIfFound<TEntity, TEntityKey>(Option<TEntity> entity, TEntityKey key)
        where TEntityKey : notnull =>
        entity.TryGetValue(out var value)
        ? Ok(value)
        : NotFound<TEntity, TEntityKey>(key);

    #endregion

    #region --- Failure ---
    /// <summary>
    /// Create a generic failure result, indicating that the request failed.
    /// </summary>
    public static Result Failure() => new(new FailureResultType());

    /// <summary>
    /// Create a generic failure result, indicating that the request failed.
    /// </summary>
    public static Result Failure(string message) => new(new FailureResultType(), message);

    /// <summary>
    /// Create a generic failure result, indicating that the request failed.
    /// </summary>
    public static Result<TValue> Failure<TValue>() => new(new FailureResultType());

    /// <summary>
    /// Create a generic failure result, indicating that the request failed.
    /// </summary>
    public static Result<TValue> Failure<TValue>(string message) => new(new FailureResultType(), message);

    /// <summary>
    /// Create a generic failure result, indicating that the request failed.
    /// </summary>
    public static Result<TValue> Failure<TValue>(TValue _) => new(new FailureResultType());

    /// <summary>
    /// Create a generic failure result, indicating that the request failed.
    /// </summary>
    public static Result<TValue> Failure<TValue>(TValue _, string message) => new(new FailureResultType(), message);
    #endregion

    #region --- Invalid ---
    /// <summary>
    /// Create a new result indicating that the validation of the request failed.
    /// </summary>
    /// <param name="property">Name of the property that failed validation.</param>
    /// <param name="message">Reason the property failed validation.</param>
    public static Result Invalid(string property, string message) =>
        new(new InvalidResultType(property, message));

    /// <summary>
    /// Create a new result indicating that the validation of the request failed.
    /// </summary>
    /// <param name="validationErrors">Collection with property names and messages of the failing properties.</param>
    public static Result Invalid(Dictionary<string, string> validationErrors) =>
        new(new InvalidResultType(InvalidResultType.ValidationInfo.FromDictionary(validationErrors)));

    /// <summary>
    /// Create a new result indicating that the validation of the request failed.
    /// </summary>
    /// <param name="property">Name of the property that failed validation.</param>
    /// <param name="message">Reason the property failed validation.</param>
    public static Result<TValue> Invalid<TValue>(string property, string message) =>
        new(new InvalidResultType(property, message));

    /// <summary>
    /// Create a new result indicating that the validation of the request failed.
    /// </summary>
    /// <param name="validationErrors">Collection with property names and messages of the failing properties.</param>
    public static Result<TValue> Invalid<TValue>(Dictionary<string, string> validationErrors) =>
        new(new InvalidResultType(InvalidResultType.ValidationInfo.FromDictionary(validationErrors)));

    /// <summary>
    /// Create a new result indicating that the validation of the request failed.
    /// </summary>
    /// <param name="property">Name of the property that failed validation.</param>
    /// <param name="message">Reason the property failed validation.</param>
    public static Result<TValue> Invalid<TValue>(TValue _, string property, string message) =>
        new(new InvalidResultType(property, message));

    /// <summary>
    /// Create a new result indicating that the validation of the request failed.
    /// </summary>
    /// <param name="validationErrors">Collection with property names and messages of the failing properties.</param>
    public static Result<TValue> Invalid<TValue>(TValue _, Dictionary<string, string> validationErrors) =>
        new(new InvalidResultType(InvalidResultType.ValidationInfo.FromDictionary(validationErrors)));
    #endregion

    #region --- InvalidIf --- 
    /// <summary>
    /// Create a new result indicating that the validation of the request failed if a given predicate passes.
    /// </summary>
    /// <param name="predicate">Boolean indicating if the result is indeed invalid.</param>
    /// <param name="property">Name of the property that failed validation.</param>
    /// <param name="message">Reason the property failed validation.</param>
    /// <returns>New result that is either Ok or Invalid depending on the predicate.</returns>
    public static Result InvalidIf(bool predicate, string property, string message) =>
        predicate
        ? new(new InvalidResultType(property, message))
        : Ok();

    /// <summary>
    /// Create a new result indicating that the validation of the request failed if a given predicate passes.
    /// </summary>
    /// <param name="predicate">Boolean indicating if the result is indeed invalid.</param>
    /// <param name="property">Name of the property that failed validation.</param>
    /// <param name="message">Reason the property failed validation.</param>
    /// <returns>New result that is either Ok or Invalid depending on the predicate.</returns>
    public static Result InvalidIf(Func<bool> predicate, string property, string message) =>
        predicate()
        ? new(new InvalidResultType(property, message))
        : Ok();

    /// <summary>
    /// Create a new result indicating that the validation of the request failed if a given predicate passes.
    /// </summary>
    /// <param name="predicate">Boolean indicating if the result is indeed invalid.</param>
    /// <param name="property">Name of the property that failed validation.</param>
    /// <param name="message">Reason the property failed validation.</param>
    /// <returns>New result that is either Ok or Invalid depending on the predicate.</returns>
    public static async Task<Result> InvalidIfAsync(Task<bool> predicate, string property, string message) =>
        await predicate
        ? new(new InvalidResultType(property, message))
        : Ok();

    /// <summary>
    /// Create a new result indicating that the validation of the request failed if a given predicate passes.
    /// </summary>
    /// <param name="predicate">Boolean indicating if the result is indeed invalid.</param>
    /// <param name="property">Name of the property that failed validation.</param>
    /// <param name="message">Reason the property failed validation.</param>
    /// <returns>New result that is either Ok or Invalid depending on the predicate.</returns>
    public static async Task<Result> InvalidIfAsync(Func<Task<bool>> predicate, string property, string message) =>
        await predicate()
        ? new(new InvalidResultType(property, message))
        : Ok();

    /// <summary>
    /// Create a new result indicating that the validation of the request failed if a given predicate passes.
    /// </summary>
    /// <param name="predicate">Boolean indicating if the result is indeed invalid.</param>
    /// <param name="property">Name of the property that failed validation.</param>
    /// <param name="message">Reason the property failed validation.</param>
    /// <returns>New result that is either Ok or Invalid depending on the predicate.</returns>
    public static Result<TValue> InvalidIf<TValue>(TValue value, bool predicate, string property, string message) =>
        predicate
        ? new(new InvalidResultType(property, message))
        : Ok(value);

    /// <summary>
    /// Create a new result indicating that the validation of the request failed if a given predicate passes.
    /// </summary>
    /// <param name="predicate">Boolean indicating if the result is indeed invalid.</param>
    /// <param name="property">Name of the property that failed validation.</param>
    /// <param name="message">Reason the property failed validation.</param>
    /// <returns>New result that is either Ok or Invalid depending on the predicate.</returns>
    public static Result<TValue> InvalidIf<TValue>(TValue value, Func<TValue, bool> predicate, string property, string message) =>
        predicate(value)
        ? new(new InvalidResultType(property, message))
        : Ok(value);

    /// <summary>
    /// Create a new result indicating that the validation of the request failed if a given predicate passes.
    /// </summary>
    /// <param name="predicate">Boolean indicating if the result is indeed invalid.</param>
    /// <param name="property">Name of the property that failed validation.</param>
    /// <param name="message">Reason the property failed validation.</param>
    /// <returns>New result that is either Ok or Invalid depending on the predicate.</returns>
    public static async Task<Result<TValue>> InvalidIfAsync<TValue>(TValue value, Task<bool> predicate, string property, string message) =>
        await predicate
        ? new(new InvalidResultType(property, message))
        : Ok(value);

    /// <summary>
    /// Create a new result indicating that the validation of the request failed if a given predicate passes.
    /// </summary>
    /// <param name="predicate">Boolean indicating if the result is indeed invalid.</param>
    /// <param name="property">Name of the property that failed validation.</param>
    /// <param name="message">Reason the property failed validation.</param>
    /// <returns>New result that is either Ok or Invalid depending on the predicate.</returns>
    public static async Task<Result<TValue>> InvalidIfAsync<TValue>(TValue value, Func<TValue, Task<bool>> predicate, string property, string message) =>
        await predicate(value)
        ? new(new InvalidResultType(property, message))
        : Ok(value);
    #endregion

    #region --- Exception ---
    /// <summary>
    /// Create an exception result due to an exception.
    /// </summary>
    /// <param name="exception">The exception that occured.</param>
    public static Result Exception(Exception exception) =>
        new(new ExceptionResultType { Exception = exception }, exception.Message);

    /// <summary>
    /// Create an exception result of a given valuetype due to an exception.
    /// </summary>
    /// <param name="exception">The exception that occured.</param>
    public static Result<TValue> Exception<TValue>(Exception exception) =>
        new(new ExceptionResultType { Exception = exception }, exception.Message);

    /// <summary>
    /// Create an exception result of a given valuetype due to an exception.
    /// </summary>
    /// <param name="exception">The exception that occured.</param>
    public static Result<TValue> Exception<TValue>(TValue _, Exception exception) =>
        new(new ExceptionResultType { Exception = exception }, exception.Message);
    #endregion

    #region --- Not Found ---
    /// <summary>
    /// Creates a new not found error indicating that the requested resource could not be found.
    /// </summary>
    /// <typeparam name="TEntity">The type of the requested entity.</typeparam>
    /// <param name="id">The id of the entity that was looked for and not found.</param>
    public static Result<TEntity> NotFound<TEntity>(Guid id) =>
        new(new NotFoundResultType<Guid>() { Id = id }, $"Did not find {typeof(TEntity).Name} with Id {id}");

    /// <summary>
    /// Creates a new not found error indicating that the requested resource could not be found.
    /// </summary>
    /// <typeparam name="TEntity">The type of the requested entity.</typeparam>
    /// <param name="id">The id of the entity that was looked for and not found.</param>
    /// <param name="message">The message generated because the entity could not be found.</param>
    public static Result<TEntity> NotFound<TEntity>(Guid id, string message) =>
        new(new NotFoundResultType<Guid>() { Id = id }, message);

    /// <summary>
    /// Creates a new not found error indicating that the requested resource could not be found.
    /// </summary>
    /// <typeparam name="TEntity">The type of the requested entity.</typeparam>
    /// <param name="id">The id of the entity that was looked for and not found.</param>
    public static Result<TEntity> NotFound<TEntity, TKey>(TKey id)
        where TKey : notnull =>
        new(new NotFoundResultType<TKey>() { Id = id }, $"Did not find {typeof(TEntity).Name} with Id {id}");

    /// <summary>
    /// Creates a new not found error indicating that the requested resource could not be found.
    /// </summary>
    /// <typeparam name="TEntity">The type of the requested entity.</typeparam>
    /// <param name="id">The id of the entity that was looked for and not found.</param>
    /// <param name="message">The message generated because the entity could not be found.</param>
    public static Result<TEntity> NotFound<TEntity, TKey>(TKey id, string message)
        where TKey : notnull =>
        new(new NotFoundResultType<TKey>() { Id = id }, message);

    #endregion
}