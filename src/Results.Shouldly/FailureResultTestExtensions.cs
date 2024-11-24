// ReSharper disable NullableWarningSuppressionIsUsed
// Justification: Shouldly null checks are not recognised by ReSharper.

namespace DA.Results.Shouldly;

/// <summary>
/// Extension methods that verify that a giver result is a failure.
/// Does different checks or returns on the issue.
/// </summary>
public static class FailureResultTestExtensions
{
    /// <summary>
    /// Verifies that this result has an issue of the provided type.
    /// </summary>
    /// <param name="result">Result where the assertion is called from.</param>
    /// <typeparam name="TIssue">The expected type of issue.</typeparam>
    public static void ShouldHaveIssue<TIssue>(this IResult result)
        where TIssue : Issue
    {
        result.ShouldNotBeNull();
        result.TryGetIssue(out var issue).ShouldBeTrue();
        issue.ShouldBeOfType<TIssue>();
    }
    
    /// <summary>
    /// Verifies that this result has an issue of the provided type.
    /// </summary>
    /// <param name="result">Result where the assertion is called from.</param>
    /// <param name="issue">The actual issue if indeed of type of the expected type.</param>
    /// <typeparam name="TIssue">The expected type of issue.</typeparam>
    public static void ShouldHaveIssue<TIssue>(this IResult result, out TIssue issue)
        where TIssue : Issue
    {
        result.ShouldNotBeNull();
        result.TryGetIssue(out var actualIssue).ShouldBeTrue();
        actualIssue.ShouldBeOfType<TIssue>();
        issue = (actualIssue as TIssue)!;
    }

    /// <summary>
    /// Verifies that this result has an issue of the provided type with exactly the provided expected message.
    /// </summary>
    /// <param name="result">Result where the assertion is called from.</param>
    /// <param name="expectedMessage">The exact message expected.</param>
    /// <typeparam name="TIssue">The expected type of issue.</typeparam>
    public static void ShouldHaveIssueAndMessage<TIssue>(this IResult result, string expectedMessage)
        where TIssue : Issue
    {
        result.ShouldHaveIssue<TIssue>(out var issue);
        issue.GetMessage().ShouldBe(expectedMessage);
    }
    
    /// <summary>
    /// Verifies that this result has an issue of the provided type with a message that matches the provided expected message.
    /// </summary>
    /// <param name="result">Result where the assertion is called from.</param>
    /// <param name="expectedMessage">The part of the message that is expected.</param>
    /// <typeparam name="TIssue">The expected type of issue.</typeparam>
    public static void ShouldHaveIssueAndContainMessage<TIssue>(this IResult result, string expectedMessage) 
        where TIssue : Issue
    {
        result.ShouldHaveIssue<TIssue>(out var issue);
        issue.GetMessage().ShouldContain(expectedMessage);
    }

    /// <summary>
    /// Verifies that this result has an <see cref="ValidationError"/> and that the inner failures match the
    /// collection of expected failures.
    /// </summary>
    /// <param name="result">Result where the assertion is called from.</param>
    /// <param name="expectedFailures">Collection of expected failures that must all be matched exactly.</param>
    public static void ShouldHaveValidationErrorsEqualTo(this IResult result, params IEnumerable<ValidationFailure> expectedFailures)
    {
        result.ShouldHaveIssue<ValidationError>(out var issue);
        issue.Failures.ShouldBeEquivalentTo(expectedFailures);
    }
    
    /// <summary>
    /// Verifies that this result has an <see cref="ValidationError"/> and that the inner failures count equals the expected.
    /// </summary>
    /// <param name="result">Result where the assertion is called from.</param>
    /// <param name="expectedFailureCount">Number of failures that validation error is expected to have.</param>
    public static void ShouldHaveValidationErrorCount(this IResult result, int expectedFailureCount)
    {
        result.ShouldHaveIssue<ValidationError>(out var issue);
        issue.Failures.Count.ShouldBe(expectedFailureCount);
    }
    
    /// <summary>
    /// Verifies that this result has an <see cref="ValidationError"/> with a failure for the provided property name.
    /// </summary>
    /// <param name="result">Result where the assertion is called from.</param>
    /// <param name="expectedPropertyName">The name of the property that is expected to have a failure.</param>
    public static void ShouldHaveValidationErrorForProperty(this IResult result, string expectedPropertyName)
    {
        result.ShouldHaveIssue<ValidationError>(out var issue);
        issue.Failures.Any(failure => failure.Property == expectedPropertyName).ShouldBeTrue();
    }

    /// <summary>
    /// Verifies that this result has an <see cref="ValidationError"/> with a failure for the provided property name.
    /// </summary>
    /// <param name="result">Result where the assertion is called from.</param>
    /// <param name="expectedPropertyName">The name of the property that is expected to have a failure.</param>
    /// <param name="expectedErrorMessage">The expected part of the content of the message of the failure of the provided property.</param>
    public static void ShouldHaveValidationErrorForPropertyWithMessage(this IResult result, string expectedPropertyName, string expectedErrorMessage)
    {
        result.ShouldHaveIssue<ValidationError>(out var issue);
        issue.Failures.Any(failure => failure.Property == expectedPropertyName && failure.Message.Contains(expectedErrorMessage)).ShouldBeTrue();
    }

    /// <summary>
    /// Verifies that this result has an <see cref="InternalServerError"/> wrapping an Exception that is expected.
    /// </summary>
    /// <param name="result">Result where the assertion is called from.</param>
    /// <typeparam name="TException">The type of the exception that is expected.</typeparam>
    public static void ShouldHaveExceptionOfType<TException>(this IResult result)
    {
        result.ShouldHaveIssue<InternalServerError>(out var issue);
        issue.Exceptions.ShouldContain(exception => exception.GetType() == typeof(TException));
    }
    
    /// <summary>
    /// Verifies that this result has an <see cref="InternalServerError"/> wrapping an Exception that is expected.
    /// </summary>
    /// <param name="result">Result where the assertion is called from.</param>
    /// <param name="expectedErrorMessage">The expected part of the content of the exception.</param>
    /// <typeparam name="TException">The type of the exception that is expected.</typeparam>
    public static void ShouldHaveExceptionOfTypeWithMessage<TException>(this IResult result, string expectedErrorMessage)
    {
        result.ShouldHaveIssue<InternalServerError>(out var issue);
        issue.Exceptions.ShouldContain(exception => 
            exception.GetType() == typeof(TException) && exception.Message.Contains(expectedErrorMessage));
    }
}