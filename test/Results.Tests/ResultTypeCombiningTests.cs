using DA.Results.ResultTypes.Failure;
using DA.Results.ResultTypes.Success;

namespace DA.Results.Tests;
public class ResultTypeCombiningTests
{
    private readonly ExceptionResultType _exception;
    private readonly InvalidResultType _invalid;
    private readonly NotFoundResultType<Guid> _notFound;
    private readonly FailureResultType _failure;
    private readonly SuccessResultType _success;

    public ResultTypeCombiningTests()
    {
        _exception = new ExceptionResultType() { Exception = new Exception("new") };
        _invalid = new InvalidResultType("new", "message1");
        _notFound = new NotFoundResultType<Guid>() { Id = Guid.NewGuid() };
        _failure = new FailureResultType();
        _success = new SuccessResultType();
    }


    [Fact]
    public void CombineWithException()
    {
        var input = new ExceptionResultType() { Exception = new Exception("1") };

        input.Combine(_exception).ShouldBe(input);
        input.Combine(_invalid).ShouldBe(input);
        input.Combine(_notFound).ShouldBe(input);
        input.Combine(_failure).ShouldBe(input);
        input.Combine(_success).ShouldBe(input);
    }

    [Fact]
    public void CombineWithInvalid()
    {
        var input = new InvalidResultType("prop", "message");

        input.Combine(_exception).ShouldBe(_exception);
        input.Combine(_notFound).ShouldBe(input);
        input.Combine(_failure).ShouldBe(input);
        input.Combine(_success).ShouldBe(input);

        var combinedWithSelf = input.Combine(_invalid);
        var result = combinedWithSelf as InvalidResultType;
        result.ShouldNotBeNull();
        result.FailedValidations.Count().ShouldBe(2);
        result.FailedValidations.First().Property.ShouldBe("prop");
        result.FailedValidations.Last().Property.ShouldBe("new");
    }

    [Fact]
    public void CombineWithNotFound()
    {
        var input = new NotFoundResultType<int>() { Id = 1 };

        input.Combine(_exception).ShouldBe(_exception);
        input.Combine(_invalid).ShouldBe(_invalid);
        input.Combine(_notFound).ShouldBe(input);
        input.Combine(_failure).ShouldBe(input);
        input.Combine(_success).ShouldBe(input);
    }

    [Fact]
    public void CombineWithFailure()
    {
        var input = new FailureResultType();

        input.Combine(_exception).ShouldBe(_exception);
        input.Combine(_invalid).ShouldBe(_invalid);
        input.Combine(_notFound).ShouldBe(_notFound);
        input.Combine(_failure).ShouldBe(_failure);
        input.Combine(_success).ShouldBe(input);
    }

    [Fact]
    public void CombineWithSuccess()
    {
        var input = new SuccessResultType();

        input.Combine(_exception).ShouldBe(_exception);
        input.Combine(_invalid).ShouldBe(_invalid);
        input.Combine(_notFound).ShouldBe(_notFound);
        input.Combine(_failure).ShouldBe(_failure);
        input.Combine(_success).ShouldBe(input);
    }
}
