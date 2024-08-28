using DA.Results.Extensions;

namespace DA.Results.Tests;

public class FlattenTests : ResultExtensionsTestBase
{
    [Fact]
    public void Flatten_Should_TurnResultT_ToResult()
    {
        var success = _successOfT.Flatten();
        success.IsSuccess.ShouldBeTrue();
        success.Messages.ShouldBe(_successOfT.Messages);

        var failure = _failureOfT.Flatten();
        failure.IsSuccess.ShouldBeFalse();
        failure.Messages.ShouldBe(_failureOfT.Messages);
    }

    [Fact]
    public async Task FlattenAsync_Should_TurnTaskOfResultOfT_ToTaskOfResult()
    {
        var successTask = _successTaskOfT.FlattenAsync();
        var success = await successTask;
        success.IsSuccess.ShouldBeTrue();
        success.Messages.ShouldBe(_successOfT.Messages);

        var failureTask = _failureTaskOfT.FlattenAsync();
        var failure = await failureTask;
        failure.IsSuccess.ShouldBeFalse();
        failure.Messages.ShouldBe(_failureOfT.Messages);
    }
}