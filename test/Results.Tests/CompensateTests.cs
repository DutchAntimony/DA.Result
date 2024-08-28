using DA.Results.Extensions;

namespace DA.Results.Tests;

public class CompensateTests : ResultExtensionsTestBase
{
    private readonly Result _compensateResult;
    private readonly Func<Result> _compensateFunc;
    private readonly Func<Task<Result>> _compensateTask;

    private readonly Result<int> _compensateResultOfT;
    private readonly Func<Result<int>> _compensateFuncOfT;
    private readonly Func<Task<Result<int>>> _compensateTaskOfT;

    public CompensateTests()
    {
        _compensateResult = Result.Ok("compensatedResult");
        _compensateFunc = () => _compensateResult;
        _compensateTask = () => Task.FromResult(_compensateResult);

        _compensateResultOfT = Result.Ok(2, "compensatedResult");
        _compensateFuncOfT = () => _compensateResultOfT;
        _compensateTaskOfT = () => Task.FromResult(_compensateResultOfT);
    }

    [Fact]
    public async Task Compensate_ShouldNotOverride_ASuccessResult()
    {
        var unmodifiedMessage = _success.Messages.First();
        var unmodifiedMessageOfT = _successOfT.Messages.First();

        _success.Compensate(_compensateFunc).Messages.First().ShouldBe(unmodifiedMessage);
        (await _success.CompensateAsync(_compensateTask)).Messages.First().ShouldBe(unmodifiedMessage);
        (await _successTask.CompensateAsync(_compensateFunc)).Messages.First().ShouldBe(unmodifiedMessage);
        (await _successTask.CompensateAsync(_compensateTask)).Messages.First().ShouldBe(unmodifiedMessage);

        _successOfT.Compensate(_compensateFuncOfT).Messages.First().ShouldBe(unmodifiedMessageOfT);
        (await _successOfT.CompensateAsync(_compensateTaskOfT)).Messages.First().ShouldBe(unmodifiedMessageOfT);
        (await _successTaskOfT.CompensateAsync(_compensateFuncOfT)).Messages.First().ShouldBe(unmodifiedMessageOfT);
        (await _successTaskOfT.CompensateAsync(_compensateTaskOfT)).Messages.First().ShouldBe(unmodifiedMessageOfT);
    }

    [Fact]
    public async Task Compensate_ShouldOverride_AFailureResult()
    {
        var modifiedMessage = _compensateResult.Messages.First();
        var modifiedMessageOfT = _compensateResultOfT.Messages.First();

        _failure.Compensate(_compensateFunc).Messages.First().ShouldBe(modifiedMessage);
        (await _failure.CompensateAsync(_compensateTask)).Messages.First().ShouldBe(modifiedMessage);
        (await _failureTask.CompensateAsync(_compensateFunc)).Messages.First().ShouldBe(modifiedMessage);
        (await _failureTask.CompensateAsync(_compensateTask)).Messages.First().ShouldBe(modifiedMessage);

        _failureOfT.Compensate(_compensateFuncOfT).Messages.First().ShouldBe(modifiedMessageOfT);
        (await _failureOfT.CompensateAsync(_compensateTaskOfT)).Messages.First().ShouldBe(modifiedMessageOfT);
        (await _failureTaskOfT.CompensateAsync(_compensateFuncOfT)).Messages.First().ShouldBe(modifiedMessageOfT);
        (await _failureTaskOfT.CompensateAsync(_compensateTaskOfT)).Messages.First().ShouldBe(modifiedMessageOfT);
    }
}