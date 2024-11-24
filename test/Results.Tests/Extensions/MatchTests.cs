namespace DA.Results.Tests.Extensions;

public class MatchTests : ResultTestBase
{
    private int _testCounter = 0;

    [Fact]
    public void Match_Should_ApplySuccessAction_IfSuccess()
    {
        var before = _testCounter;
        SuccessResult.Match(_ => IncreaseCounterWithValue(), _ => ThrowingAction());
        _testCounter.ShouldBe(before + 1);
    }

    [Fact]
    public void Match_Should_ApplyFailureAction_IfFailure()
    {
        var before = _testCounter;
        InternalServerErrorResult.Match(_ => ThrowingAction(), _ => IncreaseCounterWithValue());
        _testCounter.ShouldBe(before + 1);
    }

    [Fact]
    public void Match_Should_ApplyValueOfSuccessAction_IfSuccess()
    {
        var before = _testCounter;
        IntSuccessResult.Match(IncreaseCounter, _ => ThrowingAction());
        _testCounter.ShouldBe(before + 42);
    }

    [Fact]
    public void Match_Should_ApplyValueOfIssueAction_IfFailure()
    {
        var testString = string.Empty;
        InternalServerErrorResult.Match(_ => ThrowingAction(), issue => testString += issue.Title);
        testString.ShouldBe(new InternalServerError(new Exception()).Title);
    }

    [Fact]
    public void Match_Should_ApplySuccessFunction_IfSuccess()
    {
        var before = _testCounter;
        var result = SuccessResult.Match(_ => IncreaseCounterAndReturn(), _ => ThrowingValue());
        result.ShouldBe(before + 1);
    }
    
    [Fact]
    public void Match_Should_ApplyFailureFunction_IfFailure()
    {
        var result = InternalServerErrorResult.Match(_ => ThrowingValue(), _ => 42);
        result.ShouldBe(42);
    }

    [Fact]
    public async Task MatchAsync_Should_ApplyFailureTaskWithoutValue_IfFailure()
    {
        var testString = string.Empty;
        await IntInternalServerErrorResult.MatchAsync(
            ThrowingTask,
            issue =>
            {
                testString += issue.Title;
                return Task.CompletedTask;
            });
        testString.ShouldBe(new InternalServerError(new Exception()).Title);
    }
    
    [Fact]
    public async Task MatchAsync_Should_ApplySuccessTaskWithoutValue_IfSuccess()
    {
        var before = _testCounter;
        await IntSuccessResult.MatchAsync(IncreaseCounterTask, ThrowingTask);
        _testCounter.ShouldBe(before + 42);
    }
    
    [Fact]
    public async Task MatchAsync_Should_ApplySuccessTask_IfSuccess()
    {
        var before = _testCounter;
        var result = await SuccessResult.MatchAsync(
            IncreaseCounterAndReturnTask,
            () => Task.FromResult(ThrowingValue()));
        result.ShouldBe(before + 1);
    }
    
    [Fact]
    public async Task MatchAsync_Should_ApplyFailureTask_IfFailure()
    {
        var before = _testCounter;
        var result = await InternalServerErrorResult.MatchAsync(
            () => Task.FromResult(ThrowingValue()),
            IncreaseCounterAndReturnTask);
        result.ShouldBe(before + 1);
    }
    
    [Fact]
    public async Task MatchAsync_Should_ApplySuccessTaskFunc_IfSuccess()
    {
        var before = _testCounter;
        var result = await IntSuccessResult.MatchAsync(
            _ => IncreaseCounterAndReturnTask(), 
            _ => Task.FromResult(ThrowingValue()));
        result.ShouldBe(before + 1);
    }
    
    [Fact]
    public async Task MatchAsync_Should_ApplyFailureTaskFunc_IfFailure()
    {
        var before = _testCounter;
        var result = await InternalServerErrorResult.MatchAsync(
            _ => Task.FromResult(ThrowingValue()),
            _ => IncreaseCounterAndReturnTask());
        result.ShouldBe(before + 1);
    }

    [Fact]
    public async Task Match_Should_BehaveCorrectOnDifferentAsyncOverloads()
    {
        var intResultTask = Task.FromResult(IntSuccessResult);
        
        var before = _testCounter;
        await intResultTask.Match(IncreaseCounter, _ => ThrowingAction());
        _testCounter.ShouldBe(before + 42);
        
        before = _testCounter;
        var result = await intResultTask.Match(IncreaseCounterWithValueAndReturn, _ => ThrowingValue());
        result.ShouldBe(before + 42);
        _testCounter.ShouldBe(result);
        
        before = _testCounter;
        await intResultTask.MatchAsync(IncreaseCounterTask, ThrowingTask);
        _testCounter.ShouldBe(before + 42);
    }
    
    private void IncreaseCounterWithValue()
    {
        _testCounter++;
    }

    private void IncreaseCounter(int value)
    {
        _testCounter += value;
    }

    private int IncreaseCounterAndReturn()
    {
        _testCounter++;
        return _testCounter;
    }

    private int IncreaseCounterWithValueAndReturn(int value)
    {
        _testCounter += value;
        return _testCounter;
    }
    
    private Task IncreaseCounterTask(int value)
    {
        IncreaseCounter(value);
        return Task.CompletedTask;
    }

    private Task<int> IncreaseCounterAndReturnTask()
    {
        return Task.FromResult(IncreaseCounterAndReturn());
    }
}