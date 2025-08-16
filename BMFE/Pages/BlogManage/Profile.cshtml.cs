using BlogManage.ViewModel.PublicVM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BMFE.Pages.BlogManage
{
    public class ProfileModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public ProfileModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("AuthApi");
        }

        public MyProfileVM? Profile { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var token = HttpContext.Session.GetString("Token");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToPage("/BlogManage/Login");
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            try
            {
                var response = await _httpClient.GetAsync("api/Profile/my-profile");

                if (!response.IsSuccessStatusCode)
                {
                    ModelState.AddModelError(string.Empty, "Không thể lấy thông tin người dùng.");
                    return Page();
                }

                Profile = await response.Content.ReadFromJsonAsync<MyProfileVM>();
                return Page();
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Đã xảy ra lỗi khi kết nối API.");
                return Page();
            }
        }
    }
}
