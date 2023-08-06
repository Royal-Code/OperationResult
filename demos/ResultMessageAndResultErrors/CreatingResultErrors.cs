
using RoyalCode.OperationResults;

namespace ResultMessageAndResultErrors;

public class CreatingResultErrors
{
    public ResultErrors Create_New()
    {
        // Create a empty ResultErrors.
        var result = new ResultErrors();

        // add a message
        var message = ResultMessage.Error("Error message");
        result.Add(message);

        return result;
    }

    public ResultErrors Create_AddingMessages()
    {
        // Create a empty ResultErrors.
        var result = new ResultErrors();

        // add many messages with the With method
        result.With(ResultMessage.Error("Error message 1"))
            .With(ResultMessage.Error("Error message 2"))
            .With(ResultMessage.Error("Error message 3"));

        return result;
    }

    public ResultErrors Create_FromImplicitConversion()
    {
        // Create a ResultMessage.
        ResultMessage message = ResultMessage.Error("Error message");

        // Implicit conversion to ResultErrors.
        ResultErrors result = message;

        return result;
    }

    public ResultErrors Create_AddingMessages_FromExplicitConversion()
    {
        // Create a empty ResultErrors.
        var result = new ResultErrors();

        result += ResultMessage.Error("Error message 1");
        result += ResultMessage.Error("Error message 2");
        result += ResultMessage.Error("Error message 3");

        return result;
    }
}
