namespace DA.Results.Tests.Extensions;

public class BindTests : ResultTestBase
{
    [Fact]
    public void Bind_Should_CreateError_WhenSuccessIsBindWithError()
    {
        var result = IntSuccessResult.Bind(_ => DoubleInternalServerErrorResult);
        result.ShouldHaveIssue<InternalServerError>();
    }

    [Fact]
    public void Bind_Should_CreateSuccess_WhenSuccessIsBindWithSuccess()
    {
        var result = IntSuccessResult.Bind(_ => DoubleSuccessResult);
        result.ShouldBeSuccessWithValue(6.9);
    }

    [Fact]
    public void Bind_Should_RemainError_WhenErrorIsBindWithSuccess()
    {
        var result = IntInternalServerErrorResult.Bind(_ => DoubleSuccessResult);
        result.ShouldHaveIssue<InternalServerError>();
    }

    [Fact]
    public void Bind_Should_RemainSameErrorType_WhenErrorIsBindWithOtherError()
    {
        var result = IntInternalServerErrorResult.Bind(_ => DoubleValidationErrorResult);
        result.ShouldHaveIssue<InternalServerError>();
    }

    [Fact]
    public void Bind_Should_RemainIgnoreWarningsStatus()
    {
        var result = Result.Ok(true).Bind(_ => IntSuccessResult);
        result.IgnoreWarnings.ShouldBe(true);
    }
    
    [Fact]
    public async Task Bind_Should_BehaveCorrectOnDifferentAsyncOverloads()
    {
        var successTask = Task.FromResult(IntSuccessResult);
        var errorTask = Task.FromResult(DoubleInternalServerErrorResult);
        (await successTask.Bind(_ => IntInternalServerErrorResult)).ShouldHaveIssue<InternalServerError>();
        (await IntSuccessResult.BindAsync(() => errorTask)).ShouldHaveIssue<InternalServerError>();
        (await IntSuccessResult.BindAsync(ErrorTaskFuncOfDouble)).ShouldHaveIssue<InternalServerError>();
        (await errorTask.BindAsync(ThrowingIntResultTask)).ShouldHaveIssue<InternalServerError>();
        (await IntInternalServerErrorResult.BindAsync(ThrowingIntResultTask)).ShouldHaveIssue<InternalServerError>();
        (await IntInternalServerErrorResult.BindAsync(_ => ThrowingIntResultTask())).ShouldHaveIssue<InternalServerError>();
        (await successTask.BindAsync(() => errorTask)).ShouldHaveIssue<InternalServerError>();
        (await successTask.BindAsync(ErrorTaskFuncOfDouble)).ShouldHaveIssue<InternalServerError>();
    }
}