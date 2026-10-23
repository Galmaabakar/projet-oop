using System.ComponentModel.DataAnnotations;

namespace Kutuphane.Api.Models;

/// <summary>
/// A loan links one book to one member for a period of time.
/// <see cref="ReturnDate"/> is null while the book is still out.
/// </summary>
public class Loan
{
    public int Id { get; set; }

    // Foreign key -> Book
    public int BookId { get; set; }
    public Book? Book { get; set; }

    // Foreign key -> Member
    public int MemberId { get; set; }
    public Member? Member { get; set; }

    // All dates are UTC (PostgreSQL timestamptz).
    public DateTime LoanDate { get; set; } = DateTime.UtcNow;
    public DateTime DueDate { get; set; }

    // null => the book has not been returned yet.
    public DateTime? ReturnDate { get; set; }
}
