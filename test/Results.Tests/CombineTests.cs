using DA.Results.Extensions;

namespace DA.Results.Tests;

public class CombineTests : ResultExtensionsTestBase
{
    [Fact]
    public async Task Combine_Should_ConvertToCombinedType_WhenFirstResultIsFailure()
    {
        _failureOfT.Combine(2d).IsSuccess(out (int, double) t0).ShouldBeFalse();
        _failureOfT.Combine(Result.Ok(2d)).IsSuccess(out (int, double) t1).ShouldBeFalse();
        _failureOfT.Combine(r1 => Result.Ok(r1 * 1.5)).IsSuccess(out (int, double) t2).ShouldBeFalse();
        (await _failureOfT.CombineAsync(r1 => Task.FromResult(Result.Ok(r1 * 1.5)))).IsSuccess(out (int, double) t3).ShouldBeFalse();
        (await _failureTaskOfT.CombineAsync(Result.Ok(2d))).IsSuccess(out (int, double) t4).ShouldBeFalse();
        (await _failureTaskOfT.CombineAsync(r1 => Result.Ok(r1 * 1.5))).IsSuccess(out (int, double) t5).ShouldBeFalse();
        (await _failureTaskOfT.CombineAsync(Task.FromResult(Result.Ok(2d)))).IsSuccess(out (int, double) t6).ShouldBeFalse();
        (await _failureTaskOfT.CombineAsync(r1 => Task.FromResult(Result.Ok(r1 * 1.5)))).IsSuccess(out (int, double) t7).ShouldBeFalse();
    }

    [Fact]
    public async Task Combine_Should_ConvertToCombinedType_WhenSecondResultIsFailure()
    {
        _successOfT.Combine(Result.Failure<double>()).IsSuccess(out (int, double) t1).ShouldBeFalse();
        _successOfT.Combine(r1 => Result.Failure<double>()).IsSuccess(out (int, double) t2).ShouldBeFalse();
        (await _successOfT.CombineAsync(r1 => Task.FromResult(Result.Failure(r1 * 1.5)))).IsSuccess(out (int, double) t3).ShouldBeFalse();
        (await _successTaskOfT.CombineAsync(Result.Failure(2d))).IsSuccess(out (int, double) t4).ShouldBeFalse();
        (await _successTaskOfT.CombineAsync(r1 => Result.Failure(r1 * 1.5))).IsSuccess(out (int, double) t5).ShouldBeFalse();
        (await _successTaskOfT.CombineAsync(Task.FromResult(Result.Failure(2d)))).IsSuccess(out (int, double) t6).ShouldBeFalse();
        (await _successTaskOfT.CombineAsync(r1 => Task.FromResult(Result.Failure(r1 * 1.5)))).IsSuccess(out (int, double) t7).ShouldBeFalse();
    }

    [Fact]
    public async Task Combine_Should_ConvertToCombinedType_WhenBothResultsAreSuccess()
    {
        _successOfT.Combine(2d).IsSuccess(out (int, double) t0).ShouldBeTrue();
        t0.ShouldBe((1, 2d));
        _successOfT.Combine(Result.Ok(2d)).IsSuccess(out (int, double) t1).ShouldBeTrue();
        t1.ShouldBe((1, 2d));
        _successOfT.Combine(r1 => Result.Ok(2d)).IsSuccess(out (int, double) t2).ShouldBeTrue();
        t2.ShouldBe((1, 2d));
        (await _successOfT.CombineAsync(r1 => Task.FromResult(Result.Ok(r1 * 1.5)))).IsSuccess(out (int, double) t3).ShouldBeTrue();
        t3.ShouldBe((1, 1.5));
        (await _successTaskOfT.CombineAsync(Result.Ok(2d))).IsSuccess(out (int, double) t4).ShouldBeTrue();
        t4.ShouldBe((1, 2d));
        (await _successTaskOfT.CombineAsync(r1 => Result.Ok(r1 * 1.5))).IsSuccess(out (int, double) t5).ShouldBeTrue();
        t5.ShouldBe((1, 1.5));
        (await _successTaskOfT.CombineAsync(Task.FromResult(Result.Ok(2d)))).IsSuccess(out (int, double) t6).ShouldBeTrue();
        t6.ShouldBe((1, 2d));
        (await _successTaskOfT.CombineAsync(r1 => Task.FromResult(Result.Ok(r1 * 1.5)))).IsSuccess(out (int, double) t7).ShouldBeTrue();
        t7.ShouldBe((1, 1.5d));
    }
}