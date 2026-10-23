using System.ComponentModel.DataAnnotations;

namespace Kutuphane.Shared.Dtos;

/// <summary>Member data returned by the API.</summary>
public class MemberDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public DateTime RegisteredDate { get; set; }
    public int ActiveLoanCount { get; set; }
}

/// <summary>Payload used to create or update a member.</summary>
public class MemberInputDto
{
    [Required]
    [MaxLength(150)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(150)]
    public string Email { get; set; } = string.Empty;

    [MaxLength(30)]
    public string? Phone { get; set; }
}
