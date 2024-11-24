namespace DA.Results.Tests.Extensions.Check;

public class CheckForEachTests : ResultTestBase
{
    private readonly List<string> _passingValues = ["A", "B", "C", "D", "E", "F"];
    private readonly List<string> _failingValues = ["A", "B", "C", "D", "E", "G"];

    private IResult Predicate(string item, int value)
    {
        return item == "G"
            ? new ValidationError("Value", "Value must not be G")
            : SuccessResult;
    }

    private Task<IResult> PredicateTask(string item, int value)
    {
        return Task.FromResult(Predicate(item, value));
    }

    [Fact]
    public void CheckForEach_Should_ReturnSuccess_IfNoFailingValuesAreProvided()
    {
        var result = IntSuccessResult.CheckForEach(_passingValues, Predicate);
        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public void CheckForEach_Should_ReturnFailure_IfFailingValuesAreProvided()
    {
        var result = IntSuccessResult.CheckForEach(_failingValues, Predicate);
        result.ShouldHaveIssue<ValidationError>();
    }

    [Fact]
    public void CheckForEach_Should_RemainFailure_WhenStartingSuccessIsFailure()
    {
        var result = IntInternalServerErrorResult.CheckForEach(_failingValues, Predicate);
        result.ShouldHaveIssue<InternalServerError>();
    }

    [Fact]
    public async Task CheckForEachAsync_Should_BehaveCorrectOnDifferentAsyncOverloads()
    {
        var successTask = Task.FromResult(IntSuccessResult);
        (await IntSuccessResult.CheckForEachAsync(_passingValues, PredicateTask)).ShouldBeSuccess();
        (await IntInternalServerErrorResult.CheckForEachAsync(_passingValues, PredicateTask)).ShouldHaveIssue<InternalServerError>();
        (await successTask.CheckForEachAsync(_passingValues, PredicateTask)).ShouldBeSuccess();
        (await successTask.CheckForEach(_passingValues, Predicate)).ShouldBeSuccess();
    }
}