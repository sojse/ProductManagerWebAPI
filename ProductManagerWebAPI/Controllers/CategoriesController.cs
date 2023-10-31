using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductManagerWebAPI.Data;
using ProductManagerWebAPI.Domain;
using ProductManagerWebAPI.DTO;

namespace ProductManagerWebAPI.Controllers;

[Authorize]
[Route("[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{

    private readonly ApplicationDbContext context;

    public CategoriesController(ApplicationDbContext context)
    {
        this.context = context;
    }

    /// <summary>
    /// Get all categories and their products
    /// </summary>
    /// <returns>Array of products</returns>
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<List<CategoryDto>> GetCategories()
    {
        IEnumerable<Category> categories = context.Category.Include(c => c.Products).ToList();

        var response = categories.Select(category => new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Products = category.Products.Select(product => new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                StockKeepingUnit = product.StockKeepingUnit,
                Description = product.Description,
                ImageURL = product.ImageURL,
                Price = product.Price,
            }).ToList()
        }).ToList();

        return Ok(response);
    }

    /// <summary>
    /// Add new category
    /// </summary>
    /// <param name="request">Category</param>
    /// <returns>New category</returns>
    [Authorize(Roles = "Administrator")]
    [HttpPost]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public ActionResult<CategoryDto> CreateCategory(CreateCategoryDto request)
    {
        try
        {
            var category = new Category
            {
                Name = request.Name,
            };

            context.Category.Add(category);
            context.SaveChanges();

            var categoryDto = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
            };

            return Created("", categoryDto);

        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // POST för att skapa en koppling mellan produkt och kategori POST /categories/{id}/products
    /// <summary>
    /// Add new product to category
    /// </summary>
    /// <param name="request">Category</param>
    /// <returns>New category</returns>
    [Authorize(Roles = "Administrator")]
    [HttpPost("{id}/products")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public ActionResult<CategoryDto> AddProductToCategory(int id, [FromBody] ProductDto request)
    {

        try
        {

            var category = context.Category.FirstOrDefault(category => category.Id == id);

            var product = context.Products.FirstOrDefault(product => product.Id == request.Id);

            category.Products.Add(product);

            context.SaveChanges();

            return Created("", null);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }

}
