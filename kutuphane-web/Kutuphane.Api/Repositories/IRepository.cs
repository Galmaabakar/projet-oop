namespace Kutuphane.Api.Repositories;

/// <summary>
/// Generic repository contract: the basic CRUD operations shared by
/// every entity. Specialized repositories can add their own methods.
/// </summary>
public interface IRepository<T> where T : class
{
    Task<List<T>> GetAllAsync();
    Task<T?> GetByIdAsync(int id);
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task<bool> DeleteAsync(int id);
}
