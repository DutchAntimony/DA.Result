namespace DA.Results.Tests.Extensions.Check;

public class CheckTests : ResultTestBase
{
    [Fact]
    public void Check_Should_RemainSuccess_WhenCheckPasses()
    {
        var result = IntSuccessResult.Check(_ => SuccessResult);
        result.ShouldBeSuccessWithValue(42);
    }

    [Fact]
    public void Check_Should_BecomeError_WhenCheckFails()
    {
        var result = IntSuccessResult.Check(_ => InternalServerErrorResult);
        result.ShouldHaveIssue<InternalServerError>();
    }

    [Fact]
    public void Check_Should_RemainError_WhenCheckFails()
    {
        var result = IntInternalServerErrorResult.Check(_ => SuccessResult);
        result.ShouldHaveIssue<InternalServerError>();
    }

    [Fact]
    public void Check_Should_NotExecuteFunction_WhenInputIsFailure()
    {
        IntInternalServerErrorResult.Check(ThrowingResult);
    }

    [Fact]
    public void Check_OnResultOfTuple_Should_RequireTwoParameters()
    {
        Result<(int, double)> successTupleResult = (1, 0.5);
        successTupleResult.Check(TwoParameterFunc).ShouldBeSuccess();
        
        var errorTupleResult = successTupleResult.Check(_ => InternalServerErrorResult);
        errorTupleResult.Check(ThrowingResult);
    }
    
    [Fact]
    public async Task Check_Should_BehaveCorrectOnDifferentAsyncOverloads()
    {
        var successTask = Task.FromResult(IntSuccessResult);
        var errorTask = Task.FromResult((IResult)InternalServerErrorResult);
         
        (await IntSuccessResult.CheckAsync(() => errorTask)).ShouldHaveIssue<InternalServerError>();
        (await IntSuccessResult.CheckAsync(ErrorTaskFunc)).ShouldHaveIssue<InternalServerError>();
        (await IntInternalServerErrorResult.CheckAsync(ThrowingIResultTask)).ShouldHaveIssue<InternalServerError>();
        (await IntInternalServerErrorResult.CheckAsync(_ => ThrowingIResultTask())).ShouldHaveIssue<InternalServerError>();
        (await successTask.CheckAsync(() => errorTask)).ShouldHaveIssue<InternalServerError>();
        (await successTask.Check(_ => InternalServerErrorResult)).ShouldHaveIssue<InternalServerError>();
        (await successTask.CheckAsync(ErrorTaskFunc)).ShouldHaveIssue<InternalServerError>();

        var successTuple = Task.FromResult((Result<(int,double)>)(1, 0.5));
        (await successTuple.Check(TwoParameterFunc)).ShouldBeSuccess();
    }

    private IResult TwoParameterFunc(int value1, double value2) => SuccessResult;
}