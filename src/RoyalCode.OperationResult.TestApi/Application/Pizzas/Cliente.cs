namespace RoyalCode.OperationResults.TestApi.Application.Pizzas;

/// <summary>
/// Cliente que fez o pedido.
/// </summary>
public class Cliente
{
    /// <summary>
    /// Nome do cliente.
    /// </summary>
    public required string Nome { get; set; }

    /// <summary>
    /// Documento que identifica o cliente, como CPF ou CNPJ.
    /// </summary>
    public required string Documento { get; set; }

    /// <summary>
    /// O endereço de entrega, completo.
    /// </summary>
    public required string Endereco { get; set; }
}
