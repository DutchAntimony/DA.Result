namespace DA.Results.Tests;

public class ResultWarningTests : ResultTestBase
{
    [Fact]
    public void WarningResult_ShouldBe_Error_WithIgnoreWarningSetToFalse()
    {
        WarningResult.IsSuccess.ShouldBeFalse();
        WarningResult.ShouldHaveIssue<ConfirmationRequiredWarning>();
        
        IntWarningResult.IsSuccess.ShouldBeFalse();
        IntWarningResult.ShouldHaveIssue<ConfirmationRequiredWarning>();
    }

    [Fact]
    public void WarningResult_ShouldBe_Success_WhenCheckedAndIgnoreWarningSetToTrue()
    {
        Result.Ok(true).Bind(_ => IntSuccessResult).Check(_ => WarningResult).ShouldBeSuccess();
        Result.Ok(42, true).Check(_ => WarningResult).ShouldBeSuccess();
    }

    [Fact]
    public void ResultWithIgnoreWarnings_Should_RetainStatus_AfterMapping()
    {
        Result.Ok(true).Map(_ => 42).Check(_ => WarningResult).ShouldBeSuccess();
        Result.Ok(42, true).Map(_ => 6.9d).Check(_ => WarningResult).ShouldBeSuccess();
    }
    
    [Fact]
    public void ResultWithIgnoreWarnings_Should_RetainStatus_AfterCombiningWithValue()
    {
        Result.Ok(true)
            .Map(() => 42)
            .Combine(_ => 6.9d)
            .Check(_ => WarningResult).ShouldBeSuccess();
    }
}