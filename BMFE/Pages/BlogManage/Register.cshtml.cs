using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BlogManage.ViewModel.AuthenVM;

namespace BMFE.Pages.BlogManage
{
    public class RegisterModel : PageModel
    {
        private readonly HttpClient _httpClient;

        [BindProperty]
        public RegisterVM RegisterInfo { get; set; } = new RegisterVM();

        public RegisterModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var response = await _httpClient.PostAsJsonAsync("http://localhost:5000/api/authen/register", RegisterInfo);

                if (!response.IsSuccessStatusCode)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError(string.Empty, $"Lỗi: {errorMessage}");
                    return Page();
                }

                return RedirectToPage("/BlogManage/HomePage");
            }
            catch (HttpRequestException ex)
            {
                ModelState.AddModelError(string.Empty, $"Lỗi kết nối: {ex.Message}");
                return Page();
            }
        }
    }
}