using Kutuphane.Api.Models;
using Kutuphane.Api.Repositories;
using Kutuphane.Shared.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kutuphane.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MembersController : ControllerBase
{
    private readonly IMemberRepository _repo;

    public MembersController(IMemberRepository repo) => _repo = repo;

    // GET: api/members
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberDto>>> GetAll()
    {
        var members = await _repo.GetAllWithLoansAsync();
        return Ok(members.Select(ToDto));
    }

    // GET: api/members/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<MemberDto>> Get(int id)
    {
        var member = await _repo.GetByIdAsync(id);
        if (member is null)
            return NotFound();

        return Ok(ToDto(member));
    }

    // POST: api/members
    [HttpPost]
    public async Task<ActionResult<MemberDto>> Create(MemberInputDto input)
    {
        var member = new Member
        {
            FullName = input.FullName,
            Email = input.Email,
            Phone = input.Phone,
            RegisteredDate = DateTime.UtcNow
        };
        await _repo.AddAsync(member);
        return CreatedAtAction(nameof(Get), new { id = member.Id }, ToDto(member));
    }

    // PUT: api/members/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, MemberInputDto input)
    {
        var member = await _repo.GetByIdAsync(id);
        if (member is null)
            return NotFound();

        member.FullName = input.FullName;
        member.Email = input.Email;
        member.Phone = input.Phone;
        await _repo.UpdateAsync(member);
        return NoContent();
    }

    // DELETE: api/members/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var deleted = await _repo.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }
        catch (DbUpdateException)
        {
            return Conflict("This member could not be deleted.");
        }
    }

    private static MemberDto ToDto(Member m) => new()
    {
        Id = m.Id,
        FullName = m.FullName,
        Email = m.Email,
        Phone = m.Phone,
        RegisteredDate = m.RegisteredDate,
        ActiveLoanCount = m.Loans?.Count(l => l.ReturnDate == null) ?? 0
    };
}
