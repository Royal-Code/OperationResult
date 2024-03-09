namespace RoyalCode.OperationResults.TestApi.Application.Pizzas;

/// <summary>
/// <para>
///     A pizza é um produto que pode ser comprado por um cliente.
/// </para>
/// <para>
///     O cliente define o tamanho da pizza e os sabores que deseja.
/// </para>
/// <para>
///     Para cada tamanho existe um número de sabores que precisam ser informados.
/// </para>
/// <para>
///     Os sabores podem ser repetidos.
/// </para>
/// </summary>
public class Pizza
{
    /// <summary>
    /// Cliente que está comprando a pizza.
    /// </summary>
    public required Cliente Cliente { get; set; }

    /// <summary>
    /// Tamanho da pizza.
    /// </summary>
    public Tamanho Tamanho { get; set; }

    /// <summary>
    /// Os sabores da pizza.
    /// </summary>
    public ICollection<Sabor> Sabores { get; private set; } = new List<Sabor>();
}
