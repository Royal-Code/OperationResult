namespace RoyalCode.OperationResults.TestApi.Application.Pizzas
{
    /// <summary>
    /// <para>
    ///     Estoque de ingredientes.
    /// </para>
    /// </summary>
    public class Estoque : BaseEntity
    {
        /// <summary>
        /// Ingrediente.
        /// </summary>
        public required Ingrediente Ingrediente { get; set; }

        /// <summary>
        /// <para>
        ///     Quantidade do ingrediente em estoque.
        /// </para>
        /// <para>
        ///     As quantidades são armazenadas em porções para um 1 sabor de uma pizza.
        ///     O tamanho da pizza define a quantidade de sabores que ela pode ter.
        /// </para>
        /// </summary>
        public required int Quantidade { get; set; }
    }
}
