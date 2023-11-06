# Docs for OperationResult

Browse the archives to read about each documented topic.

The documentation is divided into:

- **`ResultMessage` and `ResultErrors`**: Classes that standardize error messages. It will be demonstrated how to create them.
- **`ValidableResult`**: A struct of a result for validation scenarios, where you can add several error messages.
- **`OperationResult`**: A struct of a result that can be a success or a failure.
- **`OperationResult<T>`**: A struct of a result that can be success or failure, containing a return value.
- **Implicit Conversions and Operators**: Implicit conversions between message classes and result structs will be presented.
- **Results for Controllers**: It will be demonstrated how to convert operation results into result objects of controllers.
- **Results for Minimal API**: It will be demonstrated how to convert operation results into result objects for Minimal API.
- **Implicit conversions for Minimal API**: Implicit conversions between operation result structs and Minimal API results will be presented.
- **`ProblemDetails`**: Support for converting operation results into `ProblemDetails` will be demonstrated.
- **Extensions for HTTP**: The use of extensions for HTTP call responses will be demonstrated, converting the response into an operation result.
