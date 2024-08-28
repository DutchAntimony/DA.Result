using DA.Options;
using DA.Results.ResultTypes.Failure;
using DA.Results.ResultTypes.Success;

namespace DA.Results.Tests;
public class ResultFactoryMethodsTests
{
    #region --- Ok ---
    [Fact]
    public void Ok_Should_CreateCorrectResult_WithoutMessages()
    {
        var okResult = Result.Ok();
        okResult.ResultType.ShouldBeOfType<SuccessResultType>();
        okResult.IsSuccess.ShouldBeTrue();
        okResult.Messages.ShouldBeEmpty();
    }

    [Fact]
    public void Ok_Should_CreateCorrectResult_WithMessages()
    {
        var okResult = Result.Ok("test");
        okResult.ResultType.ShouldBeOfType<SuccessResultType>();
        okResult.IsSuccess.ShouldBeTrue();
        okResult.Messages.Count().ShouldBe(1);
    }

    [Fact]
    public void Ok_Should_CreateCorrectResult_WithValueWithoutMessages()
    {
        var okResult = Result.Ok(1);
        okResult.ResultType.ShouldBeOfType<SuccessResultType>();
        okResult.IsSuccess(out var value).ShouldBeTrue();
        value.ShouldBe(1);
        okResult.Messages.ShouldBeEmpty();
    }

    [Fact]
    public void Ok_Should_CreateCorrectResult_WithValueWithMessages()
    {
        var okResult = Result.Ok(1, "test");
        okResult.ResultType.ShouldBeOfType<SuccessResultType>();
        okResult.IsSuccess(out var value).ShouldBeTrue();
        value.ShouldBe(1);
        okResult.Messages.Count().ShouldBe(1);
    }
    #endregion 

    #region --- OkIf ---
    [Fact]
    public void OkIf_Should_CreateCorrectResult_boolInput()
    {
        Result.OkIf(true, "failure").ResultType.ShouldBeOfType<SuccessResultType>();
        Result.OkIf(false, "failure").ResultType.ShouldBeOfType<FailureResultType>();
    }

    [Fact]
    public void OkIf_Should_CreateCorrectResult_funcInput()
    {
        Result.OkIf(() => true, "failure").ResultType.ShouldBeOfType<SuccessResultType>();
        Result.OkIf(() => false, "failure").ResultType.ShouldBeOfType<FailureResultType>();
    }

    [Fact]
    public async Task OkIf_Should_CreateCorrectResult_taskInput()
    {
        (await Result.OkIfAsync(Task.FromResult(true), "failure")).ResultType.ShouldBeOfType<SuccessResultType>();
        (await Result.OkIfAsync(Task.FromResult(false), "failure")).ResultType.ShouldBeOfType<FailureResultType>();
    }

    [Fact]
    public async Task OkIf_Should_CreateCorrectResult_taskFuncInput()
    {
        (await Result.OkIfAsync(() => Task.FromResult(true), "failure")).ResultType.ShouldBeOfType<SuccessResultType>();
        (await Result.OkIfAsync(() => Task.FromResult(false), "failure")).ResultType.ShouldBeOfType<FailureResultType>();
    }

    [Fact]
    public void OkIf_Should_CreateCorrectResultOfT_boolInput()
    {
        Result.OkIf(5, true, "failure").ResultType.ShouldBeOfType<SuccessResultType>();
        Result.OkIf(5, false, "failure").ResultType.ShouldBeOfType<FailureResultType>();
    }

    [Fact]
    public void OkIf_Should_CreateCorrectResultOfT_funcInput()
    {
        Result.OkIf(5, (x) => x > 3, "failure").ResultType.ShouldBeOfType<SuccessResultType>();
        Result.OkIf(5, (x) => x < 3, "failure").ResultType.ShouldBeOfType<FailureResultType>();
    }

    [Fact]
    public async Task OkIf_Should_CreateCorrectResultOfT_taskInput()
    {
        (await Result.OkIfAsync(5, Task.FromResult(true), "failure")).ResultType.ShouldBeOfType<SuccessResultType>();
        (await Result.OkIfAsync(5, Task.FromResult(false), "failure")).ResultType.ShouldBeOfType<FailureResultType>();
    }

    [Fact]
    public async Task OkIf_Should_CreateCorrectResultOfT_taskFuncInput()
    {
        (await Result.OkIfAsync(5, (x) => Task.FromResult(x > 3), "failure")).ResultType.ShouldBeOfType<SuccessResultType>();
        (await Result.OkIfAsync(5, (x) => Task.FromResult(x < 3), "failure")).ResultType.ShouldBeOfType<FailureResultType>();
    }

    #endregion

    #region --- OkIfFound ---
    [Fact]
    public void OkIfFound_Should_CreateCorrectResult()
    {
        Result.OkIfFound(Option.From(5), Guid.NewGuid()).ResultType.ShouldBeOfType<SuccessResultType>();
        Result.OkIfFound<int, Guid>(Option.None, Guid.NewGuid()).ResultType.ShouldBeOfType<NotFoundResultType<Guid>>();
    }
    #endregion

    #region --- Failure ---
    [Fact]
    public void Failure_Should_CreateCorrectResult_WithoutMessages()
    {
        var result = Result.Failure();
        result.ResultType.ShouldBeOfType<FailureResultType>();
        result.IsSuccess.ShouldBeFalse();
        result.Messages.ShouldBeEmpty();
    }

    [Fact]
    public void Failure_Should_CreateCorrectResult_WithMessages()
    {
        var result = Result.Failure("test");
        result.ResultType.ShouldBeOfType<FailureResultType>();
        result.IsSuccess.ShouldBeFalse();
        result.Messages.Count().ShouldBe(1);
    }

    [Fact]
    public void Failure_Should_CreateCorrectResultOfT_WithoutMessages()
    {
        var result = Result.Failure<int>();
        result.ResultType.ShouldBeOfType<FailureResultType>();
        result.IsSuccess(out var _).ShouldBeFalse();
        result.Messages.Count().ShouldBe(0);
    }

    [Fact]
    public void Failure_Should_CreateCorrectResultOfT_WithMessages()
    {
        var result = Result.Failure<int>("test");
        result.ResultType.ShouldBeOfType<FailureResultType>();
        result.IsSuccess(out var _).ShouldBeFalse();
        result.Messages.Count().ShouldBe(1);
    }

    [Fact]
    public void Failure_Should_CreateCorrectResultOfT_WithValueWithoutMessages()
    {
        var result = Result.Failure(1);
        result.ResultType.ShouldBeOfType<FailureResultType>();
        result.IsSuccess(out var _).ShouldBeFalse();
        result.Messages.Count().ShouldBe(0);
    }

    [Fact]
    public void Failure_Should_CreateCorrectResultOfT_WithValueWithMessages()
    {
        var result = Result.Failure(1, "test");
        result.ResultType.ShouldBeOfType<FailureResultType>();
        result.IsSuccess(out var _).ShouldBeFalse();
        result.Messages.Count().ShouldBe(1);
    }
    #endregion

    #region --- Invalid ---
    [Fact]
    public void Invalid_Should_CreateCorrectResult_FromPlainValues()
    {
        var result = Result.Invalid("prop", "message");
        result.ResultType.ShouldBeOfType<InvalidResultType>();
        result.IsSuccess.ShouldBeFalse();
        result.Messages.ShouldBeEmpty();

        var invalidResultType = result.ResultType as InvalidResultType;
        invalidResultType.ShouldNotBeNull();
        invalidResultType.FailedValidations.Count().ShouldBe(1);
        invalidResultType.FailedValidations.First().Property.ShouldBe("prop");
    }

    [Fact]
    public void Invalid_Should_CreateCorrectResult_FromDictionary()
    {
        var dict = new Dictionary<string, string>() { { "prop", "message" } };
        var result = Result.Invalid(dict);
        result.ResultType.ShouldBeOfType<InvalidResultType>();
        result.IsSuccess.ShouldBeFalse();
        result.Messages.ShouldBeEmpty();

        var invalidResultType = result.ResultType as InvalidResultType;
        invalidResultType.ShouldNotBeNull();
        invalidResultType.FailedValidations.Count().ShouldBe(1);
        invalidResultType.FailedValidations.First().Property.ShouldBe("prop");
    }

    [Fact]
    public void Invalid_Should_CreateCorrectResultOfT_FromPlainValues()
    {
        var result = Result.Invalid<int>("prop", "message");
        result.ResultType.ShouldBeOfType<InvalidResultType>();
        result.IsSuccess(out var _).ShouldBeFalse();
        result.Messages.ShouldBeEmpty();

        var invalidResultType = result.ResultType as InvalidResultType;
        invalidResultType.ShouldNotBeNull();
        invalidResultType.FailedValidations.Count().ShouldBe(1);
        invalidResultType.FailedValidations.First().Property.ShouldBe("prop");
    }

    [Fact]
    public void Invalid_Should_CreateCorrectResultOfT_FromDictionary()
    {
        var dict = new Dictionary<string, string>() { { "prop", "message" } };
        var result = Result.Invalid<int>(dict);
        result.ResultType.ShouldBeOfType<InvalidResultType>();
        result.IsSuccess(out var _).ShouldBeFalse();
        result.Messages.ShouldBeEmpty();

        var invalidResultType = result.ResultType as InvalidResultType;
        invalidResultType.ShouldNotBeNull();
        invalidResultType.FailedValidations.Count().ShouldBe(1);
        invalidResultType.FailedValidations.First().Property.ShouldBe("prop");
    }

    [Fact]
    public void Invalid_Should_CreateCorrectResultOfT_FromPlainValuesWithValue()
    {
        var result = Result.Invalid(1, "prop", "message");
        result.ResultType.ShouldBeOfType<InvalidResultType>();
        result.IsSuccess(out var _).ShouldBeFalse();
        result.Messages.ShouldBeEmpty();

        var invalidResultType = result.ResultType as InvalidResultType;
        invalidResultType.ShouldNotBeNull();
        invalidResultType.FailedValidations.Count().ShouldBe(1);
        invalidResultType.FailedValidations.First().Property.ShouldBe("prop");
    }

    [Fact]
    public void Invalid_Should_CreateCorrectResultOfT_FromDictionaryWith()
    {
        var dict = new Dictionary<string, string>() { { "prop", "message" } };
        var result = Result.Invalid(1, dict);
        result.ResultType.ShouldBeOfType<InvalidResultType>();
        result.IsSuccess(out var _).ShouldBeFalse();
        result.Messages.ShouldBeEmpty();

        var invalidResultType = result.ResultType as InvalidResultType;
        invalidResultType.ShouldNotBeNull();
        invalidResultType.FailedValidations.Count().ShouldBe(1);
        invalidResultType.FailedValidations.First().Property.ShouldBe("prop");
    }
    #endregion

    #region --- InvalidIf ---
    [Fact]
    public void InvalidIf_Should_CreateCorrectResult_boolInput()
    {
        Result.InvalidIf(false, "prop", "message").ResultType.ShouldBeOfType<SuccessResultType>();
        Result.InvalidIf(true, "prop", "message").ResultType.ShouldBeOfType<InvalidResultType>();
    }

    [Fact]
    public void InvalidIf_Should_CreateCorrectResult_funcInput()
    {
        Result.InvalidIf(() => false, "prop", "message").ResultType.ShouldBeOfType<SuccessResultType>();
        Result.InvalidIf(() => true, "prop", "message").ResultType.ShouldBeOfType<InvalidResultType>();
    }

    [Fact]
    public async Task InvalidIf_Should_CreateCorrectResult_taskInput()
    {
        (await Result.InvalidIfAsync(Task.FromResult(false), "prop", "message")).ResultType.ShouldBeOfType<SuccessResultType>();
        (await Result.InvalidIfAsync(Task.FromResult(true), "prop", "message")).ResultType.ShouldBeOfType<InvalidResultType>();
    }

    [Fact]
    public async Task InvalidIf_Should_CreateCorrectResult_taskFuncInput()
    {
        (await Result.InvalidIfAsync(() => Task.FromResult(false), "prop", "message")).ResultType.ShouldBeOfType<SuccessResultType>();
        (await Result.InvalidIfAsync(() => Task.FromResult(true), "prop", "message")).ResultType.ShouldBeOfType<InvalidResultType>();
    }

    [Fact]
    public void InvalidIf_Should_CreateCorrectResultOfT_boolInput()
    {
        Result.InvalidIf(5, false, "prop", "message").ResultType.ShouldBeOfType<SuccessResultType>();
        Result.InvalidIf(5, true, "prop", "message").ResultType.ShouldBeOfType<InvalidResultType>();
    }

    [Fact]
    public void InvalidIf_Should_CreateCorrectResultOfT_funcInput()
    {
        Result.InvalidIf(5, (x) => x < 3, "prop", "message").ResultType.ShouldBeOfType<SuccessResultType>();
        Result.InvalidIf(5, (x) => x > 3, "prop", "message").ResultType.ShouldBeOfType<InvalidResultType>();
    }

    [Fact]
    public async Task InvalidIf_Should_CreateCorrectResultOfT_taskInput()
    {
        (await Result.InvalidIfAsync(5, Task.FromResult(false), "prop", "message")).ResultType.ShouldBeOfType<SuccessResultType>();
        (await Result.InvalidIfAsync(5, Task.FromResult(true), "prop", "message")).ResultType.ShouldBeOfType<InvalidResultType>();
    }

    [Fact]
    public async Task InvalidIf_Should_CreateCorrectResultOfT_taskFuncInput()
    {
        (await Result.InvalidIfAsync(5, (x) => Task.FromResult(x < 3), "prop", "message")).ResultType.ShouldBeOfType<SuccessResultType>();
        (await Result.InvalidIfAsync(5, (x) => Task.FromResult(x > 3), "prop", "message")).ResultType.ShouldBeOfType<InvalidResultType>();
    }

    #endregion

    #region --- Exception --- 
    [Fact]
    public void Exception_Should_CreateCorrectResult()
    {
        var ex = new Exception("test");
        var result = Result.Exception(ex);
        result.ResultType.ShouldBeOfType<ExceptionResultType>();
        result.IsSuccess.ShouldBeFalse();
        result.Messages.Count().ShouldBe(1);
        result.Messages.First().ShouldBe("test");
    }

    [Fact]
    public void Exception_Should_CreateCorrectResultOfT()
    {
        var ex = new Exception("test");
        var result = Result.Exception<int>(ex);
        result.ResultType.ShouldBeOfType<ExceptionResultType>();
        result.IsSuccess(out var _).ShouldBeFalse();
        result.Messages.Count().ShouldBe(1);
        result.Messages.First().ShouldBe("test");
    }

    [Fact]
    public void Exception_Should_CreateCorrectResultOfT_WithValue()
    {
        var ex = new Exception("test");
        var result = Result.Exception(1, ex);
        result.ResultType.ShouldBeOfType<ExceptionResultType>();
        result.IsSuccess(out var _).ShouldBeFalse();
        result.Messages.Count().ShouldBe(1);
        result.Messages.First().ShouldBe("test");
    }
    #endregion

    #region --- NotFound ---

    [Fact]
    public void NotFound_Should_CreateCorrectResultOfT_WithGuidAndNoMessage()
    {
        var result = Result.NotFound<int>(Guid.Empty);
        result.ResultType.ShouldBeOfType<NotFoundResultType<Guid>>();
        result.IsSuccess(out var _).ShouldBeFalse();
        result.Messages.Count().ShouldBe(1);
        result.Messages.First().ShouldBe($"Did not find Int32 with Id {Guid.Empty}");

        var notFoundResultType = result.ResultType as NotFoundResultType<Guid>;
        notFoundResultType.ShouldNotBeNull();
        notFoundResultType.Id.ShouldBe(Guid.Empty);
    }

    [Fact]
    public void NotFound_Should_CreateCorrectResultOfT_WithGuidAndMessage()
    {
        var result = Result.NotFound<int>(Guid.Empty, "test");
        result.ResultType.ShouldBeOfType<NotFoundResultType<Guid>>();
        result.IsSuccess(out var _).ShouldBeFalse();
        result.Messages.Count().ShouldBe(1);
        result.Messages.First().ShouldBe($"test");

        var notFoundResultType = result.ResultType as NotFoundResultType<Guid>;
        notFoundResultType.ShouldNotBeNull();
        notFoundResultType.Id.ShouldBe(Guid.Empty);
    }

    [Fact]
    public void NotFound_Should_CreateCorrectResultOfT_WithCustomTypeAndNoMessage()
    {
        var result = Result.NotFound<int, int>(1);
        result.ResultType.ShouldBeOfType<NotFoundResultType<int>>();
        result.IsSuccess(out var _).ShouldBeFalse();
        result.Messages.Count().ShouldBe(1);
        result.Messages.First().ShouldBe($"Did not find Int32 with Id 1");

        var notFoundResultType = result.ResultType as NotFoundResultType<int>;
        notFoundResultType.ShouldNotBeNull();
        notFoundResultType.Id.ShouldBe(1);
    }

    [Fact]
    public void NotFound_Should_CreateCorrectResultOfT_WithCustomTypeAndMessage()
    {
        var result = Result.NotFound<int, int>(1, "test");
        result.ResultType.ShouldBeOfType<NotFoundResultType<int>>();
        result.IsSuccess(out var _).ShouldBeFalse();
        result.Messages.Count().ShouldBe(1);
        result.Messages.First().ShouldBe($"test");

        var notFoundResultType = result.ResultType as NotFoundResultType<int>;
        notFoundResultType.ShouldNotBeNull();
        notFoundResultType.Id.ShouldBe(1);
    }
    #endregion
}
