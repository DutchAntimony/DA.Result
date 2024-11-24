namespace DA.Results.Tests.Issues;

public class IssueTests : ResultTestBase
{
    [Fact]
    public void ConfirmationRequiredWarning_ShouldBe_ConfiguredCorrectly()
    {
        WarningResult.ShouldHaveIssue<ConfirmationRequiredWarning>(out var issue);
        issue.Title.ShouldBe("Actie moet bevestigd worden");
        issue.StatusCode.ShouldBe(400);
        issue.GetMessage().ShouldContain("Test warning");
    }

    [Fact]
    public void InternalServerError_Should_BeCreatedImplicitlyFromException()
    {
        InternalServerError ise = new Exception("Message");
        Result<Result.NoContent> result = ise;
        result.ShouldHaveIssue<InternalServerError>(out var issue);
        issue.Title.ShouldBe("One or more errors occurred:");
        issue.StatusCode.ShouldBe(500);
        issue.Exceptions.ShouldNotBeEmpty();
        issue.Exceptions.Count().ShouldBe(1);
        issue.GetMessage().ShouldContain("Message");
    }

    [Fact]
    public void InvalidOperationError_ShouldBe_ConfiguredCorrectly()
    {
        Result<Result.NoContent> result = new InvalidOperationError("Message");
        result.ShouldHaveIssue<InvalidOperationError>(out var issue);
        issue.Title.ShouldBe("Something was wrong with the request:");
        issue.StatusCode.ShouldBe(400);
        issue.GetMessage().ShouldContain("Message");
    }

    [Fact]
    public void UnmodifiedWarning_ShouldBe_ConstructedUsingAProperty()
    {
        var type = typeof(ResultTestBase);
        Result<Result.NoContent> result = new UnmodifiedWarning(type);
        result.ShouldHaveIssue<UnmodifiedWarning>(out var issue);
        issue.Types.ShouldNotBeEmpty();
        issue.Types.Count().ShouldBe(1);
        issue.Types.ShouldContain(type);
        issue.Title.ShouldBe("Actie heeft geen wijzigingen opgeleverd");
        issue.StatusCode.ShouldBe(400);
        issue.GetMessage().ShouldContain(type.Name);
    }

    [Fact]
    public void Error_CanBeConstructed_FromOtherError()
    {
        Error error = new InvalidOperationError("Message");
        var newError = error with { };
        newError.ShouldNotBeNull();
    }
    
    [Fact]
    public void Issue_CanBeConstructed_FromOtherIssue()
    {
        Issue error = new InvalidOperationError("Message");
        var newError = error with { };
        newError.ShouldNotBeNull();
    }
    
    [Fact]
    public void Warning_CanBeConstructed_FromOtherError()
    {
        Warning warning = new ConfirmationRequiredWarning("Message");
        var newWarning = warning with { };
        newWarning.ShouldNotBeNull();
    }
}