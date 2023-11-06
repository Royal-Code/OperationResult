
using Microsoft.AspNetCore.Mvc;
using RoyalCode.OperationResults;

namespace OperationResultSamples.ForControllers;

public class ControllersResultsSamples
{

    [HttpGet("products/{id}")]
    public async Task<ActionResult<Product>> GetById([FromServices] IProductService service, int id)
    {
        OperationResult<Product> result = await service.GetProductAsync(id);

        return result.ToActionResult();
    }

    [HttpPost("products")]
    public async Task<ActionResult<Product>> Create(
        [FromServices] IProductService service,
        [FromBody] ProductDto dto)
    {
        OperationResult<Product> result = await service.CreateProductAsync(dto);

        return result.ToActionResult(p => $"products/{p.Id}");
    }

    [HttpPost("products")]
    public async Task<ActionResult<Product>> Create_2(
        [FromServices] IProductService service,
        [FromBody] ProductDto dto)
    {
        OperationResult<Product> result = await service.CreateProductAsync(dto);

        var idResult = result.Convert(p => p.Id);

        return idResult.ToActionResult("products/{0}", true);
    }
}

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
}

public class ProductDto
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
}

public interface IProductService
{
    Task<OperationResult<Product>> GetProductAsync(int id);

    Task<OperationResult<Product>> CreateProductAsync(ProductDto productDto);

    Task<OperationResult<Product>> UpdateProductAsync(int id, ProductDto productDto);    
}