namespace DA.Results.ResultTypes.Failure;

public interface IFailureResultType : IResultType
{
    /// <summary>
    /// Every failure result has a weight. 
    /// Depending on the weight, when binding this result to a different failure result will change the type of the resulting result.
    /// The heigher the weight of the result, the more important.
    /// </summary>
    int Weight { get; }
}
