namespace DA.Results.Tests.Shouldly;

public class FailureResultTests : ResultTestBase
{
    private readonly IResult? _nullResult = null;
    
    [Fact]
    public void ShouldHaveIssueTests()
    {
        InternalServerErrorResult.ShouldHaveIssue<InternalServerError>();
        Should.Throw<ShouldAssertException>(() => SuccessResult.ShouldHaveIssue<ValidationError>());
        Should.Throw<ShouldAssertException>(() => InternalServerErrorResult.ShouldHaveIssue<ValidationError>());
        Should.Throw<ShouldAssertException>(() => _nullResult.ShouldHaveIssue<ValidationError>());
    }
    
    [Fact]
    public void ShouldHaveIssueWithOutParameterTests()
    {
        InternalServerErrorResult.ShouldHaveIssue<InternalServerError>(out var issue);
        issue.GetMessage().ShouldBe("Test error");
        Should.Throw<ShouldAssertException>(() => SuccessResult.ShouldHaveIssue<ValidationError>(out _));
        Should.Throw<ShouldAssertException>(() => InternalServerErrorResult.ShouldHaveIssue<ValidationError>(out _));
        Should.Throw<ShouldAssertException>(() => _nullResult.ShouldHaveIssue<ValidationError>(out _));
    }
    
    [Fact]
    public void ShouldHaveIssueAndMessageTests()
    {
        InternalServerErrorResult.ShouldHaveIssueAndMessage<InternalServerError>("Test error");
        Should.Throw<ShouldAssertException>(() => SuccessResult.ShouldHaveIssueAndMessage<ValidationError>(""));
        Should.Throw<ShouldAssertException>(() => InternalServerErrorResult.ShouldHaveIssueAndMessage<ValidationError>(""));
        Should.Throw<ShouldAssertException>(() => _nullResult.ShouldHaveIssueAndMessage<ValidationError>(""));
    }

    [Fact]
    public void ShouldHaveIssueAndContainMessageTests()
    {
        InternalServerErrorResult.ShouldHaveIssueAndContainMessage<InternalServerError>("Test");
        Should.Throw<ShouldAssertException>(() => SuccessResult.ShouldHaveIssueAndContainMessage<ValidationError>(""));
        Should.Throw<ShouldAssertException>(() => InternalServerErrorResult.ShouldHaveIssueAndContainMessage<ValidationError>(""));
        Should.Throw<ShouldAssertException>(() => _nullResult.ShouldHaveIssueAndContainMessage<ValidationError>(""));
    }

    [Fact]
    public void ShouldHaveValidationErrorsEqualToTests()
    {
        var expected = new ValidationFailure("Property", "Invalid value");
        ValidationErrorResult.ShouldHaveValidationErrorsEqualTo(expected);
        Should.Throw<ShouldAssertException>(() => SuccessResult.ShouldHaveValidationErrorsEqualTo(expected));
        Should.Throw<ShouldAssertException>(() => InternalServerErrorResult.ShouldHaveValidationErrorsEqualTo(expected));
        Should.Throw<ShouldAssertException>(() => _nullResult.ShouldHaveValidationErrorsEqualTo(expected));
    }
    
    [Fact]
    public void ShouldHaveValidationErrorCountTests()
    {
        ValidationErrorResult.ShouldHaveValidationErrorCount(1);
        Should.Throw<ShouldAssertException>(() => ValidationErrorResult.ShouldHaveValidationErrorCount(0));
        Should.Throw<ShouldAssertException>(() => SuccessResult.ShouldHaveValidationErrorCount(0));
        Should.Throw<ShouldAssertException>(() => InternalServerErrorResult.ShouldHaveValidationErrorCount(0));
        Should.Throw<ShouldAssertException>(() => _nullResult.ShouldHaveValidationErrorCount(0));
    }
    
    [Fact]
    public void ShouldHaveValidationErrorForPropertyTests()
    {
        var expected = "Property";
        ValidationErrorResult.ShouldHaveValidationErrorForProperty(expected);
        Should.Throw<ShouldAssertException>(() => ValidationErrorResult.ShouldHaveValidationErrorForProperty(""));
        Should.Throw<ShouldAssertException>(() => SuccessResult.ShouldHaveValidationErrorForProperty(expected));
        Should.Throw<ShouldAssertException>(() => InternalServerErrorResult.ShouldHaveValidationErrorForProperty(expected));
        Should.Throw<ShouldAssertException>(() => _nullResult.ShouldHaveValidationErrorForProperty(expected));
    }
    
    [Fact]
    public void ShouldHaveValidationErrorForPropertyWithMessageTests()
    {
        var expectedProp = "Property";
        var expectedMessage = "Invalid value";
        ValidationErrorResult.ShouldHaveValidationErrorForPropertyWithMessage(expectedProp, expectedMessage);
        Should.Throw<ShouldAssertException>(() => ValidationErrorResult.ShouldHaveValidationErrorForPropertyWithMessage("" , expectedMessage));
        Should.Throw<ShouldAssertException>(() => ValidationErrorResult.ShouldHaveValidationErrorForPropertyWithMessage(expectedProp , "notContaining"));
        Should.Throw<ShouldAssertException>(() => SuccessResult.ShouldHaveValidationErrorForPropertyWithMessage(expectedProp, expectedMessage));
        Should.Throw<ShouldAssertException>(() => InternalServerErrorResult.ShouldHaveValidationErrorForPropertyWithMessage(expectedProp, expectedMessage));
        Should.Throw<ShouldAssertException>(() => _nullResult.ShouldHaveValidationErrorForPropertyWithMessage(expectedProp, expectedMessage));
    }

    [Fact]
    public void ShouldHaveExceptionOfTypeTests()
    {
        InternalServerErrorResult.ShouldHaveExceptionOfType<Exception>();
        Should.Throw<ShouldAssertException>(() => InternalServerErrorResult.ShouldHaveExceptionOfType<NullReferenceException>());
        Should.Throw<ShouldAssertException>(() => SuccessResult.ShouldHaveExceptionOfType<Exception>());
        Should.Throw<ShouldAssertException>(() => _nullResult.ShouldHaveExceptionOfType<Exception>());
    }
    
    [Fact]
    public void ShouldHaveExceptionOfTypeWithMessageTests()
    {
        var expectedMessage = "Test error";
        InternalServerErrorResult.ShouldHaveExceptionOfTypeWithMessage<Exception>(expectedMessage);
        Should.Throw<ShouldAssertException>(() => InternalServerErrorResult.ShouldHaveExceptionOfTypeWithMessage<NullReferenceException>(expectedMessage));
        Should.Throw<ShouldAssertException>(() => SuccessResult.ShouldHaveExceptionOfTypeWithMessage<Exception>(expectedMessage));
        Should.Throw<ShouldAssertException>(() => _nullResult.ShouldHaveExceptionOfTypeWithMessage<Exception>(expectedMessage));
    }
}