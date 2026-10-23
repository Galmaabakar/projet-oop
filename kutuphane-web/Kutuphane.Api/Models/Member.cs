using System.ComponentModel.DataAnnotations;

namespace Kutuphane.Api.Models;

/// <summary>
/// A library member who can borrow books.
/// </summary>
public class Member
{
    public int Id { get; set; }

    [Required]
    [MaxLength(150)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(150)]
    public string Email { get; set; } = string.Empty;

    [MaxLength(30)]
    public string? Phone { get; set; }

    // Stored in UTC (required by PostgreSQL/Npgsql for timestamptz columns).
    public DateTime RegisteredDate { get; set; } = DateTime.UtcNow;

    // Navigation: every loan made by this member.
    public ICollection<Loan> Loans { get; set; } = new List<Loan>();
}
