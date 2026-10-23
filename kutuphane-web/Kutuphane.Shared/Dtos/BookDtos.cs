using System.ComponentModel.DataAnnotations;

namespace Kutuphane.Shared.Dtos;

/// <summary>Book data returned by the API (includes computed fields).</summary>
public class BookDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string? Isbn { get; set; }
    public int PublishedYear { get; set; }
    public int TotalCopies { get; set; }

    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;

    // TotalCopies minus the number of copies currently on loan.
    public int AvailableCopies { get; set; }
}

/// <summary>Payload used to create or update a book.</summary>
public class BookInputDto
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(150)]
    public string Author { get; set; } = string.Empty;

    [MaxLength(20)]
    public string? Isbn { get; set; }

    [Range(0, 2100)]
    public int PublishedYear { get; set; }

    [Range(1, 1000)]
    public int TotalCopies { get; set; } = 1;

    [Required]
    public int CategoryId { get; set; }
}
