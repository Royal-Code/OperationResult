using RoyalCode.OperationResults.TestApi.Application.Pizzas;
using RoyalCode.OperationResults.TestApi.Application.WeatherForecasts;
using System.Collections.Concurrent;
using System.Text.Json;

namespace RoyalCode.OperationResults.TestApi.Application.SeedWork;

public class WorkContext
{
    private readonly Dictionary<Type, ConcurrentDictionary<Guid, string>> database = new();
    private readonly IEnumerable<IValidation> validations;

    public WorkContext(IEnumerable<IValidation> validations)
    {
        database.Add(typeof(WeatherForecast), new ConcurrentDictionary<Guid, string>());
        database.Add(typeof(Ingrediente), new ConcurrentDictionary<Guid, string>());
        database.Add(typeof(Estoque), new ConcurrentDictionary<Guid, string>());
        database.Add(typeof(Pizza), new ConcurrentDictionary<Guid, string>());
        database.Add(typeof(Sabor), new ConcurrentDictionary<Guid, string>());
        this.validations = validations;
    }

    public void Add<T>(T entity)
        where T : IHasId<Guid>
    {
        var type = typeof(T);
        var id = entity.Id;

        // validates entity
        foreach (var validation in validations.OfType<IValidation<T>>())
        {
            validation.OnAdding(this, entity);
        }

        // Serializes entity to json
        string serializedEntity = JsonSerializer.Serialize(entity);

        if (!database[type].TryAdd(id, serializedEntity))
            throw new InvalidOperationException($"Entity {type.Name} with id {id} already exists.");
    }

    public T? Get<T>(Guid id)
        where T : IHasId<Guid>
    {
        var type = typeof(T);
        if (database[type].TryGetValue(id, out var entityJson))
        {
            // Deserializes entity from json
            var entity = JsonSerializer.Deserialize<T>(entityJson);

            return entity;
        }

        return default;
    }

    public void Update<T>(T entity)
        where T : IHasId<Guid>
    {
        // validates entity
        foreach (var validation in validations.OfType<IValidation<T>>())
        {
            validation.OnUpdating(this, default!);
        }

        var type = typeof(T);
        var id = entity.Id;

        // Serializes entity to json
        string serializedEntity = JsonSerializer.Serialize(entity);

        if (!database[type].TryGetValue(id, out var currentJson))
            throw new InvalidOperationException($"Entity {type.Name} with id {id} does not exist.");

        if (!database[type].TryUpdate(id, serializedEntity, currentJson))
            throw new InvalidOperationException($"Entity {type.Name} with id {id} was updated by another process.");
    }

    public bool Remove<T>(Guid id)
    {
        // validates entity
        foreach (var validation in validations.OfType<IValidation<T>>())
        {
            validation.OnDeleting(this, default!);
        }

        var type = typeof(T);
        return database[type].TryRemove(id, out _);
    }
}

