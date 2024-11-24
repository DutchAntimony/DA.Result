# DA.Result

## Overview

DA.Result is a Result monad implementation for C#, developed by DutchAntimony for personal projects.
In the basics, the 

## Inspiration

DA.Result draws inspiration from the following projects:
- [Ardalis.Results](https://github.com/ardalis/result)
- [FluentResults](https://www.nuget.org/packages/FluentResults)
- [CSharpFunctionalExtensions.Result](https://github.com/vkhorikov/CSharpFunctionalExtensions/tree/master/CSharpFunctionalExtensions/Result)

## Features

A Result can either be a Success with a value, or a Failure with an Issue.
It is represented as a Result<T>, where T can optionally be NoContent.
The features are as follows:

- Adheres to naming conventions of functional programming languages like F# and Haskell
- Fully supports asynchronous Tasks returning options without requiring prior task awaiting
- Warnings that can configurably ignored or causing an issue.
- Possibility to add additional Issues with the same functionality as the included issues.
- Comprehensive testing with nearly 100% code coverage using XUnit
 
## Syntax

- Construction of a Result:
```csharp
var SuccessResult = Result.Ok(IgnoreWarnings: true); // Result<Result.NoContent>
var SuccessResultInt = Result.Ok(20); // Result<int>
Result<int> SuccessResultInt = 20; // alternative declaration - IgnoreWarnings will be false.

var FailureResult = Result.Fail("Message"); // Result<Result.NoContent> with a new InvalidOperationError containing the message.
Result<int> FailureResultInt = Result.Fail<int>("Message"); // idem.
Result<int> FailureResultInt = new InvalidOperationError("Message"); // also the same.
```

- Every result inherits from IResult, which exposes a few methods:
```csharp
public class Result<TValue> : IResult;
public bool IsSuccess { get; }
public bool IgnoreWarnings { get; } // defaults to false.
public bool TryGetIssue([NotNullWhen(true)] out Issue? issue); // to get a handle on the issue.
public Type GetValueType(); // the type of the TValue.
```

## Issues

The result library comes with a couple of predefined Issues:

| IssueName                   | Parameters                       | Description                                                      | StatusCode |
|-----------------------------|----------------------------------|------------------------------------------------------------------|------------|
| ConfirmationRequiredWarning | string Message                   | Action can only be performed when IgnoreWarnings is set to true. | 400        |
| InternalServerError         | Exception exception              | An exception was caught and returned as a result.                | 500        |
| InvalidOperationError       | string Message                   | A attempt to do something that should not happen was made.       | 400        |
| UnmodifiedWarning           | IEnumerable{Type} Types          | The action did not lead to any changes in the object.            | 204        |
| ValidationError             | List{ValidationFailure} Failures | The action caused one or more validation failures.               | 400        |

## Extension methods

The following extension methods are defined for the Result{TValue}. 
Below only the direct extensions are shown, the same extensions are also present, with the same name, for all Task{Result{TValue}}:

- Bind: Convert a Result{TIn} to a Result{TOut} by supplying a Result{TOut}. It returns:
  - A failure with the original issue if the original result was a failure.
  - A failure with the second issue if the second result was a failure.
  - A success with the second value and the original IgnoreWarnings configuration.
```csharp
public static class BindResultExtensions
{
    public static Result<TOut> Bind<TIn, TOut>(this Result<TIn> first, Func<TIn, Result<TOut>> secondFunc);
    public static async Task<Result<TOut>> BindAsync<TIn, TOut>(this Result<TIn> first, Func<Task<Result<TOut>>> secondTask);
    public static async Task<Result<TOut>> BindAsync<TIn, TOut>(this Result<TIn> first, Func<TIn, Task<Result<TOut>>> secondTaskFunc);
    public static async Task<Result<TOut>> Bind<TIn, TOut>(this Task<Result<TIn>> firstTask, Func<TIn, Result<TOut>> secondFunc)
}
```
- Check: Checks a predicate(which returns a result for the messages) on the T of the Result {T}. Result is the same T.
```csharp
public static class CheckResultExtensions
{
    public static Result<TValue> Check<TValue>(this Result<TValue> result, Func<TValue, IResult> checkFunc)
    public static async Task<Result<TValue>> CheckAsync<TValue>(this Result<TValue> result, Func<Task<IResult>> checkTask)
    public static async Task<Result<TValue>> CheckAsync<TValue>(this Result<TValue> result, Func<TValue, Task<IResult>> checkFuncTask)
}
```
- CheckIf: Like check, but only if a predicate is matched. Every example below also has an overload where the predicate is independent of TValue.
```csharp
public static class CheckResultExtensions
{
    public static Result<TValue> CheckIf<TValue>(this Result<TValue> result, Func<TValue, bool> predicate, Func<TValue, IResult> checkFunc)
    public static async Task<Result<TValue>> CheckIf<TValue>(this Task<Result<TValue>> resultTask, Func<TValue, bool> predicate, Func<TValue, IResult> checkFunc)
    public static async Task<Result<TValue>> CheckIfAsync<TValue>(this Result<TValue> result, Func<TValue, bool> predicate, Func<Task<IResult>> checkTask)
    public static async Task<Result<TValue>> CheckIfAsync<TValue>(this Result<TValue> result, Func<TValue, bool> predicate, Func<TValue, Task<IResult>> checkFuncTask)   
}
```
- Work in progress: documentation for CheckForEach, Combine, Compensate, Map, Match and Tap.
## Dependent packages

There are two additional packages that depent on DA.Results:

- DA.Results.MinimalApi: Extension methods to convert a result to a Http.IResult for use in a minimal api environment.
- DA.Results.Shouldly: Extension methods to verify a result, for use in testing projects.

