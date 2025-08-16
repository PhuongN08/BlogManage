using BlogManage.ViewModel.AdminVM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace BMFE.Pages.Admin
{
    public class AccountListModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public AccountListModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("AuthApi");
        }

        public List<AccountVM> Accounts { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? Search { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var token = HttpContext.Session.GetString("Token");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToPage("/BlogManage/Login");
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            int maxPageSize = 10000;

            var query = $"api/AccountList?page=1&pageSize={maxPageSize}";

            if (!string.IsNullOrWhiteSpace(Search))
            {
                query += $"&search={Uri.EscapeDataString(Search)}";
            }

            try
            {
                var response = await _httpClient.GetAsync(query);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    Accounts = JsonSerializer.Deserialize<List<AccountVM>>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }) ?? new List<AccountVM>();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Không thể tải danh sách tài khoản.");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Lỗi khi gọi API: {ex.Message}");
            }

            return Page();
        }

    }
}
