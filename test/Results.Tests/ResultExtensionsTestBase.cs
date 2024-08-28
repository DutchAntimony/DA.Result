using DA.Results.Extensions;

namespace DA.Results.Tests;

public class ResultExtensionsTestBase
{
    protected readonly Result _success;
    protected readonly Result _failure;
    protected readonly Result<int> _successOfT;
    protected readonly Result<int> _failureOfT;
    protected readonly Task<Result> _successTask;
    protected readonly Task<Result> _failureTask;
    protected readonly Task<Result<int>> _successTaskOfT;
    protected readonly Task<Result<int>> _failureTaskOfT;

    public ResultExtensionsTestBase()
    {
        _success = Result.Ok("ok");
        _failure = Result.Failure("failure");
        _successOfT = Result.Ok(1, "okWithValue");
        _failureOfT = Result.Failure<int>("failureWithValue");
        _successTask = Task.FromResult(_success);
        _failureTask = Task.FromResult(_failure);
        _successTaskOfT = Task.FromResult(_successOfT);
        _failureTaskOfT = Task.FromResult(_failureOfT);
    }
}

public class MatchTests : ResultExtensionsTestBase
{
    [Fact]
    public void Match_WithStaticValues_ReturnsExpectedResult()
    {
        var successOutput = _success.Match("Success", "Failure");
        var failureOutput = _failure.Match("Success", "Failure");

        successOutput.ShouldBe("Success");
        failureOutput.ShouldBe("Failure");
    }

    [Fact]
    public void Match_WithFunctions_ReturnsExpectedResult()
    {
        var successOutput = _success.Match(() => "Success", () => "Failure");
        var failureOutput = _failure.Match(() => "Success", () => "Failure");

        successOutput.ShouldBe("Success");
        failureOutput.ShouldBe("Failure");
    }

    [Fact]
    public async Task MatchAsync_WithStaticTasks_ReturnsExpectedResult()
    {
        var successOutput = await _success.MatchAsync(Task.FromResult("Success"), Task.FromResult("Failure"));
        var failureOutput = await _failure.MatchAsync(Task.FromResult("Success"), Task.FromResult("Failure"));

        successOutput.ShouldBe("Success");
        failureOutput.ShouldBe("Failure");
    }

    [Fact]
    public async Task MatchAsync_WithFunctionTasks_ReturnsExpectedResult()
    {
        var successOutput = await _success.MatchAsync(() => Task.FromResult("Success"), () => Task.FromResult("Failure"));
        var failureOutput = await _failure.MatchAsync(() => Task.FromResult("Success"), () => Task.FromResult("Failure"));

        successOutput.ShouldBe("Success");
        failureOutput.ShouldBe("Failure");
    }

    [Fact]
    public async Task MatchAsync_WithTaskResult_ReturnsExpectedResult()
    {
        var successOutput = await _successTask.MatchAsync(Task.FromResult("Success"), Task.FromResult("Failure"));
        var failureOutput = await _failureTask.MatchAsync(Task.FromResult("Success"), Task.FromResult("Failure"));

        successOutput.ShouldBe("Success");
        failureOutput.ShouldBe("Failure");
    }

    [Fact]
    public async Task MatchAsync_WithTaskFunctionResult_ReturnsExpectedResult()
    {
        var successOutput = await _successTask.MatchAsync(() => Task.FromResult("Success"), () => Task.FromResult("Failure"));
        var failureOutput = await _failureTask.MatchAsync(() => Task.FromResult("Success"), () => Task.FromResult("Failure"));

        successOutput.ShouldBe("Success");
        failureOutput.ShouldBe("Failure");
    }

    [Fact]
    public void Match_WithAction_PerformsCorrectAction()
    {
        bool successActionCalled = false;
        bool failureActionCalled = false;

        _success.Match(() => successActionCalled = true, () => failureActionCalled = true);
        successActionCalled.ShouldBeTrue();
        failureActionCalled.ShouldBeFalse();

        successActionCalled = false;
        failureActionCalled = false;

        _failure.Match(() => successActionCalled = true, () => failureActionCalled = true);
        successActionCalled.ShouldBeFalse();
        failureActionCalled.ShouldBeTrue();
    }

    [Fact]
    public void Match_WithMessages_PerformsCorrectAction()
    {
        IEnumerable<string> receivedMessages = [];

        _success.Match(messages => receivedMessages = messages, _ => { });
        receivedMessages.ShouldBeEquivalentTo(_success.Messages);

        _failure.Match(_ => { }, messages => receivedMessages = messages);
        receivedMessages.ShouldBeEquivalentTo(_failure.Messages);
    }

    [Fact]
    public async Task MatchAsync_WithTaskMessages_PerformsCorrectAction()
    {
        IEnumerable<string> receivedMessages = [];

        await _successTask.MatchAsync(messages => receivedMessages = messages, _ => { });
        receivedMessages.ShouldBeEquivalentTo(_success.Messages);

        await _failureTask.MatchAsync(_ => { }, messages => receivedMessages = messages);
        receivedMessages.ShouldBeEquivalentTo(_failure.Messages);
    }

    [Fact]
    public void Match_WithGenericResult_ReturnsExpectedValue()
    {
        var successOutput = _successOfT.Match(value => value.ToString(), () => "Failure");
        var failureOutput = _failureOfT.Match(value => value.ToString(), () => "Failure");

        successOutput.ShouldBe("1");
        failureOutput.ShouldBe("Failure");
    }

    [Fact]
    public async Task MatchAsync_WithGenericTaskResult_ReturnsExpectedValue()
    {
        var successOutput = await _successTaskOfT.MatchAsync(value => value.ToString(), () => "Failure");
        var failureOutput = await _failureTaskOfT.MatchAsync(value => value.ToString(), () => "Failure");

        successOutput.ShouldBe("1");
        failureOutput.ShouldBe("Failure");
    }
}