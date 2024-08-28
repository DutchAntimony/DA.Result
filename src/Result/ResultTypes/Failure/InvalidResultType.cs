namespace DA.Results.ResultTypes.Failure;

/// <summary>
/// Failure result type that wraps information about the input that is invalid.
/// </summary>
public record InvalidResultType : IFailureResultType
{
    /// <inheritdoc/>
    public bool IsSuccess => false;

    /// <summary>
    /// Information about which received properties failed and a message indicating why.
    /// The key is the failing property.
    /// The value is the attached message.
    /// </summary>
    public IEnumerable<ValidationInfo> FailedValidations { get; }

    public InvalidResultType(string property, string message) =>
        FailedValidations = [new ValidationInfo(property, message)];

    public InvalidResultType(IEnumerable<ValidationInfo> failedValidations) =>
        FailedValidations = failedValidations;

    /// <inheritdoc/>
    public int Weight => 8;

    /// <summary>
    /// Information about a validation failure.
    /// </summary>
    /// <param name="Property">The property that has an invalid value.</param>
    /// <param name="Message">Message why the value of the property is invalid.</param>
    public record ValidationInfo(string Property, string Message)
    {
        internal static IEnumerable<ValidationInfo> FromDictionary(Dictionary<string, string> input)
        {
            return input.Select(kvp => new ValidationInfo(kvp.Key, kvp.Value));
        }
    }

    /// <inheritdoc/>
    public IResultType Combine(IResultType resultType)
    {
        if (resultType is InvalidResultType invalid)
        {
            return new InvalidResultType(FailedValidations.Concat(invalid.FailedValidations));
        }

        if (resultType is IFailureResultType failure && failure.Weight > Weight)
        {
            return failure;
        }

        return this;
    }
}