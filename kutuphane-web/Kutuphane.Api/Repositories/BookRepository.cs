using Kutuphane.Api.Data;
using Kutuphane.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Kutuphane.Api.Repositories;

public interface IBookRepository : IRepository<Book>
{
    /// <summary>All books, with their category and loans loaded.</summary>
    Task<List<Book>> GetAllWithDetailsAsync();

    /// <summary>A single book, with its category and loans loaded.</summary>
    Task<Book?> GetByIdWithDetailsAsync(int id);
}

/// <summary>
/// Book-specific repository. Adds eager loading of the related Category
/// and Loans so the API can compute "available copies".
/// </summary>
public class BookRepository : Repository<Book>, IBookRepository
{
    public BookRepository(KutuphaneContext context) : base(context)
    {
    }

    public async Task<List<Book>> GetAllWithDetailsAsync()
        => await DbSet
            .Include(b => b.Category)
            .Include(b => b.Loans)
            .OrderBy(b => b.Title)
            .ToListAsync();

    public async Task<Book?> GetByIdWithDetailsAsync(int id)
        => await DbSet
            .Include(b => b.Category)
            .Include(b => b.Loans)
            .FirstOrDefaultAsync(b => b.Id == id);
}
