
using RoyalCode.OperationResults;

namespace OperationResultSamples.WithoutValue;

public class Creating
{

    public OperationResult Create_Success_1()
    {
        var result = new OperationResult();

        return result;
    }

    public OperationResult Create_Success_2()
    {
        OperationResult result = new();

        return result;
    }

    public OperationResult Create_Failure_1()
    {
        var errors = new ResultErrors();
        errors += ResultMessage.Error("Error message");

        var result = new OperationResult(errors);

        return result;
    }

    public OperationResult Create_Failure_2()
    {
        var errors = new ResultErrors();
        errors += ResultMessage.Error("Error message");

        OperationResult result = errors;

        return result;
    }

    public OperationResult Create_Failure_3()
    {
        OperationResult result = ResultMessage.Error("Error message");

        return result;
    }
}