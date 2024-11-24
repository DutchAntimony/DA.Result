namespace DA.Results.Shouldly;

// ReSharper disable NullableWarningSuppressionIsUsed
// Justification: Shouldly null checks are not recognised by ReSharper.

/// <summary>
/// Extension methods that verify that a given result is a success.
/// Does different checks or returns on the inner value.
/// </summary>
public static class SuccessResultTestExtensions
{
    /// <summary>
    /// Verifies that this result is a success result.
    /// </summary>
    /// <param name="result">Result where the assertion is called from.</param>
    public static void ShouldBeSuccess(this IResult result)
    {
        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeTrue();
    }
    
    /// <summary>
    /// Verifies that this result is a success result and returns the inner value.
    /// </summary>
    /// <param name="result">Result where the assertion is called from.</param>
    /// <typeparam name="TValue">The type the result value has. </typeparam>
    /// <returns>The actual value if the provided expected type.</returns>
    public static TValue ShouldBeSuccess<TValue>(this Result<TValue> result)
    {
        result.ShouldNotBeNull();
        result.TryGetValue(out var actualValue).ShouldBeTrue();
        return actualValue!;
    }

    /// <summary>
    /// Verifies that this result is a success result and that the value of the result equals the expected value.
    /// </summary>
    /// <param name="result">Result where the assertion is called from.</param>
    /// <param name="expectedValue">The value that is expected for this result to have.</param>
    /// <typeparam name="TValue">The type the result value has. </typeparam>
    public static void ShouldBeSuccessWithValue<TValue>(this Result<TValue> result, TValue expectedValue)
    {
        result.ShouldNotBeNull();
        result.TryGetValue(out var actualValue).ShouldBeTrue();
        actualValue.ShouldBe(expectedValue);
    }

    /// <summary>
    /// Verifies that this result is a success result and that the value of the result matches the provided predicate.
    /// </summary>
    /// <param name="result">Result where the assertion is called from.</param>
    /// <param name="predicate">The condition the value this result has should have.</param>
    /// <typeparam name="TValue">The type the result value has. </typeparam>
    public static void ShouldHaveValueThatSatisfies<TValue>(this Result<TValue> result, Predicate<TValue> predicate)
    {
        result.ShouldNotBeNull();
        result.TryGetValue(out var actualValue).ShouldBeTrue();
        predicate(actualValue!).ShouldBeTrue();
    }
}