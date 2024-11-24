namespace DA.Results.Tests.Extensions;

public class TapCombinedTests : ResultTestBase
{
    private double _testCounter;
    private readonly Result<(int, double)> _successTupleResult;
    private readonly Result<(int, double)> _failureTupleResult;

    public TapCombinedTests()
    {
        _successTupleResult = IntSuccessResult.Combine(_ => 6.9);
        _failureTupleResult = IntInternalServerErrorResult.Combine(_ => 2.0);
    }
    
    [Fact]
    public void Tap_Should_CallAction_OnSuccessResult()
    {
        _testCounter = 0;
        var result = _successTupleResult.Tap(IncreaseCounter);
        _testCounter.ShouldBe(42 + 6.9);
        result.ShouldBeOfType<Result<(int,double)>>();
    }
    
    [Fact]
    public void Tap_Should_NotCallFunction_OnFailureResult()
    {
        var result = _failureTupleResult.Tap(() => ThrowingValue());
        result.ShouldBeOfType<Result<(int, double)>>();
    }
    
    [Fact]
    public async Task Tap_Should_BehaveCorrectOnDifferentAsyncOverloads()
    {
        var successTupleTask = Task.FromResult(_successTupleResult);

        _testCounter = 0;
        var result = await _successTupleResult.TapAsync(IncreaseCounterTask);
        result.ShouldBeOfType<Result<(int,double)>>();
        _testCounter.ShouldBe(42 + 6.9);
        
        result = await _failureTupleResult.TapAsync((value, _) => ThrowingValueTask(value));
        result.ShouldBeOfType<Result<(int,double)>>();

        _testCounter = 0;
        result = await successTupleTask.Tap(IncreaseCounter);
        result.ShouldBeOfType<Result<(int,double)>>();
        _testCounter.ShouldBe(42 + 6.9);

        _testCounter = 0;
        result = await successTupleTask.TapAsync(IncreaseCounterTask);
        result.ShouldBeOfType<Result<(int,double)>>();
        _testCounter.ShouldBe(42 + 6.9);
    }

    private void IncreaseCounter(int value1, double value2)
    {
        _testCounter += value1 + value2;
    }
    
    private async Task IncreaseCounterTask(int value1, double value2)
    {
        await Task.Delay(0);
        _testCounter += value1 + value2;
    }
}