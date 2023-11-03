using RoyalCode.OperationResults;
using System;

namespace OperationResultSamples.WithValue;

public class WorkWith
{
    
    public void TryGetValue_Sample()
    {
        OperationResult<Product> result = Create("Product 1", 10.5m);

        if (result.TryGetValue(out var product))
        {
            DoSomething(product);
        }
    }

    public void TryConvertValue_Sample_1()
    {
        OperationResult<Product> result = Create("Product 1", 10.5m);

        if (result.TryConvertValue(static product => product.Name, out var name))
        {
            DoSomething(name);
        }
    }

    public TOtherValue? TryConvertValue_Sample_2<TOtherValue>(ISelector<Product, TOtherValue> selector)
        where TOtherValue : class
    {
        OperationResult<Product> result = Create("Product 1", 10.5m);

        if (result.TryConvertValue(selector, static (p, s) => s.Select(p), out TOtherValue? other))
        {
            return other;
        }

        return default;
    }

    public OperationResult IsFailureOrGetValue_Sample_1()
    {
        OperationResult<Product> result = Create("Product 1", 10.5m);

        if (result.IsFailureOrGetValue(out var product))
        {
            return result;
        }

        var otherResult = DoSomethingElse(product);

        return otherResult;
    }

    public OperationResult IsFailureOrConvertValue_Sample_1()
    {
        OperationResult<Product> result = Create("Product 1", 10.5m);

        if (result.IsFailureOrConvertValue(static product => product.Name, out var name))
        {
            return result;
        }

        var otherResult = DoSomethingElse(name);

        return otherResult;
    }

    public OperationResult<TOtherValue> IsFailureAndGet_Sample<TOtherValue>()
    {
        OperationResult<Product> result = Create("Product 1", 10.5m);

        if (result.IsFailureAndGet(out var errors, out var product))
        {
            // if the result is failure, the errors are returned
            return errors;
        }

        // if the result is success, the value is returned
        // now you can do something with the value
        TOtherValue otherValue = DoOtherThing<TOtherValue>(product);
        return otherValue;
    }

    public OperationResult<TOtherValue> IsSuccessAndGet_Sample<TOtherValue>()
    {
        OperationResult<Product> result = Create("Product 1", 10.5m);

        if (result.IsSuccessAndGet(out var product, out var errors))
        {
            // if the result is success, the value is returned
            // now you can do something with the value
            TOtherValue otherValue = DoOtherThing<TOtherValue>(product);
            return otherValue;
        }

        // if the result is failure, the errors are returned
        return errors;
    }

    #region Helpers

    private OperationResult<Product> Create(string name, decimal price)
    {
        return new Product(name, price);
    }

    private void DoSomething(Product product)
    {
        Console.WriteLine($"Product {product.Name} created with price {product.Price}");
    }

    private OperationResult DoSomethingElse(Product product)
    {
        Console.WriteLine($"Product {product.Name} created with price {product.Price}");
        return new();
    }

    private OperationResult DoSomethingElse(string name)
    {
        Console.WriteLine($"Product {name} created");
        return new();
    
    }

    private void DoSomething(string name)
    {
        Console.WriteLine($"Product {name} created");
    }

    private TOtherValue DoOtherThing<TOtherValue>(Product product)
    {
        return default!;
    }

    public interface ISelector<TIn, TOut>
    {
        TOut Select(TIn input);
    }

    #endregion
}