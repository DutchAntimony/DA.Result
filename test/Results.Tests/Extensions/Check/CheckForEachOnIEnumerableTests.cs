namespace DA.Results.Tests.Extensions.Check;

public class CheckForEachOnIEnumerableTests : ResultTestBase
{
    private readonly Result<IEnumerable<string>> _successResult = (List<string>) ["A", "B", "C", "D", "E", "F"]; 
    private readonly Result<IEnumerable<string>> _failingResult = (List<string>) ["A", "B", "C", "D", "E", "G"]; 

    private IResult Predicate(string item)
    {
        return item == "G"
            ? new ValidationError("Value", "Value must not be G")
            : SuccessResult;
    }
    
    private Task<IResult> PredicateTask(string item)
    {
        return Task.FromResult(Predicate(item));
    }

    [Fact]
    public void CheckForEach_Should_ReturnSuccess_IfNoFailingValuesAreProvided()
    {
        var result = _successResult.CheckForEach(Predicate);
        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public void CheckForEach_Should_ReturnFailure_IfFailingValuesAreProvided()
    {
        var result = _failingResult.CheckForEach(Predicate);
        result.ShouldHaveIssue<ValidationError>();
    }

    [Fact]
    public void CheckForEach_Should_RemainFailure_WhenStartingSuccessIsFailure()
    {
        var failingResult =_successResult.Check(_ => InternalServerErrorResult);
        var result = failingResult.CheckForEach(Predicate);
        result.ShouldHaveIssue<InternalServerError>();
    }

    [Fact]
    public async Task CheckForEachAsync_Should_BehaveCorrectOnDifferentAsyncOverloads()
    {
        var successTask = Task.FromResult(_successResult);
        (await successTask.CheckForEach(Predicate)).ShouldBeSuccess();
        (await _successResult.CheckForEachAsync(PredicateTask)).ShouldBeSuccess();
        (await _failingResult.CheckForEachAsync(PredicateTask)).ShouldHaveIssue<ValidationError>();
        (await successTask.CheckForEachAsync(PredicateTask)).ShouldBeSuccess();
    }
}