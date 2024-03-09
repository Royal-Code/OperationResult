using RoyalCode.OperationResults.TestApi.Application.SeedWork;

namespace RoyalCode.OperationResults.TestApi.Application.Pizzas;

/// <summary>
/// Classe base para as entidades
/// </summary>
public abstract class BaseEntity : IHasId<Guid>
{
    /// <summary>
    /// Construtor padrão que gera um novo identificador único
    /// </summary>
    protected BaseEntity()
    {
        Id = Guid.NewGuid();
    }

    /// <summary>
    /// Identificador único do registro
    /// </summary>
    public Guid Id { get; protected set; }

    /// <summary>
    /// Se o registro está inativo
    /// </summary>
    public bool Inativo { get; set; }
}
