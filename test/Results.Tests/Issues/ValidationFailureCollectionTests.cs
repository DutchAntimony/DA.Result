namespace DA.Results.Tests.Issues;

public class ValidationFailureCollectionTests : ResultTestBase
{
    private readonly List<ValidationFailure> _failures =
    [
        new ValidationFailure("Prop1", "Message1"),
        new ValidationFailure("Prop1", "Message2"),
        new ValidationFailure("Prop2", "Message1")
    ];
        
    [Fact]
    public void ToResult_ShouldBecomeSuccessResult_WhenNoFailures()
    {
        var validationFailureCollection = new ValidationFailureCollection();
        var result = validationFailureCollection.ToResult();
        result.ShouldBeSuccess();
    }
    
    [Fact]
    public void ToResultWithValue_ShouldBecomeSuccessResult_WhenNoFailures()
    {
        var validationFailureCollection = new ValidationFailureCollection();
        var result = validationFailureCollection.ToResult(42);
        result.ShouldBeSuccess();
        result.ShouldBeOfType<Result<int>>();
    }

    [Fact]
    public void ToResult_ShouldBecomeValidationError_WhenThereAreFailures()
    {
        var validationFailureCollection = new ValidationFailureCollection(_failures);
        var result = validationFailureCollection.ToResult();
        result.ShouldHaveIssue<ValidationError>();
    }
    
    [Fact]
    public void ToResultWithValue_ShouldBecomeValidationError_WhenThereAreFailures()
    {
        var validationFailureCollection = new ValidationFailureCollection(_failures);
        var result = validationFailureCollection.ToResult(42);
        result.ShouldHaveIssue<ValidationError>();
        result.ShouldBeOfType<Result<int>>();
    }

    [Fact]
    public void ToResult_ShouldBecomeValidationError_WhenFailureResultIsAdded()
    {
        var validationFailureCollection = new ValidationFailureCollection();
        validationFailureCollection.AddFrom(ValidationErrorResult);
        var result = validationFailureCollection.ToResult();
        result.ShouldHaveIssue<ValidationError>();
    }
    
    [Fact]
    public void ToResult_ShouldBecomeValidationError_WhenFailureResultCollectionIsAdded()
    {
        var validationFailureCollection = new ValidationFailureCollection();
        validationFailureCollection.AddFrom((NoContentResult)new ValidationError(_failures));
        var result = validationFailureCollection.ToResult();
        result.ShouldHaveIssue<ValidationError>(out var issue);
        issue.Title.ShouldBe("One or more validation errors occurred:");
        issue.StatusCode.ShouldBe(400);
    }

    [Fact]
    public void ToResult_ShouldBecomeExistingNotValidationFailures_WhenOtherFailureIsAdded()
    {
        var validationFailureCollection = new ValidationFailureCollection(_failures);
        var result = validationFailureCollection.AddFrom(IntWarningResult).ToResult();
        result.ShouldHaveIssue<ConfirmationRequiredWarning>();
    }
    
    [Fact]
    public void ToResultWithValue_ShouldBecomeExistingNotValidationFailures_WhenOtherFailureIsAdded()
    {
        var validationFailureCollection = new ValidationFailureCollection(_failures);
        var result = validationFailureCollection.AddFrom(IntWarningResult).ToResult(42);
        result.ShouldHaveIssue<ConfirmationRequiredWarning>();
    }
}