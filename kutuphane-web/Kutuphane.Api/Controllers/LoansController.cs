using Kutuphane.Api.Models;
using Kutuphane.Api.Repositories;
using Kutuphane.Shared.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Kutuphane.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LoansController : ControllerBase
{
    private readonly ILoanRepository _loans;

    public LoansController(ILoanRepository loans) => _loans = loans;

    // GET: api/loans
    [HttpGet]
    public async Task<ActionResult<IEnumerable<LoanDto>>> GetAll()
    {
        var loans = await _loans.GetAllWithDetailsAsync();
        return Ok(loans.Select(ToDto));
    }

    // GET: api/loans/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<LoanDto>> Get(int id)
    {
        var loan = await _loans.GetByIdWithDetailsAsync(id);
        if (loan is null)
            return NotFound();

        return Ok(ToDto(loan));
    }

    // POST: api/loans  -> lend a book to a member
    [HttpPost]
    public async Task<ActionResult<LoanDto>> Borrow(LoanInputDto input)
    {
        var loan = await _loans.BorrowAsync(input.BookId, input.MemberId, input.LoanDays);
        if (loan is null)
            return BadRequest("No copy available, or the book/member does not exist.");

        var created = await _loans.GetByIdWithDetailsAsync(loan.Id);
        return CreatedAtAction(nameof(Get), new { id = loan.Id }, ToDto(created!));
    }

    // POST: api/loans/5/return  -> mark a loan as returned
    [HttpPost("{id:int}/return")]
    public async Task<IActionResult> Return(int id)
    {
        var ok = await _loans.ReturnAsync(id);
        return ok ? NoContent() : BadRequest("Loan not found or already returned.");
    }

    // DELETE: api/loans/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _loans.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }

    private static LoanDto ToDto(Loan l) => new()
    {
        Id = l.Id,
        BookId = l.BookId,
        BookTitle = l.Book?.Title ?? string.Empty,
        MemberId = l.MemberId,
        MemberName = l.Member?.FullName ?? string.Empty,
        LoanDate = l.LoanDate,
        DueDate = l.DueDate,
        ReturnDate = l.ReturnDate
    };
}
