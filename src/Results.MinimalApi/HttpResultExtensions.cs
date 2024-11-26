using DA.Results.Issues;
using Microsoft.AspNetCore.Mvc;

namespace DA.Results.MinimalApi;

public static class HttpResultExtensions
{
    public static Microsoft.AspNetCore.Http.IResult ToHttpResult<TValue>(this Result<TValue> result)
    {
        return !result.TryGetValue(out var value, out var issue)
            ? Microsoft.AspNetCore.Http.Results.Problem(issue.ToProblemDetails())
            : result is NoContentResult
                ? Microsoft.AspNetCore.Http.Results.NoContent()
                : Microsoft.AspNetCore.Http.Results.Ok(value);
    }

    private static ProblemDetails ToProblemDetails(this Issue issue)
    {
        return new ProblemDetails()
        {
            Title = issue.Title,
            Detail = issue.GetMessage(),
            Status = issue.StatusCode,
        };
    }
}