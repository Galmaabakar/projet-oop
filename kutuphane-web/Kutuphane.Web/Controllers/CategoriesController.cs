using Kutuphane.Shared.Dtos;
using Kutuphane.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace Kutuphane.Web.Controllers;

public class CategoriesController : Controller
{
    private readonly LibraryApiClient _api;

    public CategoriesController(LibraryApiClient api) => _api = api;

    public async Task<IActionResult> Index()
        => View(await _api.GetCategoriesAsync());

    public IActionResult Create() => View(new CategoryInputDto());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CategoryInputDto input)
    {
        if (!ModelState.IsValid) return View(input);

        var result = await _api.CreateCategoryAsync(input);
        if (!result.Success)
        {
            ModelState.AddModelError(string.Empty, result.Error ?? "Kategori eklenemedi.");
            return View(input);
        }

        TempData["Success"] = "Kategori eklendi.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var category = await _api.GetCategoryAsync(id);
        if (category is null) return NotFound();

        ViewBag.CategoryId = id;
        return View(new CategoryInputDto { Name = category.Name });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, CategoryInputDto input)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.CategoryId = id;
            return View(input);
        }

        var result = await _api.UpdateCategoryAsync(id, input);
        if (!result.Success)
        {
            ModelState.AddModelError(string.Empty, result.Error ?? "Güncelleme başarısız.");
            ViewBag.CategoryId = id;
            return View(input);
        }

        TempData["Success"] = "Kategori güncellendi.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var category = await _api.GetCategoryAsync(id);
        if (category is null) return NotFound();
        return View(category);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var result = await _api.DeleteCategoryAsync(id);
        if (!result.Success)
            TempData["Error"] = result.Error ?? "Kategori silinemedi.";
        else
            TempData["Success"] = "Kategori silindi.";

        return RedirectToAction(nameof(Index));
    }
}
