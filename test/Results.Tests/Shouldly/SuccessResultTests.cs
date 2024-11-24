namespace DA.Results.Tests.Shouldly;

public class SuccessResultTests : ResultTestBase
{
    [Fact]
    public void ShouldBeSuccesTests()
    {
        ((IResult)SuccessResult).ShouldBeSuccess();
        Should.Throw<ShouldAssertException>(() => ((IResult)null).ShouldBeSuccess());
        Should.Throw<ShouldAssertException>(() => ((IResult)InternalServerErrorResult).ShouldBeSuccess());
    }

    [Fact]
    public void ShouldBeSuccesWithReturnTests()
    {
        var value = IntSuccessResult.ShouldBeSuccess();
        value.ShouldBe(42);
        Should.Throw<ShouldAssertException>(() => ((Result<int>)null).ShouldBeSuccess());
        Should.Throw<ShouldAssertException>(() => InternalServerErrorResult.ShouldBeSuccess());
    }
    
    [Fact]
    public void ShouldBeSuccesWithValueTests()
    {
        IntSuccessResult.ShouldBeSuccessWithValue(42);
        Should.Throw<ShouldAssertException>(() => ((Result<int>)null).ShouldBeSuccessWithValue(42));
        Should.Throw<ShouldAssertException>(() => IntInternalServerErrorResult.ShouldBeSuccessWithValue(42));
    }
    
    [Fact]
    public void ShouldBeSuccesWithPredicateTests()
    {
        IntSuccessResult.ShouldHaveValueThatSatisfies(x => x > 0);
        Should.Throw<ShouldAssertException>(() => ((Result<int>)null).ShouldHaveValueThatSatisfies(x => x > 0));
        Should.Throw<ShouldAssertException>(() => IntInternalServerErrorResult.ShouldHaveValueThatSatisfies(x => x > 0));
    }
}