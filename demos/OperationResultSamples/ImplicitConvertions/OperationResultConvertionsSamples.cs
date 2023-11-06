
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace RoyalCode.OperationResults;

public class OperationResultConvertionsSamples
{
    public OperationResult ConvertSample_1(ValidableResult validableResult)
    {
        OperationResult operationResult = validableResult;
        return operationResult;
    }

    public OperationResult ConvertSample_2(ResultMessage message)
    {
        OperationResult operationResult = message;
        return operationResult;
    }

    public OperationResult ConvertSample_3(ResultErrors errors)
    {
        OperationResult operationResult = errors;
        return operationResult;
    }

    public OperationResult ConvertSample_4<TValue>(OperationResult<TValue> otherResult)
    {
        OperationResult operationResult = otherResult;
        return operationResult;
    }

    public OperationResult OperatorSample_1(OperationResult result, ResultMessage message)
    {
        result += message;
        return result;
    }

    public OperationResult OperatorSample_2(OperationResult result, ResultErrors errors)
    {
        result += errors;
        return result;
    }

    public OperationResult OperatorSample_3(OperationResult result, ValidableResult validableResult)
    {
        result += validableResult;
        return result;
    }

    public OperationResult OperatorSample_4(OperationResult result, OperationResult otherResult)
    {
        result += otherResult;
        return result;
    }
}