namespace DA.Results.Tests;

public class ResultConstructionTests : ResultTestBase
{
    [Fact]
    public void Fail_Should_CreateInvalidOperationError_WithCorrectMessage()
    {
        Result.Fail("The message").ShouldHaveIssueAndMessage<InvalidOperationError>("The message");
        Result.Fail<string>("The message").ShouldHaveIssueAndMessage<InvalidOperationError>("The message");
    }

    [Fact]
    public void Warn_Should_CreateSuccess_WithCorrectValue_WhenIgnoreWarningSetToTrue()
    {
        Result.Warn(new UnmodifiedWarning(typeof(string)), true)
            .ShouldBeSuccess();
        
        Result.Warn(new UnmodifiedWarning(typeof(string)), "Value", true)
            .ShouldBeSuccessWithValue("Value");
    }

    [Fact]
    public void Warn_Should_CreateFailure_WithCorrectMessage_WhenIgnoreWarningSetToFalse()
    {
        Result.Warn(new UnmodifiedWarning(typeof(string)), false)
            .ShouldHaveIssueAndMessage<UnmodifiedWarning>(nameof(String));
        
        Result.Warn(new UnmodifiedWarning(typeof(string)), "Value", false)
            .ShouldHaveIssueAndMessage<UnmodifiedWarning>(nameof(String));
    }

    [Fact]
    public void Ok_Should_CreateSuccess_WithNoContentValueType()
    {
        Result.Ok().GetValueType().ShouldBe(typeof(NoContent));
    }

    [Fact]
    public void Ok_Should_CreateSuccess_WithCorrectValueType()
    {
        Result.Ok(true,true).GetValueType().ShouldBe(typeof(bool));
    }

    [Fact]
    public void ResultWithoutContentTests()
    {
        var test1 = (NoContentResult)Result.Ok(3);
        test1.ShouldBeOfType<NoContentResult>();
        test1.IsSuccess.ShouldBeTrue();

        var test2 = Result.Ok("abc").WithoutContent();
        test2.ShouldBeOfType<NoContentResult>();
        test2.IsSuccess.ShouldBeTrue();
        
        var test3 = Result.Fail<string>("failure").WithoutContent();
        test3.ShouldBeOfType<NoContentResult>();
        test3.ShouldHaveIssueAndMessage<InvalidOperationError>("failure");
    }
}