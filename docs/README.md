# Docs for OperationResult

Navegue pelos arquivos para ler sobre cada tópico documentado.

A documentação é dividida em:

- **`ResultaMessage` e `ResultErrors`**: Classes que padronizam as mensagens de erro. Será demonstrado como criá-las.
- **`ValidableResult`**: Um struct de resultado para cenários de validação, onde pode se adicionar várias mensagens de erro.
- **`OperationResult`**: Um struct de resultado que pode ser de sucesso ou de falha.
- **`OperationResult<T>`**: Um struct de resultado que pode ser de sucesso ou falha, contendo um valor de retorno.
- **Conversões Implicitas**: Serão apresentadas as conversões implicitas entre as classes de mensagem e os structs de resultado.
- **Resultados para Controllers**: Será demonstrado como converter resultados de operação em objetos de resultado de controllers.
- **Resultados para Minimal API**: Será demonstrado como converter resultados de operação em objetos de resultado para Minimal API.
- **Conversões implicitas para Minimal API**: Serão apresentadas as conversões implicitas entre os structs de resultado de operação e resultados de Minimal API.
- **`ProblemDetails`**: Será demonstrado o suporte para conversão de resultados de operações em `ProblemDetails`.
- **Extensões para HTTP**: Será demostrado o uso das extensões para respostas de chamadas HTTP, convertendo a resposta em resulado de operação.
