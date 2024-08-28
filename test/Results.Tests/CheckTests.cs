using DA.Results.Extensions;

namespace DA.Results.Tests;
public class CheckTests : ResultExtensionsTestBase
{
    public static Result CheckFunc() => Result.Ok("Checked");
    public static Result<bool> CheckFuncOfT() => Result.Ok(true, "Checked");
    public static Result CheckFunc(int value) => Result.Ok($"Checked {value}");
    public static Result CheckFuncOfT(int value) => Result.Ok(value < 1, $"Checked {value}");

    [Fact]
    public void Check_OfResult_FromResult()
    {
        var result = _success.Check(_failure);
        result.IsSuccess.ShouldBeFalse();
        result.Messages.Count().ShouldBe(2);
    }

    [Fact]
    public async Task Check_OnSuccessResult_Should_ReturnResultOfCheck()
    {
        _success.Check(CheckFunc).IsSuccess.ShouldBeTrue();
        _success.Check(CheckFuncOfT).IsSuccess.ShouldBeTrue();
        _successOfT.Check(CheckFunc).IsSuccess(out int _).ShouldBeTrue();
        _successOfT.Check(CheckFuncOfT).IsSuccess(out int _).ShouldBeTrue();
        (await _successOfT.CheckAsync(Task.FromResult(_success))).IsSuccess(out int _).ShouldBeTrue();
        (await _successOfT.CheckAsync(_successTaskOfT)).IsSuccess(out int _).ShouldBeTrue();
        (await _successOfT.CheckAsync((x) => Task.FromResult(CheckFunc(x)))).IsSuccess(out int _).ShouldBeTrue();
        (await _successOfT.CheckAsync((x) => Task.FromResult(CheckFuncOfT(x)))).IsSuccess(out int _).ShouldBeTrue();
        (await _successTaskOfT.CheckAsync(_success)).IsSuccess(out int _).ShouldBeTrue();
        (await _successTaskOfT.CheckAsync(CheckFunc)).IsSuccess(out int _).ShouldBeTrue();
        (await _successTaskOfT.CheckAsync(CheckFuncOfT)).IsSuccess(out int _).ShouldBeTrue();
        (await _successTaskOfT.CheckAsync(Task.FromResult(_success))).IsSuccess(out int _).ShouldBeTrue();
        (await _successTaskOfT.CheckAsync(_successTaskOfT)).IsSuccess(out int _).ShouldBeTrue();
        (await _successTaskOfT.CheckAsync((x) => Task.FromResult(CheckFunc(x)))).IsSuccess(out int _).ShouldBeTrue();
        (await _successTaskOfT.CheckAsync((x) => Task.FromResult(CheckFuncOfT(x)))).IsSuccess(out int _).ShouldBeTrue();
    }

    [Fact]
    public async Task Check_OnFailureResult_Should_ReturnFailure()
    {
        _failure.Check(CheckFunc).IsSuccess.ShouldBeFalse();
        _failure.Check(CheckFuncOfT).IsSuccess.ShouldBeFalse();
        _failureOfT.Check(CheckFunc).IsSuccess(out int _).ShouldBeFalse();
        _failureOfT.Check(CheckFuncOfT).IsSuccess(out int _).ShouldBeFalse();
        (await _failureOfT.CheckAsync(Task.FromResult(_success))).IsSuccess(out int _).ShouldBeFalse();
        (await _failureOfT.CheckAsync(_successTaskOfT)).IsSuccess(out int _).ShouldBeFalse();
        (await _failureOfT.CheckAsync((x) => Task.FromResult(CheckFunc(x)))).IsSuccess(out int _).ShouldBeFalse();
        (await _failureOfT.CheckAsync((x) => Task.FromResult(CheckFuncOfT(x)))).IsSuccess(out int _).ShouldBeFalse();
        (await _failureTaskOfT.CheckAsync(_success)).IsSuccess(out int _).ShouldBeFalse();
        (await _failureTaskOfT.CheckAsync(CheckFunc)).IsSuccess(out int _).ShouldBeFalse();
        (await _failureTaskOfT.CheckAsync(CheckFuncOfT)).IsSuccess(out int _).ShouldBeFalse();
        (await _failureTaskOfT.CheckAsync(Task.FromResult(_success))).IsSuccess(out int _).ShouldBeFalse();
        (await _failureTaskOfT.CheckAsync(_successTaskOfT)).IsSuccess(out int _).ShouldBeFalse();
        (await _failureTaskOfT.CheckAsync((x) => Task.FromResult(CheckFunc(x)))).IsSuccess(out int _).ShouldBeFalse();
        (await _failureTaskOfT.CheckAsync((x) => Task.FromResult(CheckFuncOfT(x)))).IsSuccess(out int _).ShouldBeFalse();
    }

    [Fact]
    public async Task Check_OnResultTuple_ShouldWork()
    {
        var successTaskOfTuple = Task.FromResult(Result.Ok((1, false), "tuple"));
        var failureTaskOfTyple = Task.FromResult(Result.Failure<(int, bool)>("failureTuple"));

        static Result CheckFunc(int input, bool condition)
        {
            return condition
                ? Result.Failure("condition failed")
                : Result.Ok($"{input * 3}");
        }

        var output1 = await successTaskOfTuple.CheckAsync(CheckFunc);
        output1.IsSuccess(out (int, bool) value).ShouldBeTrue();
        value.Item2.ShouldBe(false);
        output1.Messages.Count().ShouldBe(2);
        output1.Messages.Last().ShouldBe("3");

        var output2 = await failureTaskOfTyple.CheckAsync(CheckFunc);
        output2.IsSuccess(out (int, bool) _).ShouldBeFalse();
        output2.Messages.Count().ShouldBe(1);
    }
}