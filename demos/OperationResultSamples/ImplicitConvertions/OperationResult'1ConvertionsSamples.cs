
namespace RoyalCode.OperationResults;

public class OperationResultConvertionsSamples<TValue>
{
    public OperationResult<TValue> ConvertSample_1(TValue value)
    {
        OperationResult<TValue> operationResult = value;
        return operationResult;
    }

    public OperationResult<TValue> ConvertSample_2(ResultErrors errors)
    {
        OperationResult<TValue> operationResult = errors;
        return operationResult;
    }

    public OperationResult<TValue> ConvertSample_3(ResultMessage message)
    {
        OperationResult<TValue> operationResult = message;
        return operationResult;
    }

    public OperationResult<TValue> OperatorSample_1(OperationResult<TValue> result, ResultMessage message)
    {
        result += message;
        return result;
    }

    public OperationResult<TValue> OperatorSample_2(OperationResult<TValue> result, ResultErrors errors)
    {
        result += errors;
        return result;
    }

    public OperationResult<TValue> OperatorSample_3(OperationResult<TValue> result, ValidableResult validableResult)
    {
        result += validableResult;
        return result;
    }

    public OperationResult<TValue> OperatorSample_4(OperationResult<TValue> result, OperationResult otherResult)
    {
        result += otherResult;
        return result;
    }

    public OperationResult<TValue> OperatorSample_5(OperationResult<TValue> result, OperationResult<TValue> otherResult)
    {
        result += otherResult;
        return result;
    }
}