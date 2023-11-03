
using RoyalCode.OperationResults;

namespace OperationResultSamples.WithValue;

public class Creating
{

    public OperationResult<Product> Create_Success_1()
    {
        var product = new Product("Product name", 10.0m);

        var result = new OperationResult<Product>(product);

        return result;
    }

    public OperationResult<Product> Create_Success_2()
    {
        var product = new Product("Product name", 10.0m);

        OperationResult<Product> result = new(product);

        return result;
    }

    public OperationResult<Product> Create_Success_3()
    {
        var product = new Product("Product name", 10.0m);

        OperationResult<Product> result = product;

        return result;
    }

    public OperationResult<Product> Create_Success_4()
    {
        var product = new Product("Product name", 10.0m);

        return product;
    }

    public OperationResult<Product> Create_Failure_1()
    {
        var errors = new ResultErrors();
        errors += ResultMessage.Error("Error message");

        var result = new OperationResult<Product>(errors);

        return result;
    }

    public OperationResult<Product> Create_Failure_2()
    {
        var errors = new ResultErrors();
        errors += ResultMessage.Error("Error message");

        OperationResult<Product> result = errors;

        return result;
    }

    public OperationResult<Product> Create_Failure_3()
    {
        OperationResult<Product> result = ResultMessage.Error("Error message");

        return result;
    }

    public OperationResult<Product> Create_Failure_4(string name, decimal value)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return ResultMessage.Error("Name is required");
        }

        if (value <= 0)
        {
            return ResultMessage.Error("Value must be greater than zero");
        }

        return new Product(name, value);
    }

    public OperationResult<Product> Create_Failure_5(string name, decimal value)
    {
        ValidableResult validation = new();
        
        if (string.IsNullOrWhiteSpace(name))
            validation += ResultMessage.Error("Name is required");

        if (value <= 0)
            validation += ResultMessage.Error("Value must be greater than zero");

        if (validation.TryGetError(out var error))
            return error;


        return new Product(name, value);
    }
}