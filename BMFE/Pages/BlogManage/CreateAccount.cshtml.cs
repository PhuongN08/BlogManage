using BlogManage.ViewModel.AdminVM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace BMFE.Pages.Admin
{
    public class CreateAccountModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public CreateAccountModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("AuthApi");
        }

        [BindProperty]
        public CreateAccountVM CreateAccountVM { get; set; } = new();

        [TempData]
        public string? Message { get; set; }

        public void OnGet()
        {
            // Trang load form tạo tài khoản
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var token = HttpContext.Session.GetString("Token");
            if (string.IsNullOrEmpty(token))
            {
                // Redirect to login nếu chưa đăng nhập
                return RedirectToPage("/BlogManage/Login");
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/AccountList/create", CreateAccountVM);

                if (response.IsSuccessStatusCode)
                {
                    Message = "Tạo tài khoản thành công.";
                    return RedirectToPage("AccountList"); // Redirect về danh sách tài khoản
                }
                else
                {
                    // Lấy lỗi trả về từ API
                    var error = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError(string.Empty, $"Lỗi API: {error}");
                    return Page();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Lỗi khi gọi API: {ex.Message}");
                return Page();
            }
        }
    }
}
