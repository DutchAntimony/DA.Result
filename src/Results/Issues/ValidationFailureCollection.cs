namespace DA.Results.Issues;

public sealed class ValidationFailureCollection
{
    private readonly List<ValidationFailure> _failures = [];
    private Issue? _existingNonValidationFailure;

    public ValidationFailureCollection() {}

    public ValidationFailureCollection(List<ValidationFailure> failures)
    {
        _failures = failures;
    }
    
    public ValidationFailureCollection AddFrom(IResult result)
    {
        if (!result.TryGetIssue(out var issue)) 
            return this;

        if (issue is ValidationError validationError)
            _failures.AddRange(validationError.Failures);
        else
            _existingNonValidationFailure = issue;

        return this;
    }

    public Result<Result.NoContent> ToResult(bool ignoreWarnings = false)
    {
        if (_existingNonValidationFailure is not null)
            return _existingNonValidationFailure;

        return _failures.Count != 0
            ? new ValidationError(_failures)
            : Result.Ok(ignoreWarnings);
    }

    public Result<TValue> ToResult<TValue>(TValue value, bool ignoreWarnings = false)
    {
        if (_existingNonValidationFailure is not null)
            return Result.Fail<TValue>(_existingNonValidationFailure);

        return _failures.Count != 0 
            ? new ValidationError(_failures) 
            : Result.Ok(value, ignoreWarnings);
    }
}