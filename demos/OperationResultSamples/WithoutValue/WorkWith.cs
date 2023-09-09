
using RoyalCode.OperationResults;
using System;

namespace OperationResultSamples.WithoutValue;

public class WorkWith
{
    public void TryConvertError_ToException()
    {
        OperationResult result = DoSomething();

        if (result.TryConvertError(static errors => errors.CreateException(), out var exception))
            throw exception;

        DoMore();
    }


    public OperationResult<SomeValueObject> IsSuccessOrGetErrorSample()
    {
        OperationResult result = DoSomething();

        if (result.IsSuccessOrGetError(out var error))
        {
            SomeValueObject value = DoSomethingElse();
            return value;
        }

        return error;
    }

    public SomeValueObject IsSuccessOrConvertErrorSample()
    {
        OperationResult result = DoSomething();

        if (result.IsSuccessOrConvertError(static errors => errors.CreateException(), out var exception))
        {
            SomeValueObject value = DoSomethingElse();
            return value;
        }

        throw exception;
    }

    private void DoMore()
    {
        throw new NotImplementedException();
    }

    public OperationResult DoSomething()
    {
        return new();
    }

    public SomeValueObject DoSomethingElse()
    {
        return new("value");
    }

    public record SomeValueObject(string Value);
}
