namespace RoyalCode.OperationResults.TestApi.Application.Pizzas
{
    /// <summary>
    /// <para>
    ///     Ingrediente da pizza.
    /// </para>
    /// <para>
    ///     Os ingredientes compõem os sabores das pizzas.
    /// </para>
    /// </summary>
    public class Ingrediente : BaseEntity
    {
        /// <summary>
        /// Nome do ingrediente.
        /// </summary>
        public required string Nome { get; set; }
    }
}
