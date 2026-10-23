using System.ComponentModel.DataAnnotations;

namespace Kutuphane.Api.Models;

/// <summary>
/// A book title owned by the library. The library may own several
/// physical copies of the same title (see <see cref="TotalCopies"/>).
/// </summary>
public class Book
{
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(150)]
    public string Author { get; set; } = string.Empty;

    [MaxLength(20)]
    public string? Isbn { get; set; }

    public int PublishedYear { get; set; }

    // How many physical copies the library owns in total.
    public int TotalCopies { get; set; }

    // Foreign key -> Category (a book belongs to exactly one category).
    public int CategoryId { get; set; }
    public Category? Category { get; set; }

    // Navigation: every loan ever made for this book.
    public ICollection<Loan> Loans { get; set; } = new List<Loan>();
}
