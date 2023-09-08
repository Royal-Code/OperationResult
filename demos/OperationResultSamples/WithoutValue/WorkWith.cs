
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

    private void DoMore()
    {
        throw new NotImplementedException();
    }

    public OperationResult DoSomething()
    {
        return new();
    }
}
