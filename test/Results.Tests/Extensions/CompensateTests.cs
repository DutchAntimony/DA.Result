namespace DA.Results.Tests.Extensions;

public class CompensateTests : ResultTestBase
{
    [Fact]
    public void Compensate_Should_DoNothingToSuccess()
    {
        var result = IntSuccessResult.Compensate(ThrowingIntResult);
        result.ShouldBeSuccess();
    }

    [Fact]
    public void Compensate_Should_ReturnCompensatedResult_WhenIncomingResultIsAFailure()
    {
        var result = IntInternalServerErrorResult.Compensate(() => IntSuccessResult);
        result.ShouldBeSuccess();
    }

    [Fact]
    public async Task Compensate_Should_BehaveCorrectOnDifferentAsyncOverloads()
    {
        var failureTask = Task.FromResult(IntInternalServerErrorResult);
        var successTask = Task.FromResult(IntSuccessResult);
        
        (await IntInternalServerErrorResult.CompensateAsync(() => successTask)).ShouldBeSuccess();
        (await IntSuccessResult.CompensateAsync(ThrowingIntResultTask)).ShouldBeSuccess();
        (await failureTask.Compensate(() => IntSuccessResult)).ShouldBeSuccess();
        (await failureTask.CompensateAsync(() => successTask)).ShouldBeSuccess();
    }
}