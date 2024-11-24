namespace DA.Results.Tests.Extensions.Map;

public class MapIndependentTests : ResultTestBase
{
    [Fact]
    public void Map_Should_RemainFailure_OnFailure()
    {
        var result = DoubleInternalServerErrorResult.Map(42);
        result.ShouldHaveIssue<InternalServerError>();
        result.ShouldBeOfType<Result<int>>();
    }
    
    [Fact]
    public void Map_Should_AddValue_OnSuccess()
    {
        var result = DoubleSuccessResult.Map(42);
        result.ShouldBeSuccessWithValue(42);
        result.ShouldBeOfType<Result<int>>();
    }
    
    [Fact]
    public void Map_Should_NotCallResultType_OnFailure()
    {
        var result = DoubleInternalServerErrorResult.Map(_ => ThrowingValue());
        result.ShouldHaveIssue<InternalServerError>();
        result.ShouldBeOfType<Result<int>>();
    }

    [Fact]
    public void Map_Should_AddValueFromFunc_OnSuccess()
    {
        var result = DoubleSuccessResult.Map(_ => 42);
        result.ShouldBeSuccessWithValue(42);
        result.ShouldBeOfType<Result<int>>();
    }

    [Fact]
    public async Task Map_Should_BehaveCorrectOnDifferentAsyncOverloads()
    {
        var resultTask = Task.FromResult(DoubleSuccessResult);
        var valueTask = Task.FromResult(42);
        
        (await DoubleSuccessResult.MapAsync(() => valueTask)).ShouldBeSuccessWithValue(42);
        
        var failingResult = await IntInternalServerErrorResult.MapAsync(value => ThrowingValueTask(value));
        failingResult.ShouldHaveIssue<InternalServerError>();
        failingResult.ShouldBeOfType<Result<int>>();
        
        (await resultTask.Map(42)).ShouldBeSuccessWithValue(42);
        (await resultTask.Map(() => 42)).ShouldBeSuccessWithValue(42);
        (await resultTask.MapAsync(() => valueTask)).ShouldBeSuccessWithValue(42);
    }
}