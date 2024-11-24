namespace DA.Results.Tests.Extensions.Map;

public class MapDependentTests : ResultTestBase
{
    [Fact]
    public void Map_Should_NotCallResultType_OnFailure()
    {
        var result = DoubleInternalServerErrorResult.Map(value => ThrowingValue(value));
        result.ShouldHaveIssue<InternalServerError>();
        result.ShouldBeOfType<Result<int>>();
    }

    [Fact]
    public void Map_Should_UpdateValue_OnSuccess()
    {
        var result = DoubleSuccessResult.Map(dbl => 42*dbl);
        result.ShouldBeSuccessWithValue(42*6.9);
        result.ShouldBeOfType<Result<double>>();
    }

    [Fact]
    public async Task Map_Should_BehaveCorrectOnDifferentAsyncOverloads()
    {
        var resultTask = Task.FromResult(DoubleSuccessResult);
        
        (await DoubleSuccessResult.MapAsync(ValueTask)).ShouldBeSuccessWithValue(42);
        (await resultTask.Map(_ => 42)).ShouldBeSuccessWithValue(42);
        (await resultTask.MapAsync(ValueTask)).ShouldBeSuccessWithValue(42);
        return;
        
        Task<int> ValueTask(double original) => Task.FromResult(42);
    }
}