# OperationResult

## Description
The OperationResult library is an essential component for handling the return of operations in systems developed in .Net. It offers a few main classes/structs, for different situations, called OperationResult, ValidableResult and ResultMessage. Designed to simplify the communication of success or failure information between different parts of the system, such as use cases, APIs, Controllers and HTTP requests. 
It supports conversion to ProblemDetails and has components for serialization and deserialization.

## Key features
- Standardized returns: `OperationResult` is a struct that contains the result of an operation, or function, of a system. In case of failure the return will contain messages to inform the problem occurred. This is lighter than firing exceptions.
- Generic Return: The struct `OperationResult` also has its own generic version, `OperationResult<TValue>`, allowing you to return results from different types of operations in your system consistently and efficiently.
- Volatile results: The struct `ValidableResult` allows you to add error messages, allowing you to use them in validation operations.
- Standardized messages: The library facilitates the creation of standardized messages through the `ResultMessage` class.
- Error Codes: The `ResultMessage` class allows you to associate meaningful error codes with the results of unsuccessful operations, making it easier to identify and deal with problems.
- Error Description and Additional Data: In addition to error codes, you can include detailed descriptions of the errors that occurred, including additional information, which helps debugging and troubleshooting faster and more efficiently.
- Results for Minimal API and Controllers: The library offers features for generating results suitable for systems based on Minimal API and Controllers, speeding up development and making interaction with the presentation layer easier.
- Conversion to `ProblemDetails`: The conversion of results into objects of type `ProblemDetails` is facilitated by the library, allowing the standardization of error presentation according to the RFC 7807 specification.
- Serialization and Deserialization: The `ResultMessage` class provides support for serialization and deserialization, making the exchange of information between different system components more practical and consistent.
- Conversion from HTTP responses: The library has methods to generate `OperationResult` from HTTP responses, deserilizing the messages and converting `ProblemDetails`, also supporting plain text.

## How to use
It is simple to start using the OperationResult library in your project:

1) Install the NuGet package: `dotnet add package RoyalCode.OperationResult`.

2) Import the namespace into your code:

```cs
using OperationResults;
```

3) Create and return an operation result in your classes and methods:
