using Kutuphane.Shared.Dtos;
using Kutuphane.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Kutuphane.Web.Controllers;

public class BooksController : Controller
{
    private readonly LibraryApiClient _api;

    public BooksController(LibraryApiClient api) => _api = api;

    // GET: /Books
    public async Task<IActionResult> Index()
    {
        var books = await _api.GetBooksAsync();
        return View(books);
    }

    // GET: /Books/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var book = await _api.GetBookAsync(id);
        if (book is null) return NotFound();
        return View(book);
    }

    // GET: /Books/Create
    public async Task<IActionResult> Create()
    {
        await PopulateCategoriesAsync();
        return View(new BookInputDto());
    }

    // POST: /Books/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(BookInputDto input)
    {
        if (!ModelState.IsValid)
        {
            await PopulateCategoriesAsync(input.CategoryId);
            return View(input);
        }

        var result = await _api.CreateBookAsync(input);
        if (!result.Success)
        {
            ModelState.AddModelError(string.Empty, result.Error ?? "Kayıt eklenemedi.");
            await PopulateCategoriesAsync(input.CategoryId);
            return View(input);
        }

        TempData["Success"] = "Kitap eklendi.";
        return RedirectToAction(nameof(Index));
    }

    // GET: /Books/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var book = await _api.GetBookAsync(id);
        if (book is null) return NotFound();

        await PopulateCategoriesAsync(book.CategoryId);
        ViewBag.BookId = id;
        return View(ToInput(book));
    }

    // POST: /Books/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, BookInputDto input)
    {
        if (!ModelState.IsValid)
        {
            await PopulateCategoriesAsync(input.CategoryId);
            ViewBag.BookId = id;
            return View(input);
        }

        var result = await _api.UpdateBookAsync(id, input);
        if (!result.Success)
        {
            ModelState.AddModelError(string.Empty, result.Error ?? "Güncelleme başarısız.");
            await PopulateCategoriesAsync(input.CategoryId);
            ViewBag.BookId = id;
            return View(input);
        }

        TempData["Success"] = "Kitap güncellendi.";
        return RedirectToAction(nameof(Index));
    }

    // GET: /Books/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var book = await _api.GetBookAsync(id);
        if (book is null) return NotFound();
        return View(book);
    }

    // POST: /Books/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var result = await _api.DeleteBookAsync(id);
        if (!result.Success)
            TempData["Error"] = result.Error ?? "Kitap silinemedi.";
        else
            TempData["Success"] = "Kitap silindi.";

        return RedirectToAction(nameof(Index));
    }

    private async Task PopulateCategoriesAsync(int? selectedId = null)
    {
        var categories = await _api.GetCategoriesAsync();
        ViewBag.Categories = new SelectList(categories, nameof(CategoryDto.Id), nameof(CategoryDto.Name), selectedId);
    }

    private static BookInputDto ToInput(BookDto b) => new()
    {
        Title = b.Title,
        Author = b.Author,
        Isbn = b.Isbn,
        PublishedYear = b.PublishedYear,
        TotalCopies = b.TotalCopies,
        CategoryId = b.CategoryId
    };
}
