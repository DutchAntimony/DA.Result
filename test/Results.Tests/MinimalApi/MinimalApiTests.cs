using DA.Results.MinimalApi;

namespace DA.Results.Tests.MinimalApi;

public class MinimalApiTests : ResultTestBase
{
    [Fact]
    public void ToResult_Should_ReturnResultsNoContent_When_ResultIsSuccessWithoutValue()
    {
        var httpResult = SuccessResult.ToHttpResult();
        httpResult.ShouldBeOfType<Microsoft.AspNetCore.Http.HttpResults.NoContent>();
    }
    
    [Fact]
    public void ToResult_Should_ReturnResultsOk_When_ResultIsSuccessWithValue()
    {
        var httpResult = IntSuccessResult.ToHttpResult();
        httpResult.ShouldBeOfType<Microsoft.AspNetCore.Http.HttpResults.Ok<int>>();
        var actual = httpResult as Microsoft.AspNetCore.Http.HttpResults.Ok<int>;
        actual.ShouldNotBeNull();
        actual.Value.ShouldBe(42);
    }

    [Fact]
    public void ToResult_Should_ReturnResultsProblem_When_ResultIsFailure()
    {
        var httpResult = InternalServerErrorResult.ToHttpResult();
        httpResult.ShouldBeOfType<Microsoft.AspNetCore.Http.HttpResults.ProblemHttpResult>();
        var actual = httpResult as Microsoft.AspNetCore.Http.HttpResults.ProblemHttpResult;
        actual.ShouldNotBeNull();
        actual.StatusCode.ShouldBe(500);
        actual.ProblemDetails.Title.ShouldBe("One or more errors occurred:");
        actual.ProblemDetails.Detail.ShouldBe("Test error");
    }
}