using Kutuphane.Shared.Dtos;
using Kutuphane.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Kutuphane.Web.Controllers;

public class LoansController : Controller
{
    private readonly LibraryApiClient _api;

    public LoansController(LibraryApiClient api) => _api = api;

    // GET: /Loans
    public async Task<IActionResult> Index()
        => View(await _api.GetLoansAsync());

    // GET: /Loans/Borrow
    public async Task<IActionResult> Borrow()
    {
        await PopulateDropdownsAsync();
        return View(new LoanInputDto());
    }

    // POST: /Loans/Borrow
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Borrow(LoanInputDto input)
    {
        if (!ModelState.IsValid)
        {
            await PopulateDropdownsAsync(input.BookId, input.MemberId);
            return View(input);
        }

        var result = await _api.BorrowAsync(input);
        if (!result.Success)
        {
            ModelState.AddModelError(string.Empty, result.Error ?? "Ödünç verme başarısız.");
            await PopulateDropdownsAsync(input.BookId, input.MemberId);
            return View(input);
        }

        TempData["Success"] = "Kitap ödünç verildi.";
        return RedirectToAction(nameof(Index));
    }

    // POST: /Loans/Return/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Return(int id)
    {
        var result = await _api.ReturnAsync(id);
        if (!result.Success)
            TempData["Error"] = result.Error ?? "İade başarısız.";
        else
            TempData["Success"] = "Kitap iade edildi.";

        return RedirectToAction(nameof(Index));
    }

    private async Task PopulateDropdownsAsync(int? selectedBook = null, int? selectedMember = null)
    {
        var books = await _api.GetBooksAsync();
        var members = await _api.GetMembersAsync();

        // Only books that still have a free copy can be lent.
        var available = books
            .Where(b => b.AvailableCopies > 0)
            .Select(b => new { b.Id, Label = $"{b.Title} ({b.AvailableCopies} adet)" });

        ViewBag.Books = new SelectList(available, "Id", "Label", selectedBook);
        ViewBag.Members = new SelectList(members, nameof(MemberDto.Id), nameof(MemberDto.FullName), selectedMember);
    }
}
