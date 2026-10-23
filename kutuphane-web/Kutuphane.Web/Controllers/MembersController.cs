using Kutuphane.Shared.Dtos;
using Kutuphane.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace Kutuphane.Web.Controllers;

public class MembersController : Controller
{
    private readonly LibraryApiClient _api;

    public MembersController(LibraryApiClient api) => _api = api;

    public async Task<IActionResult> Index()
        => View(await _api.GetMembersAsync());

    public IActionResult Create() => View(new MemberInputDto());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(MemberInputDto input)
    {
        if (!ModelState.IsValid) return View(input);

        var result = await _api.CreateMemberAsync(input);
        if (!result.Success)
        {
            ModelState.AddModelError(string.Empty, result.Error ?? "Üye eklenemedi.");
            return View(input);
        }

        TempData["Success"] = "Üye eklendi.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var member = await _api.GetMemberAsync(id);
        if (member is null) return NotFound();

        ViewBag.MemberId = id;
        return View(new MemberInputDto
        {
            FullName = member.FullName,
            Email = member.Email,
            Phone = member.Phone
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, MemberInputDto input)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.MemberId = id;
            return View(input);
        }

        var result = await _api.UpdateMemberAsync(id, input);
        if (!result.Success)
        {
            ModelState.AddModelError(string.Empty, result.Error ?? "Güncelleme başarısız.");
            ViewBag.MemberId = id;
            return View(input);
        }

        TempData["Success"] = "Üye güncellendi.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var member = await _api.GetMemberAsync(id);
        if (member is null) return NotFound();
        return View(member);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var result = await _api.DeleteMemberAsync(id);
        if (!result.Success)
            TempData["Error"] = result.Error ?? "Üye silinemedi.";
        else
            TempData["Success"] = "Üye silindi.";

        return RedirectToAction(nameof(Index));
    }
}
