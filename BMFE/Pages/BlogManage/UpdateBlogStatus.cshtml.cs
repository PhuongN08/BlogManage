using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Text.Json;

namespace BMFE.Pages.BlogManage
{
    public class UpdateBlogStatusModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public UpdateBlogStatusModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("AuthApi");
        }

        [BindProperty]
        public int BlogId { get; set; }

        [BindProperty]
        public int NewStatusId { get; set; }

        public string? Message { get; set; }
        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var token = HttpContext.Session.GetString("Token");
            if (string.IsNullOrEmpty(token))
                return RedirectToPage("/BlogManage/Login");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var url = $"api/WriterBlog/{BlogId}/status?newStatusId={NewStatusId}";

            try
            {
                var response = await _httpClient.PatchAsync(url, null);
                if (response.IsSuccessStatusCode)
                {
                    Message = "✅ Trạng thái blog đã được cập nhật.";
                }
                else
                {
                    var errorJson = await response.Content.ReadAsStringAsync();
                    ErrorMessage = $"❌ Không thể cập nhật trạng thái: {errorJson}";
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
