using System.Net.Http.Json;
using Kutuphane.Shared.Dtos;

namespace Kutuphane.Web.Services;

/// <summary>Result of a write operation against the API.</summary>
public record ApiResult(bool Success, string? Error = null)
{
    public static readonly ApiResult Ok = new(true);
}

/// <summary>
/// Typed HttpClient that talks to the Kutuphane REST API.
/// This is the "consume a Web API with HttpClient" part of the course (week 15):
/// the MVC site never touches the database directly, it calls the API.
/// </summary>
public class LibraryApiClient
{
    private readonly HttpClient _http;

    public LibraryApiClient(HttpClient http) => _http = http;

    // ---- Categories ------------------------------------------------------
    public async Task<List<CategoryDto>> GetCategoriesAsync()
        => await _http.GetFromJsonAsync<List<CategoryDto>>("api/categories") ?? new();

    public async Task<CategoryDto?> GetCategoryAsync(int id)
        => await _http.GetFromJsonAsync<CategoryDto>($"api/categories/{id}");

    public async Task<ApiResult> CreateCategoryAsync(CategoryInputDto input)
        => await SendAsync(() => _http.PostAsJsonAsync("api/categories", input));

    public async Task<ApiResult> UpdateCategoryAsync(int id, CategoryInputDto input)
        => await SendAsync(() => _http.PutAsJsonAsync($"api/categories/{id}", input));

    public async Task<ApiResult> DeleteCategoryAsync(int id)
        => await SendAsync(() => _http.DeleteAsync($"api/categories/{id}"));

    // ---- Books -----------------------------------------------------------
    public async Task<List<BookDto>> GetBooksAsync()
        => await _http.GetFromJsonAsync<List<BookDto>>("api/books") ?? new();

    public async Task<BookDto?> GetBookAsync(int id)
        => await _http.GetFromJsonAsync<BookDto>($"api/books/{id}");

    public async Task<ApiResult> CreateBookAsync(BookInputDto input)
        => await SendAsync(() => _http.PostAsJsonAsync("api/books", input));

    public async Task<ApiResult> UpdateBookAsync(int id, BookInputDto input)
        => await SendAsync(() => _http.PutAsJsonAsync($"api/books/{id}", input));

    public async Task<ApiResult> DeleteBookAsync(int id)
        => await SendAsync(() => _http.DeleteAsync($"api/books/{id}"));

    // ---- Members ---------------------------------------------------------
    public async Task<List<MemberDto>> GetMembersAsync()
        => await _http.GetFromJsonAsync<List<MemberDto>>("api/members") ?? new();

    public async Task<MemberDto?> GetMemberAsync(int id)
        => await _http.GetFromJsonAsync<MemberDto>($"api/members/{id}");

    public async Task<ApiResult> CreateMemberAsync(MemberInputDto input)
        => await SendAsync(() => _http.PostAsJsonAsync("api/members", input));

    public async Task<ApiResult> UpdateMemberAsync(int id, MemberInputDto input)
        => await SendAsync(() => _http.PutAsJsonAsync($"api/members/{id}", input));

    public async Task<ApiResult> DeleteMemberAsync(int id)
        => await SendAsync(() => _http.DeleteAsync($"api/members/{id}"));

    // ---- Loans -----------------------------------------------------------
    public async Task<List<LoanDto>> GetLoansAsync()
        => await _http.GetFromJsonAsync<List<LoanDto>>("api/loans") ?? new();

    public async Task<ApiResult> BorrowAsync(LoanInputDto input)
        => await SendAsync(() => _http.PostAsJsonAsync("api/loans", input));

    public async Task<ApiResult> ReturnAsync(int loanId)
        => await SendAsync(() => _http.PostAsync($"api/loans/{loanId}/return", null));

    // ---- Helper ----------------------------------------------------------
    // Runs a write request and turns the HTTP response into an ApiResult,
    // reading the error body when the API rejects the request.
    private static async Task<ApiResult> SendAsync(Func<Task<HttpResponseMessage>> send)
    {
        var response = await send();
        if (response.IsSuccessStatusCode)
            return ApiResult.Ok;

        var body = await response.Content.ReadAsStringAsync();
        var message = string.IsNullOrWhiteSpace(body)
            ? $"İşlem başarısız (HTTP {(int)response.StatusCode})."
            : body;
        return new ApiResult(false, message);
    }
}
