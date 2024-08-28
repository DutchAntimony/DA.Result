using DA.Results.Extensions;

namespace DA.Results.Tests;

public class CheckForEachTests : ResultExtensionsTestBase
{
    private readonly IEnumerable<int> _collection = [2, 3, 4];

    private Result NotAbove3(int value) => Result.OkIf(value <= 2, "Value is above 2");
    private Result NotAboveResult(int enumvalue, int resultvalue) => Result.OkIf(enumvalue < resultvalue, $"Value is above {resultvalue}");
    private Result NotBelowResult(int enumvalue, int resultvalue) => Result.OkIf(enumvalue >= resultvalue, $"Value is below {resultvalue}");
    private Result Succeeds(int _) => Result.Ok();

    [Fact]
    public void CheckForEach_Should_ReturnFailure_WhenTestFuncFindFailure()
    {
        var output = _success.CheckForEach(_collection, NotAbove3);
        output.IsSuccess.ShouldBeFalse();
        output.Messages.Count().ShouldBe(2);
        output.Messages.Last().ShouldBe("Value is above 2");
    }

    [Fact]
    public void CheckForEach_Should_ReturnSuccess_WhenTestFuncSucceeds()
    {
        var output = _success.CheckForEach(_collection, Succeeds);
        output.IsSuccess.ShouldBeTrue();
        output.Messages.Count().ShouldBe(1);
    }

    [Fact]
    public void CheckForEach_Should_ReturnFailure_WhenTestFuncOfTFindFailure()
    {
        var output = _successOfT.CheckForEach(_collection, NotAboveResult);
        output.IsSuccess(out var _).ShouldBeFalse();
        output.Messages.Count().ShouldBe(2);
        output.Messages.Last().ShouldBe("Value is above 1");
    }

    [Fact]
    public void CheckForEach_Should_ReturnSuccess_WhenTestFuncOfTSucceeds()
    {
        var output = _successOfT.CheckForEach(_collection, NotBelowResult);
        output.IsSuccess(out int value).ShouldBeTrue();
        value.ShouldBe(1);
        output.Messages.Count().ShouldBe(1);
    }

    [Fact]
    public async Task CheckForEach_Should_ReturnFailure_WhenPredicateTaskFails()
    {
        var output = await _success.CheckForEachAsync(_collection, (x) => Task.FromResult(NotAbove3(x)));
        output.IsSuccess.ShouldBeFalse();
        output.Messages.Count().ShouldBe(2);
        output.Messages.Last().ShouldBe("Value is above 2");
    }

    [Fact]
    public async Task CheckForEach_Should_ReturnFailureOfT_WhenPredicateOnTaskFails()
    {
        var output = await _successTaskOfT.CheckForEachAsync(_collection, NotAboveResult);
        output.IsSuccess(out var _).ShouldBeFalse();
        output.Messages.Count().ShouldBe(2);
        output.Messages.Last().ShouldBe("Value is above 1");
    }

    [Fact]
    public async Task CheckForEach_Should_ResultFailureOfT_WhenPredicateTaskOfTFails()
    {
        var outputOfT = await _successOfT.CheckForEachAsync(_collection, (x, y) => Task.FromResult(NotAboveResult(x, y)));
        outputOfT.IsSuccess(out var _).ShouldBeFalse();
        outputOfT.Messages.Count().ShouldBe(2);
        outputOfT.Messages.Last().ShouldBe("Value is above 1");
    }

    [Fact]
    public async Task CheckForEach_Should_ResultFailureOfT_WhenPredicateTaskOfTOnTaskOfTFails()
    {
        var outputOfT = await _successTaskOfT.CheckForEachAsync(_collection, (x, y) => Task.FromResult(NotAboveResult(x, y)));
        outputOfT.IsSuccess(out var _).ShouldBeFalse();
        outputOfT.Messages.Count().ShouldBe(2);
        outputOfT.Messages.Last().ShouldBe("Value is above 1");
    }

    [Fact]
    public async Task CheckForEach_Should_ResultFailure_WhenInputIsFailure()
    {
        var input = Task.FromResult(Result.Failure(_collection, "collection initialized"));
        var output = await input.CheckForEachAsync(Succeeds);
        output.IsSuccess(out var _).ShouldBeFalse();
        output.Messages.Count().ShouldBe(1);
    }

    [Fact]
    public async Task CheckForEach_Should_ResultFailure_WhenPredicateOnResultOfIEnumerableOfTFails()
    {
        var input = Task.FromResult(Result.Ok(_collection, "collection initialized"));
        var output = await input.CheckForEachAsync(NotAbove3);
        output.IsSuccess(out var _).ShouldBeFalse();
        output.Messages.Count().ShouldBe(2);
        output.Messages.Last().ShouldBe("Value is above 2");
    }

    [Fact]
    public async Task CheckForEach_Should_ResultSuccess_WhenPredicateOnResultOfIEnumerableOfTSucceeds()
    {
        var input = Task.FromResult(Result.Ok(_collection, "collection initialized"));
        var output = await input.CheckForEachAsync(Succeeds);
        output.IsSuccess(out var value).ShouldBeTrue();
        output.Messages.Count().ShouldBe(1);
        value.ShouldBeEquivalentTo(_collection);
    }

    [Fact]
    public async Task CheckForEach_Should_ResultFailure_WhenInputIsFailure_AndPredicateIsTask()
    {
        var input = Task.FromResult(Result.Failure(_collection, "collection initialized"));
        var output = await input.CheckForEachAsync((x) => Task.FromResult(Succeeds(x)));
        output.IsSuccess(out var _).ShouldBeFalse();
        output.Messages.Count().ShouldBe(1);
    }

    [Fact]
    public async Task CheckForEach_Should_ResultFailure_WhenPredicateTaskOnResultOfIEnumerableOfTFails()
    {
        var input = Task.FromResult(Result.Ok(_collection, "collection initialized"));
        var output = await input.CheckForEachAsync((x) => Task.FromResult(NotAbove3(x)));
        output.IsSuccess(out var _).ShouldBeFalse();
        output.Messages.Count().ShouldBe(2);
        output.Messages.Last().ShouldBe("Value is above 2");
    }

    [Fact]
    public async Task CheckForEach_Should_ResultSuccess_WhenPredicateTaskOnResultOfIEnumerableOfTSucceeds()
    {
        var input = Task.FromResult(Result.Ok(_collection, "collection initialized"));
        var output = await input.CheckForEachAsync((x) => Task.FromResult(Succeeds(x)));
        output.IsSuccess(out var value).ShouldBeTrue();
        output.Messages.Count().ShouldBe(1);
        value.ShouldBeEquivalentTo(_collection);
    }
}