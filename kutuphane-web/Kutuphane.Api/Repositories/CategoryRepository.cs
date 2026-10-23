using Kutuphane.Api.Data;
using Kutuphane.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Kutuphane.Api.Repositories;

public interface ICategoryRepository : IRepository<Category>
{
    /// <summary>All categories, with their books loaded (to count them).</summary>
    Task<List<Category>> GetAllWithBooksAsync();
}

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    public CategoryRepository(KutuphaneContext context) : base(context)
    {
    }

    public async Task<List<Category>> GetAllWithBooksAsync()
        => await DbSet
            .Include(c => c.Books)
            .OrderBy(c => c.Name)
            .ToListAsync();
}
