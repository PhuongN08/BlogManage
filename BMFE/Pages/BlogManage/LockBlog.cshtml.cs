using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Net.Http.Headers;

namespace BMFE.Pages.BlogManage
{
    public class LockBlogModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public LockBlogModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("AuthApi");
        }

        [BindProperty]
        public int BlogId { get; set; }

        [BindProperty]
        public int NewStatus { get; set; } = 4; // default to lock

        public string? Message { get; set; }
        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var token = HttpContext.Session.GetString("Token");
            if (string.IsNullOrEmpty(token))
                return RedirectToPage("/BlogManage/Login");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            try
            {
                var response = await _httpClient.PutAsync($"api/BlogManage/status/{BlogId}?status={NewStatus}", null);
                if (response.IsSuccessStatusCode)
                {
                    Message = NewStatus == 4
                        ? "✅ Blog đã được khóa thành công."
                        : "✅ Blog đã được mở (Active) thành công.";
                }
                else
                {
                    var errorDetail = await response.Content.ReadAsStringAsync();
                    ErrorMessage = $"Lỗi từ server: {errorDetail}";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Lỗi khi gọi API: {ex.Message}";
            }

            return Page();
        }
    }
}
