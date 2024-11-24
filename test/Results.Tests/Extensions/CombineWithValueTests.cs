namespace DA.Results.Tests.Extensions;

public class CombineWithValueTests : ResultTestBase
{
    [Fact]
    public void Combine_Should_CreateCombinedSuccessResult_WhenResultsIsSuccess()
    {
        var result = IntSuccessResult.Combine(6.9d);
        result.ShouldBeOfType<Result<(int, double)>>();
        result.ShouldBeSuccess();
    }
    
    [Fact]
    public void Combine_Should_CreateCombinedSuccessResult_WhenResultsIsSuccess_FromFunc()
    {
        var result = IntSuccessResult.Combine(_ => 6.9d);
        result.ShouldBeOfType<Result<(int, double)>>();
        result.ShouldBeSuccess();
    }

    [Fact]
    public void Combine_Should_NotCallSecondResult_WhenFirstResultIsFailure()
    {
        var result = IntInternalServerErrorResult.Combine(12);
        result.ShouldBeOfType<Result<(int, int)>>();
        result.ShouldHaveIssue<InternalServerError>();
    }

    [Fact]
    public void Combine_Should_NotCallSecondResult_WhenFirstResultIsFailure_FromFunc()
    {
        var result = IntInternalServerErrorResult.Combine(_ => ThrowingValue());
        result.ShouldBeOfType<Result<(int, int)>>();
        result.ShouldHaveIssue<InternalServerError>();
    }
    
    [Fact]
    public async Task Combine_Should_BehaveCorrectOnDifferentAsyncOverloads()
    {
        var intSuccessTask = Task.FromResult(IntSuccessResult);
        var doubleTask = Task.FromResult(6.9);
        
        (await IntSuccessResult.CombineAsync(() => doubleTask)).ShouldBeSuccess();
        (await IntInternalServerErrorResult.CombineAsync(_ => ThrowingValueTask())).ShouldHaveIssue<InternalServerError>();
        
        (await intSuccessTask.Combine(6.9d)).ShouldBeSuccess();
        (await intSuccessTask.Combine(_ => 6.9)).ShouldBeSuccess();
        (await intSuccessTask.CombineAsync(() => doubleTask)).ShouldBeSuccess();
        (await intSuccessTask.CombineAsync(_ => doubleTask)).ShouldBeSuccess();
    }
}