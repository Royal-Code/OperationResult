
using RoyalCode.OperationResults;

namespace ResultMessageAndResultErrors;

public class WithAdditionalInformation
{
    public ResultMessage AddingExtraData()
    {
        // variables for the example
        var id = Guid.NewGuid();
        var value = 10;
        var required = 20;

        // create the message and add extra data
        var message = ResultMessage.Conflict("error-code, Error message", "PropertyName")
            .WithAdditionInfo("Id", id)
            .WithAdditionInfo("CurrentValue", value)
            .WithAdditionInfo("RequiredValue", required);

        return message;
    }
}
