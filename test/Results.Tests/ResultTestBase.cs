using System.Diagnostics.CodeAnalysis;

namespace DA.Results.Tests;

/// <summary>
/// Base test for all result tests.
/// Contains many protected results and fields to make result testing easier.
/// </summary>
[ExcludeFromCodeCoverage(Justification = "Throwing should specifically not be hit.")]
public abstract class ResultTestBase
{
    protected readonly NoContentResult SuccessResult = Result.Ok();
    protected readonly NoContentResult WarningResult = new ConfirmationRequiredWarning("Test warning");
    protected readonly NoContentResult InternalServerErrorResult = new InternalServerError(new Exception("Test error"));
    protected readonly NoContentResult ValidationErrorResult = new ValidationError("Property", "Invalid value");
    
    protected readonly Result<int> IntSuccessResult = 42;
    protected readonly Result<int> IntWarningResult = ((Result<int>)42).Check(_ => (NoContentResult)(new ConfirmationRequiredWarning("Test warning")));
    protected readonly Result<int> IntInternalServerErrorResult = new InternalServerError(new Exception("Test error"));
    protected readonly Result<int> IntValidationErrorResult = new ValidationError("Property", "Invalid value");
    
    protected readonly Result<double> DoubleSuccessResult = 6.9;
    protected readonly Result<double> DoubleWarningResult = ((Result<double>)6.9).Check(_ => (NoContentResult)(new ConfirmationRequiredWarning("Test warning")));
    protected readonly Result<double> DoubleInternalServerErrorResult = new InternalServerError(new Exception("Test error"));
    protected readonly Result<double> DoubleValidationErrorResult = new ValidationError("Property", "Invalid value");
    
        
    protected Task<Result<double>> ErrorTaskFuncOfDouble(int i) => Task.FromResult(DoubleInternalServerErrorResult);
    protected Task<IResult> ErrorTaskFunc(int i) => Task.FromResult((IResult)InternalServerErrorResult);
    
    
    protected static Task<int> ThrowingValueTask() => throw new ShouldAssertException("Method should not have been called");
    protected static Task<int> ThrowingValueTask(int value) => throw new ShouldAssertException("Method should not have been called");
    protected static Task<NoContentResult> ThrowingResultTask() => throw new ShouldAssertException("Method should not have been called");
    protected static Task<IResult> ThrowingIResultTask() => throw new ShouldAssertException("Method should not have been called");
    protected static Task<Result<int>> ThrowingIntResultTask() => throw new ShouldAssertException("Method should not have been called");
    protected static Task<IResult> ThrowingIntResultTask(int input) => throw new ShouldAssertException("Method should not have been called");
    protected static IResult ThrowingResult(int value) => throw new ShouldAssertException("Method should not have been called");
    protected static IResult ThrowingResult(int value1, double value2) => throw new ShouldAssertException("Method should not have been called");
    protected static Result<int> ThrowingIntResult() => throw new ShouldAssertException("Method should not have been called");
    protected static int ThrowingValue() => throw new ShouldAssertException("Method should not have been called");
    protected static int ThrowingValue(double value) => throw new ShouldAssertException("Method should not have been called");
    protected static int ThrowingValue(int value1, double value2) => throw new ShouldAssertException("Method should not have been called");
    protected static Result<double> DoubleThrowingResult(int value) => throw new ShouldAssertException("Method should not have been called");
    protected static void ThrowingAction() => throw new ShouldAssertException("Method should not have been called");
    protected static Task ThrowingTask(int value) => throw new ShouldAssertException("Method should not have been called");
    protected static Task ThrowingTask(Issue issue) => throw new ShouldAssertException("Method should not have been called");
}