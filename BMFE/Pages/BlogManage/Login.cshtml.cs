using BlogManage.ViewModel.AuthenVM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BMFE.Pages.BlogManage
{
    public class LoginModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public LoginModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("AuthApi");
        }

        [BindProperty]
        public LoginVM Login { get; set; } = new LoginVM();

        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/authen/login", Login);
                if (!response.IsSuccessStatusCode)
                {
                    ErrorMessage = "Sai tài khoản hoặc mật khẩu.";
                    return Page();
                }

                var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();
                if (loginResponse == null)
                {
                    ErrorMessage = "Đã xảy ra lỗi trong quá trình đăng nhập.";
                    return Page();
                }

                // Lưu thông tin người dùng vào session
                HttpContext.Session.SetString("Token", loginResponse.Token);
                HttpContext.Session.SetString("UserId", loginResponse.UserId.ToString());
                HttpContext.Session.SetString("Role", loginResponse.Role);

                return RedirectToPage("/BlogManage/HomePage"); 
            }
            catch (HttpRequestException ex)
            {
                ErrorMessage = $"Lỗi: {ex.Message}";
                return Page();
            }
        }
    }
}