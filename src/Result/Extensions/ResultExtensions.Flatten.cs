namespace DA.Results.Extensions;

public static partial class ResultExtensions
{
    public static async Task<Result> FlattenAsync<T>(this Task<Result<T>> resultTask)
    {
        var result = await resultTask;
        return result.Flatten();
    }
}