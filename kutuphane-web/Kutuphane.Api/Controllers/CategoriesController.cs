using Kutuphane.Api.Models;
using Kutuphane.Api.Repositories;
using Kutuphane.Shared.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kutuphane.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryRepository _repo;

    public CategoriesController(ICategoryRepository repo) => _repo = repo;

    // GET: api/categories
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAll()
    {
        var categories = await _repo.GetAllWithBooksAsync();
        return Ok(categories.Select(ToDto));
    }

    // GET: api/categories/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<CategoryDto>> Get(int id)
    {
        var category = await _repo.GetByIdAsync(id);
        if (category is null)
            return NotFound();

        return Ok(ToDto(category));
    }

    // POST: api/categories
    [HttpPost]
    public async Task<ActionResult<CategoryDto>> Create(CategoryInputDto input)
    {
        var category = new Category { Name = input.Name };
        await _repo.AddAsync(category);
        return CreatedAtAction(nameof(Get), new { id = category.Id }, ToDto(category));
    }

    // PUT: api/categories/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, CategoryInputDto input)
    {
        var category = await _repo.GetByIdAsync(id);
        if (category is null)
            return NotFound();

        category.Name = input.Name;
        await _repo.UpdateAsync(category);
        return NoContent();
    }

    // DELETE: api/categories/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var deleted = await _repo.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }
        catch (DbUpdateException)
        {
            // The category still has books (foreign key restriction).
            return Conflict("This category still contains books and cannot be deleted.");
        }
    }

    private static CategoryDto ToDto(Category c) => new()
    {
        Id = c.Id,
        Name = c.Name,
        BookCount = c.Books?.Count ?? 0
    };
}
