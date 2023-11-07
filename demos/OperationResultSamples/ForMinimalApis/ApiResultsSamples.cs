
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RoyalCode.OperationResults;
using RoyalCode.OperationResults.HttpResults;

namespace OperationResultSamples.ForMinimalApis;

public class MinimalApiSamples
{

    public async Task<IResult> Create(
        [FromBody] ProductDto productDto,
        [FromServices] IProductService productService)
    {
        var result = await productService.CreateProductAsync(productDto);

        return Results.Extensions.ToResult(result, p => $"products/{p.Id}");
    }

    public async Task<CreatedMatch<Product>> Create_2(
        [FromBody] ProductDto productDto,
        [FromServices] IProductService productService)
    {
        var result = await productService.CreateProductAsync(productDto);

        return result.CreatedMatch(p => $"products/{p.Id}");
    }

    public Results<Ok<int>, BadRequest> SampleWithResults()
    {
        return TypedResults.Ok(1);
    }

    [HttpGet("{id}")]
    public static OkMatch<Product> Get(int id, [FromServices] IProductService productService)
    {
        Product? result = productService.Find(id);

        if (result is null)
            return ResultMessage.NotFound("Product not found", nameof(id));

        return result;
    }

    [HttpGet("{id}")]
    public static async Task<OkMatch<Product>> GetAsync(int id, [FromServices] IProductService productService)
    {
        OperationResult<Product> result = await productService.GetProductAsync(id);
        return result;
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
    Product? Find(int id);

    Task<OperationResult<Product>> GetProductAsync(int id);

    Task<OperationResult<Product>> CreateProductAsync(ProductDto productDto);

    Task<OperationResult<Product>> UpdateProductAsync(int id, ProductDto productDto);    
}