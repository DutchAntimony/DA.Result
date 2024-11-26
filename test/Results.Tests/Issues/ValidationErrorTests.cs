namespace DA.Results.Tests.Issues;

public class ValidationErrorTests : ResultTestBase
{
    private readonly List<ValidationFailure> _failures =
    [
        new ValidationFailure("Prop1", "Message1"),
        new ValidationFailure("Prop1", "Message2"),
        new ValidationFailure("Prop2", "Message1")
    ];

    [Fact]
    public void ValidationError_ShouldBe_ConstructableFrom_PropertyAndMessage()
    {
        const string property = "Property";
        const string message = "Message";

        NoContentResult result = new ValidationError(property, message);
        result.ShouldHaveIssue<ValidationError>(out var issue);
        issue.GetMessage().ShouldContain($"{property}: {message}");
    }

    [Fact]
    public void ValidationError_ShouldBe_ConstructableFrom_ValidationFailureList()
    {
        NoContentResult result = new ValidationError(_failures);
        result.ShouldHaveIssue<ValidationError>();
    }

    [Fact]
    public void GetFailuresGroupedByProperty_Should_ReturnProperDictionary()
    {
        var validationError = new ValidationError(_failures);
        var dictionary = validationError.GetFailuresGroupedByProperty();
        dictionary.Count.ShouldBe(2);

        // check failures for prop1:
        dictionary.ShouldContainKey("Prop1");
        var prop1Errors = dictionary["Prop1"].ToList();
        prop1Errors.Count.ShouldBe(2);
        prop1Errors.ShouldContain("Message1");
        prop1Errors.ShouldContain("Message2");

        // check failures for prop2:
        dictionary.ShouldContainKey("Prop2");
        var prop2Errors = dictionary["Prop2"].ToList();
        prop2Errors.Count.ShouldBe(1);
        prop2Errors.ShouldContain("Message1");
    }

    [Fact]
    public void GetFailuresOfProperty_Should_ReturnEmptyCollection_WhenPropertyIsNotFound()
    {
        var validationError = new ValidationError(_failures);
        var failuresForProp3 = validationError.GetFailuresOfProperty("Prop3");
        failuresForProp3.ShouldBeEmpty();
    }

    [Fact]
    public void GetFailuresOfProperty_Should_ReturnCollection_WhenPropertyIsFound()
    {
        var validationError = new ValidationError(_failures);
        var failuresForProp1 = validationError.GetFailuresOfProperty("Prop1").ToList();
        failuresForProp1.Count.ShouldBe(2);
        failuresForProp1.ShouldContain("Message1");
    }
}