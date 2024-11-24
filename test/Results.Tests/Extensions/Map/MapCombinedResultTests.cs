namespace DA.Results.Tests.Extensions.Map;

public class MapCombinedResultTests : ResultTestBase
{
    private readonly Result<(int, double)> _tupleSuccessResult = (42, 6.9);
    
    [Fact]
    public void Map_Should_NotCallResultType_OnFailure()
    {
        var tupleFailingResult = _tupleSuccessResult.Check(_ => InternalServerErrorResult);
        var result = tupleFailingResult.Map((i,d) => ThrowingValue(i,d));
        result.ShouldHaveIssue<InternalServerError>();
        result.ShouldBeOfType<Result<int>>();
    }

    [Fact]
    public void Map_Should_UpdateValue_OnSuccess()
    {
        var result = _tupleSuccessResult.Map((i,d) => i*d);
        result.ShouldBeSuccessWithValue(42*6.9);
        result.ShouldBeOfType<Result<double>>();
    }

    [Fact]
    public async Task Map_Should_BehaveCorrectOnDifferentAsyncOverloads()
    {
        var resultTask = Task.FromResult(_tupleSuccessResult);
        (await _tupleSuccessResult.MapAsync(ValueTask)).ShouldBeSuccessWithValue(42);
        (await resultTask.Map((i,d) => i*d)).ShouldBeSuccessWithValue(42*6.9);
        (await resultTask.MapAsync(ValueTask)).ShouldBeSuccessWithValue(42);
        return;
        
        Task<int> ValueTask(int v1, double v2) => Task.FromResult(42);
    }
}