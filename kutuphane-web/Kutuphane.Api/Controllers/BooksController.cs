using Kutuphane.Api.Models;
using Kutuphane.Api.Repositories;
using Kutuphane.Shared.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kutuphane.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IBookRepository _books;
    private readonly ICategoryRepository _categories;

    public BooksController(IBookRepository books, ICategoryRepository categories)
    {
        _books = books;
        _categories = categories;
    }

    // GET: api/books
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookDto>>> GetAll()
    {
        var books = await _books.GetAllWithDetailsAsync();
        return Ok(books.Select(ToDto));
    }

    // GET: api/books/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<BookDto>> Get(int id)
    {
        var book = await _books.GetByIdWithDetailsAsync(id);
        if (book is null)
            return NotFound();

        return Ok(ToDto(book));
    }

    // POST: api/books
    [HttpPost]
    public async Task<ActionResult<BookDto>> Create(BookInputDto input)
    {
        if (await _categories.GetByIdAsync(input.CategoryId) is null)
            return BadRequest($"Category {input.CategoryId} does not exist.");

        var book = new Book
        {
            Title = input.Title,
            Author = input.Author,
            Isbn = input.Isbn,
            PublishedYear = input.PublishedYear,
            TotalCopies = input.TotalCopies,
            CategoryId = input.CategoryId
        };
        await _books.AddAsync(book);

        var created = await _books.GetByIdWithDetailsAsync(book.Id);
        return CreatedAtAction(nameof(Get), new { id = book.Id }, ToDto(created!));
    }

    // PUT: api/books/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, BookInputDto input)
    {
        var book = await _books.GetByIdAsync(id);
        if (book is null)
            return NotFound();

        if (await _categories.GetByIdAsync(input.CategoryId) is null)
            return BadRequest($"Category {input.CategoryId} does not exist.");

        book.Title = input.Title;
        book.Author = input.Author;
        book.Isbn = input.Isbn;
        book.PublishedYear = input.PublishedYear;
        book.TotalCopies = input.TotalCopies;
        book.CategoryId = input.CategoryId;
        await _books.UpdateAsync(book);
        return NoContent();
    }

    // DELETE: api/books/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var deleted = await _books.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }
        catch (DbUpdateException)
        {
            // The book still has loan history (foreign key restriction).
            return Conflict("This book has loan records and cannot be deleted.");
        }
    }

    private static BookDto ToDto(Book b)
    {
        var onLoan = b.Loans?.Count(l => l.ReturnDate == null) ?? 0;
        return new BookDto
        {
            Id = b.Id,
            Title = b.Title,
            Author = b.Author,
            Isbn = b.Isbn,
            PublishedYear = b.PublishedYear,
            TotalCopies = b.TotalCopies,
            CategoryId = b.CategoryId,
            CategoryName = b.Category?.Name ?? string.Empty,
            AvailableCopies = b.TotalCopies - onLoan
        };
    }
}
