using DA.Results.Extensions;

namespace DA.Results.Tests;

public class CheckIfTests : ResultExtensionsTestBase
{
    [Fact]
    public void CheckIf_Should_WorkWithBooleanPredicate()
    {
        _failure.CheckIf(true, Result.Ok()).IsSuccess.ShouldBeFalse();
        _success.CheckIf(false, Result.Ok()).IsSuccess.ShouldBeTrue();
        _success.CheckIf(true, Result.Failure()).IsSuccess.ShouldBeFalse();
        _success.CheckIf(true, Result.Ok()).IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public void CheckIf_Should_WorkWithFuncPredicate()
    {
        _failure.CheckIf(() => true, Result.Ok).IsSuccess.ShouldBeFalse();
        _success.CheckIf(() => false, Result.Ok).IsSuccess.ShouldBeTrue();
        _success.CheckIf(() => true, Result.Failure).IsSuccess.ShouldBeFalse();
        _success.CheckIf(() => true, Result.Ok).IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public void CheckIf_Should_WorkWithBooleanPredicateOfT()
    {
        _failureOfT.CheckIf(true, Result.Ok()).IsSuccess(out var _).ShouldBeFalse();
        _successOfT.CheckIf(false, Result.Ok()).IsSuccess(out var _).ShouldBeTrue();
        _successOfT.CheckIf(true, Result.Failure()).IsSuccess(out var _).ShouldBeFalse();
        _successOfT.CheckIf(true, Result.Ok()).IsSuccess(out var _).ShouldBeTrue();
    }

    [Fact]
    public void CheckIf_Should_WorkWithFuncPredicateOfT()
    {
        _failureOfT.CheckIf((x) => x >= 1, (x) => Result.Ok()).IsSuccess(out var _).ShouldBeFalse();
        _successOfT.CheckIf((x) => x < 1, (x) => Result.Ok()).IsSuccess(out var _).ShouldBeTrue();
        _successOfT.CheckIf((x) => x >= 1, (x) => Result.Failure()).IsSuccess(out var _).ShouldBeFalse();
        _successOfT.CheckIf((x) => x >= 1, (x) => Result.Ok()).IsSuccess(out var _).ShouldBeTrue();
    }
}