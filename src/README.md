# OperationResult

## Description
The **OperationResult** library is an essential component for handling the return of operations in systems developed in **.Net**. Designed to simplify the communication of success or failure between different parts of the system, such as: use cases, APIs, Controllers and HTTP requests, it offers a few main structs/classes, for different situations, called OperationResult, ValidableResult, ResultMessage and ResultErros, also, it supports conversion to ProblemDetails and has components for serialization and deserialization.

## Key features
- **Standardized returns**: `OperationResult` is a struct that contains the result of an operation, or function, of a system. In case of failure the return will contain messages to inform the problem occurred. This is lighter than firing exceptions.
- **Generic Return**: The struct `OperationResult` also has its own generic version, `OperationResult<TValue>`, allowing you to return results from different types of operations in your system consistently and efficiently.
- **Volatile results**: The struct `ValidableResult` allows you to add error messages, allowing you to use them in validation scenarios.
- **Standardized messages**: The library facilitates the creation of standardized messages through the `ResultMessage` class.
- **Error Codes**: The `ResultMessage` class allows you to associate meaningful error codes with the results of unsuccessful operations, making it easier to identify and deal with problems.
- **Error Description and Additional Data**: In addition to error codes, you can include detailed descriptions of the errors that occurred, including additional information, which helps debugging and troubleshooting faster and more efficiently.
- **Results for Minimal API and Controllers**: The library offers features for generating results suitable for systems based on Minimal API and Controllers, speeding up development and making interaction with the presentation layer easier.
- **Conversion to `ProblemDetails`**: The conversion of results into objects of type `ProblemDetails` is facilitated by the library, allowing the standardization of error presentation according to the RFC 7807 specification.
- **Serialization and Deserialization**: The `ResultMessage` class provides support for serialization and deserialization, making the exchange of information between different system components more practical and consistent.
- **Conversion from HTTP responses**: The library has methods to generate `OperationResult` from HTTP responses, deserilizing the messages and converting `ProblemDetails`, also supporting plain text.
- **Implicit operation e convertions**: There are implicit operations to, for example, use `+=` to add messages to the error collection and conversions between errors, message values and return types.
- **Monad aspect**: The `OperationResult` encapsulates the value or collection of errors depending on whether the result is success or failure. To work with these values, there are methods to extract or convert them.

## How to use

It is simple to start using the **OperationResult** library in your project:

1) Install the NuGet package: `dotnet add package RoyalCode.OperationResult`.

2) Import the namespace into your code:

```cs
using OperationResults;
```

3) Create a method that returns an `OperationResult<T>` and returns an error message or the value.:

```cs
public OperationResult<MyModel> DoSomething(string input)
{
    if (string.IsNullOrEmpty(input))
        return ResultMessage.InvalidParameter("Some error message", nameof(input));

    return new MyModel(input);
}
```

4 - You can also return an `OperationResult` in minimal APIs, using methods for converting to `IResult`:

```cs
// MapPost("/products")
public static async Task<CreatedMatch<Product>> Create(
    ProductDto productDto /* FromBody */,
    IProductService productService /* FromServices */)
{
    OperationResult<Product> result = await productService.CreateProductAsync(productDto);

    return result.CreatedMatch(p => $"products/{p.Id}");
}
```

5 - There are implicit conversions for the **'Match'** types in the library, such as the example for `OkMatch<T>`:

```cs
// MapGet("/products/{id}")
public static OkMatch<Product> Get(int id, IProductService productService)
{
    Product? product = productService.Find(id);

    if (product is null)
        return ResultMessage.NotFound("Product not found", nameof(id));

    return product;
}
```

6 - You can also convert an `HttpResponseMessage` to an `OperationResult`:

```cs
var response = await client.GetAsync("api/products/1");
OperationResult<ProductDto> result = await response.ToOperationResultAsync<ProductDto>();
```

See more details in the [documentation](https://github.com/Royal-Code/OperationResult/tree/main/docs).
