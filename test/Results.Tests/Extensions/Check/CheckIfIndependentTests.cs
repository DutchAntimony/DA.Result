namespace DA.Results.Tests.Extensions.Check;

public class CheckIfIndependentTests : ResultTestBase
{
    private static bool DoTheCheckFunc() => true;
    private static bool DontDoTheCheckFunc() => false;

    [Fact]
    public void CheckIfResult_Should_RemainSuccess_WhenPredicateIsFalse()
    {
        var result = IntSuccessResult.CheckIf(DontDoTheCheckFunc, _ => InternalServerErrorResult);
        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public void CheckIfResult_Should_BecomeFailure_WhenPredicateIsTrue()
    {
        var result = IntSuccessResult.CheckIf(DoTheCheckFunc, _ => InternalServerErrorResult);
        result.ShouldHaveIssue<InternalServerError>();
    }

    [Fact]
    public void CheckIfResult_Should_NotExecuteFunction_WhenIncomingResultIsFailure()
    {
        var result = IntInternalServerErrorResult.CheckIf(DoTheCheckFunc, ThrowingResult);
        result.ShouldHaveIssue<InternalServerError>();
    }

    [Fact]
    public async Task CheckIf_Should_BehaveCorrectOnDifferentAsyncOverloads()
    {
        var successTask = Task.FromResult(IntSuccessResult);
        var errorTask = Task.FromResult((IResult)InternalServerErrorResult);
        (await IntSuccessResult.CheckIfAsync(DoTheCheckFunc, () => errorTask)).ShouldHaveIssue<InternalServerError>();
        (await IntSuccessResult.CheckIfAsync(DontDoTheCheckFunc, ThrowingIResultTask)).ShouldBeSuccess();
        (await IntSuccessResult.CheckIfAsync(DontDoTheCheckFunc, ThrowingIntResultTask)).ShouldBeSuccess();
        (await IntInternalServerErrorResult.CheckIfAsync(DoTheCheckFunc, ThrowingIResultTask)).ShouldHaveIssue<InternalServerError>();
        (await successTask.CheckIf(DoTheCheckFunc, _ => InternalServerErrorResult)).ShouldHaveIssue<InternalServerError>();
        (await successTask.CheckIfAsync(DoTheCheckFunc, () => errorTask)).ShouldHaveIssue<InternalServerError>();
        (await successTask.CheckIfAsync(DontDoTheCheckFunc, ThrowingIntResultTask)).ShouldBeSuccess();
    }
}