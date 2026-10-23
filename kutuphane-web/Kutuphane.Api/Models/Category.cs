using System.ComponentModel.DataAnnotations;

namespace Kutuphane.Api.Models;

/// <summary>
/// A book category (e.g. Novel, Science, History).
/// </summary>
public class Category
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    // Navigation: one category contains many books.
    public ICollection<Book> Books { get; set; } = new List<Book>();
}
