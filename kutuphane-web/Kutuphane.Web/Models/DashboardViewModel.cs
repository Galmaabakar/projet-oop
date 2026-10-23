using Kutuphane.Shared.Dtos;

namespace Kutuphane.Web.Models;

/// <summary>Numbers and highlights shown on the home page.</summary>
public class DashboardViewModel
{
    public int BookCount { get; set; }
    public int CategoryCount { get; set; }
    public int MemberCount { get; set; }
    public int ActiveLoanCount { get; set; }

    public List<BookDto> Books { get; set; } = new();

    // Set when the API cannot be reached, so the view can warn the user.
    public bool ApiUnavailable { get; set; }
}
