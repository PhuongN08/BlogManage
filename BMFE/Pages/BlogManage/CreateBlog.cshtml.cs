
using BlogManage.ViewModel.WriterVM;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BMFE.Pages.BlogManage
{
    public class CreateBlogModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public CreateBlogModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("AuthApi");
        }

        [BindProperty]
        public BlogCreateVM Blog { get; set; } = new BlogCreateVM();

        [BindProperty]
        public IFormFile Thumbnail { get; set; }

        public string? ErrorMessage { get; set; }
        public string? SuccessMessage { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ErrorMessage = "Vui lòng nhập đầy đủ thông tin.";
                return Page();
            }

            var token = HttpContext.Session.GetString("Token");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToPage("/BlogManage/Login");
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var content = new MultipartFormDataContent();
            content.Add(new StringContent(Blog.Title), "Title");
            content.Add(new StringContent(Blog.Content), "Content");
            content.Add(new StringContent(Blog.Summary), "Summary");
            content.Add(new StringContent(Blog.StatusId.ToString()), "StatusId");
            content.Add(new StringContent(Blog.CategoryId.ToString()), "CategoryId");

            if (Thumbnail != null && Thumbnail.Length > 0)
            {
                var fileContent = new StreamContent(Thumbnail.OpenReadStream());
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(Thumbnail.ContentType);
                content.Add(fileContent, "thumbnail", Thumbnail.FileName);
            }

            try
            {
                var response = await _httpClient.PostAsync("api/WriterBlog/AddBlog", content);
                if (response.IsSuccessStatusCode)
                {
                    SuccessMessage = "Tạo blog thành công!";
                    ModelState.Clear();
                    Blog = new BlogCreateVM();
                    return Page();
                }

                var result = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
                ErrorMessage = result?["detail"] ?? "Lỗi không xác định";
            }
            catch
            {
                ErrorMessage = "Đã xảy ra lỗi khi kết nối API.";
            }

            return Page();
        }
    }
}
