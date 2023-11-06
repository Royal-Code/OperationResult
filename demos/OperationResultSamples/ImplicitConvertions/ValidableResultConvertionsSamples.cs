
using RoyalCode.OperationResults;

namespace OperationResultSamples.ImplicitConvertions;

public class ValidableResultConvertionsSamples
{

    public ValidableResult ConvertSample_1(OperationResult operationResult)
    {
        ValidableResult validableResult = operationResult;
        return validableResult;
    }

    public ValidableResult OperatorSample_1(ValidableResult result, ResultMessage message)
    {
        result += message;
        return result;
    }

    public ValidableResult OperatorSample_2(ValidableResult result, ResultErrors errors)
    {
        result += errors;
        return result;
    }
    
    public ValidableResult OperatorSample_3(ValidableResult result, ValidableResult otherResult)
    {
        result += otherResult;
        return result;
    }

    public ValidableResult OperatorSample_4(ValidableResult result, OperationResult otherResult)
    {
        result += otherResult;
        return result;
    }
}