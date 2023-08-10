
using RoyalCode.OperationResults;

namespace ValidableResultSamples;

public class CreatingValidableResults
{
    public ValidableResult Create_Success()
    {
        var result = new ValidableResult();

        return result;
    }

    public ValidableResult Create_Failure()
    {
        var result = new ValidableResult();
        result += ResultMessage.Error("Error message");

        return result;
    }

    public ValidableResult Create_Failure_FromResultErros()
    {
        var errors = new ResultErrors();
        errors += ResultMessage.Error("Error message");

        var result = new ValidableResult(errors);

        return result;
    }
}
