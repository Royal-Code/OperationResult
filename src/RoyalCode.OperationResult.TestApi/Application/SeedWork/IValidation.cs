namespace RoyalCode.OperationResults.TestApi.Application.SeedWork;

public interface IValidation { }

public interface IValidation<in T> : IValidation
{
    public Task OnAdding(WorkContext workContext, T entity);

    public Task OnUpdating(WorkContext workContext, T entity);

    public Task OnDeleting(WorkContext workContext, T entity);
}
