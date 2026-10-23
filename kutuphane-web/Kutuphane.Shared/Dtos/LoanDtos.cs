using System.ComponentModel.DataAnnotations;

namespace Kutuphane.Shared.Dtos;

/// <summary>Loan data returned by the API (with book and member names).</summary>
public class LoanDto
{
    public int Id { get; set; }

    public int BookId { get; set; }
    public string BookTitle { get; set; } = string.Empty;

    public int MemberId { get; set; }
    public string MemberName { get; set; } = string.Empty;

    public DateTime LoanDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? ReturnDate { get; set; }

    // Convenience flag: true while the book has not been returned.
    public bool IsReturned => ReturnDate != null;
}

/// <summary>Payload used to lend a book to a member.</summary>
public class LoanInputDto
{
    [Required]
    public int BookId { get; set; }

    [Required]
    public int MemberId { get; set; }

    // How many days the member can keep the book (default: 14).
    [Range(1, 180)]
    public int LoanDays { get; set; } = 14;
}
