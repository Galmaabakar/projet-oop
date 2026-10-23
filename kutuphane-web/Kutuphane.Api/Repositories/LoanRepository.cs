using Kutuphane.Api.Data;
using Kutuphane.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Kutuphane.Api.Repositories;

public interface ILoanRepository : IRepository<Loan>
{
    Task<List<Loan>> GetAllWithDetailsAsync();
    Task<Loan?> GetByIdWithDetailsAsync(int id);

    /// <summary>Number of copies of a book that are currently out on loan.</summary>
    Task<int> GetOnLoanCountAsync(int bookId);

    /// <summary>Lend a book to a member. Returns null if no copy is available.</summary>
    Task<Loan?> BorrowAsync(int bookId, int memberId, int loanDays);

    /// <summary>Mark a loan as returned. Returns false if already returned/not found.</summary>
    Task<bool> ReturnAsync(int loanId);
}

/// <summary>
/// Loan-specific repository. Handles the borrow/return business rules
/// (a book can only be lent while a free copy exists).
/// </summary>
public class LoanRepository : Repository<Loan>, ILoanRepository
{
    public LoanRepository(KutuphaneContext context) : base(context)
    {
    }

    public async Task<List<Loan>> GetAllWithDetailsAsync()
        => await DbSet
            .Include(l => l.Book)
            .Include(l => l.Member)
            .OrderByDescending(l => l.LoanDate)
            .ToListAsync();

    public async Task<Loan?> GetByIdWithDetailsAsync(int id)
        => await DbSet
            .Include(l => l.Book)
            .Include(l => l.Member)
            .FirstOrDefaultAsync(l => l.Id == id);

    public async Task<int> GetOnLoanCountAsync(int bookId)
        => await DbSet.CountAsync(l => l.BookId == bookId && l.ReturnDate == null);

    public async Task<Loan?> BorrowAsync(int bookId, int memberId, int loanDays)
    {
        var book = await Context.Books.FindAsync(bookId);
        var member = await Context.Members.FindAsync(memberId);
        if (book is null || member is null)
            return null;

        // Business rule: only lend if at least one copy is free.
        var onLoan = await GetOnLoanCountAsync(bookId);
        if (onLoan >= book.TotalCopies)
            return null;

        var now = DateTime.UtcNow;
        var loan = new Loan
        {
            BookId = bookId,
            MemberId = memberId,
            LoanDate = now,
            DueDate = now.AddDays(loanDays),
            ReturnDate = null
        };

        DbSet.Add(loan);
        await Context.SaveChangesAsync();
        return loan;
    }

    public async Task<bool> ReturnAsync(int loanId)
    {
        var loan = await DbSet.FindAsync(loanId);
        if (loan is null || loan.ReturnDate != null)
            return false;

        loan.ReturnDate = DateTime.UtcNow;
        await Context.SaveChangesAsync();
        return true;
    }
}
