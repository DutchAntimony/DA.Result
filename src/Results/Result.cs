using DA.Results.Issues;

namespace DA.Results;

/// <summary>
/// Static class to initialize Results without needing to specify a TValue.
/// </summary>
public static class Result
{
    /// <summary>
    /// Create a new Ok (Success) result with no value.
    /// </summary>
    /// <param name="ignoreWarnings">Should any warnings be ignored (true) or treated as errors (false, default)</param>
    /// <returns>New instance of a Result with as type parameter NoContent.</returns>
    public static Result<NoContent> Ok(bool ignoreWarnings = false) => new(NoContent.Instance, ignoreWarnings);
   
    /// <summary>
    /// Create a new Ok (Success) result with the provided value.
    /// </summary>
    /// <param name="value">The value encapsulated by the result</param>
    /// <param name="ignoreWarnings">Should any warnings be ignored (true) or treated as errors (false, default)</param>
    /// <returns>New instance of a Result with as type <typeparam name="TValue" /> and the provided value.</returns>
    public static Result<TValue> Ok<TValue>(TValue value, bool ignoreWarnings = false) => new(value, ignoreWarnings);
    
    /// <summary>
    /// Create a new failing result for a specific TValue with the provided issue.
    /// </summary>
    /// <param name="issue">The issue that causes this result to fail.</param>
    /// <typeparam name="TValue">The type of the created issue.</typeparam>
    /// <returns>New instance of a Failing Result with as type <typeparam name="TValue" /> and the provided issue.</returns>
    public static Result<TValue> Fail<TValue>(Issue issue) => new(issue);
    
    /// <summary>
    /// Create a new generic failing result with the provided failing message.
    /// </summary>
    /// <param name="message">The message of the failure.</param>
    /// <returns>New instance of a Failing Result with an <see cref="InvalidOperationError"/> as issue.</returns>
    public static Result<NoContent> Fail(string message) => new InvalidOperationError(message);
    
    /// <summary>
    /// Create a new failing result for a specific TValue with the provided failing message.
    /// </summary>
    /// <param name="message">The message of the failure.</param>
    /// <typeparam name="TValue">The type of the created issue.</typeparam>
    /// <returns>
    /// New instance of a Failing Result with as type <typeparam name="TValue" />
    /// and an <see cref="InvalidOperationError"/> as issue.
    /// </returns>
    public static Result<TValue> Fail<TValue>(string message) => new InvalidOperationError(message);
    
    /// <summary>
    /// Create a new warning that is either turned into an issue or turned into a <see cref="NoContent"/>
    /// depending on the ignoreWarnings flag.
    /// </summary>
    /// <param name="warning">The warning that has occured.</param>
    /// <param name="ignoreWarnings">Flag to indicate if warnings should be ignored.</param>
    /// <returns>New instance of a Result with as type parameter NoContent.</returns>
    public static Result<NoContent> Warn(Warning warning, bool ignoreWarnings) => 
        ignoreWarnings ? Ok(ignoreWarnings) : warning;
    
    /// <summary>
    /// Create a new warning that is either turned into an issue or turned into a success of TValue
    /// depending on the ignoreWarnings flag.
    /// </summary>
    /// <param name="value">The value if the warning is ignored.</param>
    /// <param name="warning">The warning that has occured.</param>
    /// <param name="ignoreWarnings">Flag to indicate if warnings should be ignored.</param>
    /// <returns>New instance of a Result with as type parameter NoContent.</returns>
    public static Result<TValue> Warn<TValue>(Warning warning, TValue value,  bool ignoreWarnings) => 
        ignoreWarnings ? Ok(value, ignoreWarnings) : warning;
    
    /// <summary>
    /// A Result with only information if something happened and no (actual) value.
    /// </summary>
    public record NoContent
    {
        private NoContent() { /* No new instances of the NoContent class can be made */ }
        
        /// <summary>
        /// The only instance of NoContent that can ever exist.
        /// </summary>
        public static NoContent Instance => new();
    }
}