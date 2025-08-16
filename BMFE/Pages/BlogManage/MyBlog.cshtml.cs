using BlogManage.ViewModel.PublicVM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace BMFE.Pages.BlogManage
{
    public class MyBlogModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public MyBlogModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("AuthApi");
        }

        public List<BlogVM> Blogs { get; set; } = new();

        [BindProperty(SupportsGet = true)] public string? SearchTerm { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var token = HttpContext.Session.GetString("Token");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToPage("/BlogManage/Login");
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var query = "api/WriterBlog/getAll";
            if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                query += $"?searchTerm={SearchTerm}";
            }

            try
            {
                var response = await _httpClient.GetAsync(query);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    Blogs = JsonSerializer.Deserialize<List<BlogVM>>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }) ?? new List<BlogVM>();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Không thể tải dữ liệu.");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Lỗi: {ex.Message}");
            }

            return Page();
        }
    }
}
