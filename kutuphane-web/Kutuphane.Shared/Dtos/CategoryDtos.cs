using System.ComponentModel.DataAnnotations;

namespace Kutuphane.Shared.Dtos;

/// <summary>Category data returned by the API.</summary>
public class CategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int BookCount { get; set; }
}

/// <summary>Payload used to create or update a category.</summary>
public class CategoryInputDto
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
}
