namespace DA.Results.Tests.Extensions;

public class CombineWithResultTests : ResultTestBase
{
    private Result<double> CreateSuccess(int value) => DoubleSuccessResult;
    private Result<double> CreateFailure(int value) => DoubleInternalServerErrorResult;
    
    [Fact]
    public void Combine_Should_CreateCombinedSuccessResult_WhenBothResultsAreSuccess()
    {
        var result = IntSuccessResult.Combine(CreateSuccess);
        result.ShouldBeOfType<Result<(int, double)>>();
        result.ShouldBeSuccess();
    }

    [Fact]
    public void Combine_Should_CreateCombinedFailureResult_WhenSecondResultIsFailure()
    {
        var result = IntSuccessResult.Combine(CreateFailure);
        result.ShouldBeOfType<Result<(int, double)>>();
        result.ShouldHaveIssue<InternalServerError>();
    }

    [Fact]
    public void Combine_Should_NotCallSecondResult_WhenFirstResultIsFailure()
    {
        var result = IntInternalServerErrorResult.Combine(DoubleThrowingResult);
        result.ShouldBeOfType<Result<(int, double)>>();
        result.ShouldHaveIssue<InternalServerError>();
    }

    [Fact]
    public async Task Combine_Should_BehaveCorrectOnDifferentAsyncOverloads()
    {
        var intSuccessTask = Task.FromResult(IntSuccessResult);
        var doubleSuccessTask = Task.FromResult(DoubleSuccessResult);
        
        (await IntSuccessResult.CombineAsync(() => doubleSuccessTask)).ShouldBeSuccess();
        (await IntSuccessResult.CombineAsync(_ => doubleSuccessTask)).ShouldBeSuccess();
        
        (await IntInternalServerErrorResult.CombineAsync(ThrowingIntResultTask)).ShouldHaveIssue<InternalServerError>();
        (await IntInternalServerErrorResult.CombineAsync(_ => ThrowingIntResultTask())).ShouldHaveIssue<InternalServerError>();
        
        (await intSuccessTask.Combine(_ => DoubleSuccessResult)).ShouldBeSuccess();
        (await intSuccessTask.CombineAsync(() => doubleSuccessTask)).ShouldBeSuccess();
        (await intSuccessTask.CombineAsync(_ => doubleSuccessTask)).ShouldBeSuccess();
    }
}