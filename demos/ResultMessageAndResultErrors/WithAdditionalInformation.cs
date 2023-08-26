
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
            .WithInformation("Id", id)
            .WithInformation("CurrentValue", value)
            .WithInformation("RequiredValue", required);

        return message;
    }

    public OperationResult CreateMessage_ForbiddenError(PaymentRequest request, Account account)
    {
        if (account.Balance < request.Amount)
        {
            var message = ResultMessage.Forbidden("out-of-credit", 
                    $"Your current balance is {account.Balance}, but that costs {request.Amount}.", 
                    nameof(PaymentRequest.Amount))
                .WithInformation(nameof(Account.Balance), account.Balance)
                .WithInformation(nameof(PaymentRequest.Amount), request.Amount);

            return message;
        }

        return new();
    }
}

public class PaymentRequest
{
    public decimal Amount { get; set; }

    public string CreditCardNumber { get; set; }

    public string CreditCardExpiration { get; set; }

    public string CreditCardSecurityCode { get; set; }
}

public class Account
{
    public decimal Balance { get; set; }
}