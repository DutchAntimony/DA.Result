namespace DA.Results.Tests.Shouldly;

public class SuccessResultTests : ResultTestBase
{
    private readonly IResult? _nullResult = null;
    private readonly Result<int>? _intNullResult = null;
    
    [Fact]
    public void ShouldBeSuccesTests()
    {
        ((IResult)SuccessResult).ShouldBeSuccess();
        Should.Throw<ShouldAssertException>(() => _nullResult.ShouldBeSuccess());
        Should.Throw<ShouldAssertException>(() => ((IResult)InternalServerErrorResult).ShouldBeSuccess());
    }

    [Fact]
    public void ShouldBeSuccesWithReturnTests()
    {
        var value = IntSuccessResult.ShouldBeSuccess();
        value.ShouldBe(42);
        Should.Throw<ShouldAssertException>(() => _intNullResult.ShouldBeSuccess());
        Should.Throw<ShouldAssertException>(() => InternalServerErrorResult.ShouldBeSuccess());
    }
    
    [Fact]
    public void ShouldBeSuccesWithValueTests()
    {
        IntSuccessResult.ShouldBeSuccessWithValue(42);
        Should.Throw<ShouldAssertException>(() => _intNullResult.ShouldBeSuccessWithValue(42));
        Should.Throw<ShouldAssertException>(() => IntInternalServerErrorResult.ShouldBeSuccessWithValue(42));
    }
    
    [Fact]
    public void ShouldBeSuccesWithPredicateTests()
    {
        IntSuccessResult.ShouldHaveValueThatSatisfies(x => x > 0);
        Should.Throw<ShouldAssertException>(() => _intNullResult.ShouldHaveValueThatSatisfies(x => x > 0));
        Should.Throw<ShouldAssertException>(() => IntInternalServerErrorResult.ShouldHaveValueThatSatisfies(x => x > 0));
    }
}