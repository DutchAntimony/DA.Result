namespace DA.Results.Tests.Extensions;

public class TapTests : ResultTestBase
{
    private int _testCounter = 0;
    
    [Fact]
    public void Tap_Should_CallAction_OnSuccessResult()
    {
        var before = _testCounter;
        var result = IntSuccessResult.Tap(IncreaseCounterWithValue);
        _testCounter.ShouldBe(before + 1);
        result.ShouldBeOfType<Result<int>>();
    }
    
    [Fact]
    public void Tap_Should_CallActionOfT_OnSuccessResult()
    {
        var before = _testCounter;
        var result = IntSuccessResult.Tap(IncreaseCounter);
        _testCounter.ShouldBe(before + 42);
        result.ShouldBeOfType<Result<int>>();
    }
    
    [Fact]
    public void Tap_Should_CallAction_OnSuccessResultOfT()
    {
        var before = _testCounter;
        var result = IntSuccessResult.Tap(IncreaseCounter);
        _testCounter.ShouldBe(before + 42);
        result.ShouldBeOfType<Result<int>>();
    }

    [Fact]
    public void Tap_Should_NotCallAction_OnFailureResult()
    {
        var result = InternalServerErrorResult.Tap(() => throw new ShouldAssertException("Should not call action"));
        result.ShouldBeOfType<NoContentResult>();
    }
    
    [Fact]
    public void Tap_Should_NotCallFunction_OnFailureResult()
    {
        var result = InternalServerErrorResult.Tap(() => ThrowingValue());
        result.ShouldBeOfType<NoContentResult>();
    }
    
    [Fact]
    public void Tap_Should_NotCallFunction_OnFailureResultOfT()
    {
        var result = DoubleInternalServerErrorResult.Tap(() => ThrowingValue());
        result.ShouldBeOfType<Result<double>>();
    }

    [Fact]
    public async Task Tap_Should_BehaveCorrectOnDifferentAsyncOverloads()
    {
        var intResultTask = Task.FromResult(IntSuccessResult);
        
        var before = _testCounter;
        var result = await IntSuccessResult.TapAsync(IncreaseCounterTask);
        result.ShouldBeOfType<Result<int>>();
        _testCounter.ShouldBe(before + 42);
        
        result = await IntInternalServerErrorResult.TapAsync(value => ThrowingValueTask(value));
        result.ShouldBeOfType<Result<int>>();

        before = _testCounter;
        result = await intResultTask.Tap(IncreaseCounterWithValue);
        result.ShouldBeOfType<Result<int>>();
        _testCounter.ShouldBe(before + 1);
        
        before = _testCounter;
        result = await intResultTask.TapAsync(() =>
        {
            IncreaseCounterWithValue();
            return Task.CompletedTask;
        });
        
        result.ShouldBeOfType<Result<int>>();
        _testCounter.ShouldBe(before + 1);
        
        before = _testCounter;
        result = await intResultTask.Tap(IncreaseCounter);
        result.ShouldBeOfType<Result<int>>();
        _testCounter.ShouldBe(before + 42);

        before = _testCounter;
        result = await intResultTask.TapAsync(IncreaseCounterTask);
        result.ShouldBeOfType<Result<int>>();
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
    
    private async Task IncreaseCounterTask(int value)
    {
        await Task.Delay(0);
        _testCounter += value;
    }
}