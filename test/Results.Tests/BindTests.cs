using DA.Results.Extensions;
using System.Diagnostics.CodeAnalysis;

namespace DA.Results.Tests;

public class BindTests : ResultExtensionsTestBase
{

    #region Local function to support unit testing
    private static Result ResultFunc()
    {
        return Result.Ok("FromFunc");
    }

    private static Result<bool> ResultFuncOfT()
    {
        return Result.Ok(true, "FromFunc");
    }

    private static Result<bool> ResultFuncOfT(int value)
    {
        return Result.Ok(value < 3, "FromFunc");
    }

    private static Task<Result<bool>> ResultFuncOfTAsync(int value)
    {
        return Task.FromResult(Result.Ok(value < 3, "FromFunc"));
    }

    [ExcludeFromCodeCoverage(Justification = "Should never be called; that is the test.")]
    private static Result AssertNotCalledResultFunc()
    {
        throw new ShouldAssertException("Func should not be called.");
    }

    [ExcludeFromCodeCoverage(Justification = "Should never be called; that is the test.")]
    private static Result<bool> AssertNotCalledResultFuncOfT()
    {
        throw new ShouldAssertException("Func should not be called.");
    }

    [ExcludeFromCodeCoverage(Justification = "Should never be called; that is the test.")]
    private static Result<bool> AssertNotCalledResultFuncOfT(int value)
    {
        throw new ShouldAssertException("Func should not be called.");
    }

    [ExcludeFromCodeCoverage(Justification = "Should never be called; that is the test.")]
    private static Task<Result<bool>> AssertNotCalledResultFuncOfTAsync(int value)
    {
        throw new ShouldAssertException("Func should not be called.");
    }

    #endregion

    #region Bind Result to Result
    [Fact]
    public void Bind_Should_CreateFailure_WhenSuccessIsBindedWithFailure()
    {
        var output = _success.Bind(_failure);
        output.IsSuccess.ShouldBeFalse();
        output.Messages.Count().ShouldBe(2);
        output.Messages.First().ShouldBe("ok");
    }

    [Fact]
    public void Bind_Should_CreateSuccess_WhenSuccessIsBindedWithSuccess()
    {
        var output = _success.Bind(_success);
        output.IsSuccess.ShouldBeTrue();
        output.Messages.Count().ShouldBe(2);
    }

    [Fact]
    public void Bind_Should_NotCallFunction_WhenFailureIsBinded()
    {
        var output = _failure.Bind(AssertNotCalledResultFunc);
        output.IsSuccess.ShouldBeFalse();
        output.Messages.Count().ShouldBe(1);
    }

    [Fact]
    public void Bind_Should_CallFunction_WhenSuccessIsBinded()
    {
        var output = _success.Bind(ResultFunc);
        output.IsSuccess.ShouldBeTrue();
        output.Messages.Count().ShouldBe(2);
    }

    [Fact]
    public async Task BindAsync_Should_NotCallFunction_WhenFailureIsBinded()
    {
        var output = await _failure.BindAsync(Task.FromResult(_success));
        output.IsSuccess.ShouldBeFalse();
        output.Messages.Count().ShouldBe(1);
    }

    [Fact]
    public async Task BindAsync_Should_CallFunction_WhenSuccessIsBinded()
    {
        var output = await _success.BindAsync(Task.FromResult(ResultFunc()));
        output.IsSuccess.ShouldBeTrue();
        output.Messages.Count().ShouldBe(2);
    }

    [Fact]
    public async Task BindAsync_Should_StayFailing_WhenFailureTaskIsBinded()
    {
        var output = await Task.FromResult(_failure).BindAsync(_success);
        output.IsSuccess.ShouldBeFalse();
        output.Messages.Count().ShouldBe(1);
    }

    [Fact]
    public async Task BindAsync_Should_CombineResults_WhenSuccessTaskIsBinded()
    {
        var output = await Task.FromResult(_success).BindAsync(_success);
        output.IsSuccess.ShouldBeTrue();
        output.Messages.Count().ShouldBe(2);
    }

    [Fact]
    public async Task BindAsync_Should_NotCallFunction_WhenFailureTaskIsBinded()
    {
        var output = await Task.FromResult(_failure).BindAsync(AssertNotCalledResultFunc);
        output.IsSuccess.ShouldBeFalse();
        output.Messages.Count().ShouldBe(1);
    }

    [Fact]
    public async Task BindAsync_Should_CallFunction_WhenSuccessTaskIsBinded()
    {
        var output = await Task.FromResult(_success).BindAsync(ResultFunc);
        output.IsSuccess.ShouldBeTrue();
        output.Messages.Count().ShouldBe(2);
    }

    [Fact]
    public async Task BindAsync_Should_NotAwaitTask_WhenFailureTaskIsBinded()
    {
        var output = await Task.FromResult(_failure).BindAsync(Task.FromResult(_success));
        output.IsSuccess.ShouldBeFalse();
        output.Messages.Count().ShouldBe(1);
    }

    [Fact]
    public async Task BindAsync_Should_AwaitTask_WhenSuccessTaskIsBinded()
    {
        var output = await Task.FromResult(_success).BindAsync(Task.FromResult(_success));
        output.IsSuccess.ShouldBeTrue();
        output.Messages.Count().ShouldBe(2);
    }
    #endregion

    #region Bind Result to Result{T}
    [Fact]
    public void Bind_Should_CreateFailure_WhenSuccessIsBindedWithFailureOfT()
    {
        var output = _success.Bind(_failureOfT);
        output.IsSuccess(out var _).ShouldBeFalse();
        output.Messages.Count().ShouldBe(2);
        output.Messages.First().ShouldBe("ok");
    }

    [Fact]
    public void Bind_Should_CreateSuccess_WhenSuccessIsBindedWithSuccessOfT()
    {
        var output = _success.Bind(_successOfT);
        output.IsSuccess(out var value).ShouldBeTrue();
        value.ShouldBe(1);
        output.Messages.Count().ShouldBe(2);
    }

    [Fact]
    public void Bind_Should_NotCallFunction_WhenFailureIsBindedWithAnyOfT()
    {
        var output = _failure.Bind(AssertNotCalledResultFuncOfT);
        output.IsSuccess(out var _).ShouldBeFalse();
        output.Messages.Count().ShouldBe(1);
    }

    [Fact]
    public void Bind_Should_CallFunction_WhenSuccessIsBindedWithAnyOfT()
    {
        var output = _success.Bind(ResultFuncOfT);
        output.IsSuccess(out var value).ShouldBeTrue();
        value.ShouldBe(true);
        output.Messages.Count().ShouldBe(2);
    }

    [Fact]
    public async Task BindAsync_Should_NotCallFunction_WhenFailureIsBindedWithAnyOfT()
    {
        var output = await _failure.BindAsync(_successTaskOfT);
        output.IsSuccess(out var _).ShouldBeFalse();
        output.Messages.Count().ShouldBe(1);
    }

    [Fact]
    public async Task BindAsync_Should_CallFunction_WhenSuccessIsBindedWithAnyOfT()
    {
        var output = await _success.BindAsync(Task.FromResult(ResultFuncOfT()));
        output.IsSuccess(out var value).ShouldBeTrue();
        value.ShouldBe(true);
        output.Messages.Count().ShouldBe(2);
    }

    [Fact]
    public async Task BindAsync_Should_StayFailing_WhenFailureTaskIsBindedWithAnyOfT()
    {
        var output = await _failureTask.BindAsync(_successOfT);
        output.IsSuccess(out var _).ShouldBeFalse();
        output.Messages.Count().ShouldBe(2);
    }

    [Fact]
    public async Task BindAsync_Should_CombineResults_WhenSuccessTaskIsBindedWithAnyOfT()
    {
        var output = await _successTask.BindAsync(_successOfT);
        output.IsSuccess(out var value).ShouldBeTrue();
        value.ShouldBe(1);
        output.Messages.Count().ShouldBe(2);
    }

    [Fact]
    public async Task BindAsync_Should_NotCallFunction_WhenFailureTaskIsBindedWithAnyOfT()
    {
        var output = await _failureTask.BindAsync(AssertNotCalledResultFuncOfT);
        output.IsSuccess(out var _).ShouldBeFalse();
        output.Messages.Count().ShouldBe(1);
    }

    [Fact]
    public async Task BindAsync_Should_CallFunction_WhenSuccessTaskIsBindedWithAnyOfT()
    {
        var output = await _successTask.BindAsync(ResultFuncOfT);
        output.IsSuccess(out var value).ShouldBeTrue();
        value.ShouldBe(true);
        output.Messages.Count().ShouldBe(2);
    }

    [Fact]
    public async Task BindAsync_Should_NotAwaitTask_WhenFailureTaskIsBindedWithAnyOfT()
    {
        var output = await _failureTask.BindAsync(Task.FromResult(_successOfT));
        output.IsSuccess(out var _).ShouldBeFalse();
        output.Messages.Count().ShouldBe(1);
    }

    [Fact]
    public async Task BindAsync_Should_AwaitTask_WhenSuccessTaskIsBindedWithAnyOfT()
    {
        var output = await _successTask.BindAsync(Task.FromResult(_successOfT));
        output.IsSuccess(out var value).ShouldBeTrue();
        value.ShouldBe(1);
        output.Messages.Count().ShouldBe(2);
    }
    #endregion

    #region Bind Result{TIn} to Result{TOut}

    [Fact]
    public void Bind_Should_CreateFailure_WhenSuccessOfTIsBindedWithFailureOfT()
    {
        var output = _successOfT.Bind(_failureOfT);
        output.IsSuccess(out var _).ShouldBeFalse();
        output.Messages.Count().ShouldBe(2);
        output.Messages.First().ShouldBe("okWithValue");
    }

    [Fact]
    public void Bind_Should_CreateSuccess_WhenSuccessOfTIsBindedWithSuccessOfT()
    {
        var output = _successOfT.Bind(_successOfT);
        output.IsSuccess(out var value).ShouldBeTrue();
        value.ShouldBe(1);
        output.Messages.Count().ShouldBe(2);
    }

    [Fact]
    public void Bind_Should_NotCallFunction_WhenFailureOfTIsBindedWithAnyOfT()
    {
        var output = _failureOfT.Bind(AssertNotCalledResultFuncOfT);
        output.IsSuccess(out var _).ShouldBeFalse();
        output.Messages.Count().ShouldBe(1);
    }

    [Fact]
    public void Bind_Should_CallFunction_WhenSuccessOfTIsBindedWithAnyOfT()
    {
        var output = _successOfT.Bind(ResultFuncOfT);
        output.IsSuccess(out var value).ShouldBeTrue();
        value.ShouldBe(true);
        output.Messages.Count().ShouldBe(2);
    }

    [Fact]
    public async Task BindAsync_Should_NotCallFunction_WhenFailureOfTIsBindedWithAnyOfT()
    {
        var output = await _failureOfT.BindAsync(_successTaskOfT);
        output.IsSuccess(out var _).ShouldBeFalse();
        output.Messages.Count().ShouldBe(1);
    }

    [Fact]
    public async Task BindAsync_Should_CallFunction_WhenSuccessOfTIsBindedWithAnyOfT()
    {
        var output = await _successOfT.BindAsync(Task.FromResult(ResultFuncOfT()));
        output.IsSuccess(out var value).ShouldBeTrue();
        value.ShouldBe(true);
        output.Messages.Count().ShouldBe(2);
    }

    [Fact]
    public async Task BindAsync_Should_StayFailing_WhenFailureOfTTaskIsBindedWithAnyOfT()
    {
        var output = await _failureTaskOfT.BindAsync(_successOfT);
        output.IsSuccess(out var _).ShouldBeFalse();
        output.Messages.Count().ShouldBe(2);
    }

    [Fact]
    public async Task BindAsync_Should_CombineResults_WhenSuccessOfTTaskIsBindedWithAnyOfT()
    {
        var output = await _successTaskOfT.BindAsync(_successOfT);
        output.IsSuccess(out var value).ShouldBeTrue();
        value.ShouldBe(1);
        output.Messages.Count().ShouldBe(2);
    }

    [Fact]
    public async Task BindAsync_Should_NotCallFunction_WhenFailureOfTTaskIsBindedWithAnyOfT()
    {
        var output = await _failureTaskOfT.BindAsync(AssertNotCalledResultFuncOfT);
        output.IsSuccess(out var _).ShouldBeFalse();
        output.Messages.Count().ShouldBe(1);
    }

    [Fact]
    public async Task BindAsync_Should_CallFunction_WhenSuccessOfTTaskIsBindedWithAnyOfT()
    {
        var output = await _successTaskOfT.BindAsync(ResultFuncOfT);
        output.IsSuccess(out var value).ShouldBeTrue();
        value.ShouldBe(true);
        output.Messages.Count().ShouldBe(2);
    }

    [Fact]
    public async Task BindAsync_Should_NotAwaitTask_WhenFailureOfTTaskIsBindedWithAnyOfT()
    {
        var output = await _failureTaskOfT.BindAsync(_successTaskOfT);
        output.IsSuccess(out var _).ShouldBeFalse();
        output.Messages.Count().ShouldBe(1);
    }

    [Fact]
    public async Task BindAsync_Should_AwaitTask_WhenSuccessOfTTaskIsBindedWithAnyOfT()
    {
        var output = await _successTaskOfT.BindAsync(_successTaskOfT);
        output.IsSuccess(out var value).ShouldBeTrue();
        value.ShouldBe(1);
        output.Messages.Count().ShouldBe(2);
    }

    [Fact]
    public async Task BindAsync_Should_NotAwaitTask_WhenFailureOfTTasFunckIsBindedWithAnyOfT()
    {
        var output = await _failureTaskOfT.BindAsync(AssertNotCalledResultFuncOfTAsync);
        output.IsSuccess(out var _).ShouldBeFalse();
        output.Messages.Count().ShouldBe(1);
    }

    [Fact]
    public async Task BindAsync_Should_AwaitTask_WhenSuccessOfTTaskFuncIsBindedWithAnyOfT()
    {
        var output = await _successTaskOfT.BindAsync(ResultFuncOfTAsync);
        output.IsSuccess(out var value).ShouldBeTrue();
        value.ShouldBe(true);
        output.Messages.Count().ShouldBe(2);
    }
    #endregion
}