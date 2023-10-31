using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductManagerWebAPI.Data;
using ProductManagerWebAPI.Domain;
using ProductManagerWebAPI.DTO;
using System.ComponentModel.DataAnnotations;

namespace ProductManagerWebAPI.Controllers;

[Authorize]
[Route("[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    // Dependency Injection
    private readonly ApplicationDbContext context;

    public ProductsController(ApplicationDbContext context)
    {
        this.context = context;
    }

    /// <summary>
    /// Get all products
    /// </summary>
    /// <returns>Array of products</returns>
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<List<ProductDto>> GetProducts([FromQuery] string? name)
    {
        IEnumerable<Product> products = name is not null
            ? context.Products.Where(x => x.Name.Contains(name))
            : context.Products.ToList();

        var response = products.Select(product => new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            StockKeepingUnit = product.StockKeepingUnit,
            Description = product.Description,
            ImageURL = product.ImageURL,
            Price = product.Price,
        });

        return Ok(response);
    }

    /// <summary>
    /// Get one product
    /// </summary>
    /// <param name="stockKeepingUnit">SKU</param>
    /// <returns>Product</returns>
    [HttpGet("{stockKeepingUnit}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<ProductDto> GetProduct(string stockKeepingUnit)
    {
        var product = context.Products.FirstOrDefault( x => x.StockKeepingUnit == stockKeepingUnit);

        if (product == null)
        {
            return NotFound();
        }

        var response = new ProductDto()
        {
            Id = product.Id,
            Name = product.Name,
            StockKeepingUnit = product.StockKeepingUnit,
            Description = product.Description,
            ImageURL = product.ImageURL,
            Price = product.Price,
        };

        return Ok(response);
    }

    /// <summary>
    /// Add new product
    /// </summary>
    /// <param name="request">Product</param>
    /// <returns>New product</returns>
    [Authorize(Roles = "Administrator")]
    [HttpPost]
    [Consumes("application/json")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public ActionResult<ProductDto> CreateProduct(CreateProductRequestDto request)
    {
        try
        {
            var product = new Product
            {
                Name = request.Name,
                StockKeepingUnit = request.StockKeepingUnit,
                Description = request.Description,
                ImageURL = request.ImageURL,
                Price = request.Price,
            };

            context.Products.Add(product);
            context.SaveChanges();

            var productDto = new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                StockKeepingUnit = product.StockKeepingUnit,
                Description = product.Description,
                ImageURL = product.ImageURL,
                Price = product.Price,
            };

            return Created("", productDto);

        } catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


    /// <summary>
    /// Delete product
    /// </summary>
    /// <param name="stockKeepingUnit">SKU</param>
    [Authorize(Roles = "Administrator")]
    [HttpDelete("{stockKeepingUnit}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult DeleteProduct(string stockKeepingUnit)
    {
        var product = context.Products.FirstOrDefault(x => x.StockKeepingUnit == stockKeepingUnit);

        if (product == null)
        {
            return NotFound();
        }

        context.Products.Remove(product);
        context.SaveChanges();

        return NoContent();
    }

    /// <summary>
    /// Update product
    /// </summary>
    /// <param name="stockKeepingUnit">SKU</param>
    /// <param name="request">Information about product</param>
    /// <returns></returns>
    [Authorize(Roles = "Administrator")]
    [HttpPut("{stockKeepingUnit}")]
    [Consumes("application/json")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult UpdateProduct(string stockKeepingUnit, UpdateProductRequest request)
    {
        if(request.StockKeepingUnit.Equals(stockKeepingUnit))
        {
            return BadRequest("SKU does not match");
        }

        var product = context.Products.FirstOrDefault(x => x.StockKeepingUnit == stockKeepingUnit);

        if (product is null)
        {
            return NotFound();
        }

        try
        {
            product.Name = request.Name;
            product.StockKeepingUnit = stockKeepingUnit;
            product.Description = request.Description;
            product.ImageURL = request.ImageURL;
            product.Price = request.Price;

            context.SaveChanges();
            return NoContent();

        } catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
