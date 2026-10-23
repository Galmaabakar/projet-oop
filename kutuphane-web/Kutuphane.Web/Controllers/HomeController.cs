using System.Diagnostics;
using Kutuphane.Web.Models;
using Kutuphane.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace Kutuphane.Web.Controllers;

public class HomeController : Controller
{
    private readonly LibraryApiClient _api;
    private readonly ILogger<HomeController> _logger;

    public HomeController(LibraryApiClient api, ILogger<HomeController> logger)
    {
        _api = api;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var model = new DashboardViewModel();
        try
        {
            var books = await _api.GetBooksAsync();
            var categories = await _api.GetCategoriesAsync();
            var members = await _api.GetMembersAsync();
            var loans = await _api.GetLoansAsync();

            model.Books = books;
            model.BookCount = books.Count;
            model.CategoryCount = categories.Count;
            model.MemberCount = members.Count;
            model.ActiveLoanCount = loans.Count(l => !l.IsReturned);
        }
        catch (HttpRequestException ex)
        {
            // The API project is probably not running.
            _logger.LogError(ex, "Could not reach the API.");
            model.ApiUnavailable = true;
        }

        return View(model);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
