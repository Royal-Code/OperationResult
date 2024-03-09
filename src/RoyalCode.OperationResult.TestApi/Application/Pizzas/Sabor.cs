namespace RoyalCode.OperationResults.TestApi.Application.Pizzas;

/// <summary>
/// <para>
///     Sabor de pizza.
/// </para>
/// <para>
///     Um sabor é composto por uma lista de ingredientes.
/// </para>
/// </summary>
public class Sabor : BaseEntity
{
    /// <summary>
    /// Nome do sabor.
    /// </summary>
    public required string Nome { get; set; }

    /// <summary>
    /// Ingredientes do sabor.
    /// </summary>
    public ICollection<Ingrediente> Ingredientes { get; private set; } = new List<Ingrediente>();
}
