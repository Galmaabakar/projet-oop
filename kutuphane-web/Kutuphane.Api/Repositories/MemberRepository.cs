using Kutuphane.Api.Data;
using Kutuphane.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Kutuphane.Api.Repositories;

public interface IMemberRepository : IRepository<Member>
{
    /// <summary>All members, with their loans loaded (to count active ones).</summary>
    Task<List<Member>> GetAllWithLoansAsync();
}

public class MemberRepository : Repository<Member>, IMemberRepository
{
    public MemberRepository(KutuphaneContext context) : base(context)
    {
    }

    public async Task<List<Member>> GetAllWithLoansAsync()
        => await DbSet
            .Include(m => m.Loans)
            .OrderBy(m => m.FullName)
            .ToListAsync();
}
