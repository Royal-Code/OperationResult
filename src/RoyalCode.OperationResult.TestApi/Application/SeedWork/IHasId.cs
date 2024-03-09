namespace RoyalCode.OperationResults.TestApi.Application.SeedWork;

public interface IHasId<out TId>
{
    TId Id { get; }
}
