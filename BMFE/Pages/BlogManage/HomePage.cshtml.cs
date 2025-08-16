using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace BMFE.Pages.BlogManage
{
    public class HomePageModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public int UserId { get; set; }
        public string Role { get; set; }
        public string Message { get; set; }
        public List<BlogSumVM> Blogs { get; set; } = new List<BlogSumVM>();

        public HomePageModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task OnGetAsync()
        {
            UserId = int.TryParse(HttpContext.Session.GetString("UserId"), out var userId) ? userId : 0;
            Role = HttpContext.Session.GetString("Role");

            if (UserId == 0 || string.IsNullOrEmpty(Role))
            {
                Message = "Bạn cần đăng nhập để xem trang này.";
                Response.Redirect("/BlogManage/Login");
                return;
            }

            var searchTerm = Request.Query["searchTerm"].ToString();
            await LoadBlogsAsync(searchTerm);
        }

        private async Task LoadBlogsAsync(string searchTerm)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"http://localhost:5000/api/blog/all?pageNumber=1&pageSize=1000&searchTerm={searchTerm}");

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                Blogs = JsonConvert.DeserializeObject<List<BlogSumVM>>(jsonResponse);
            }
            else
            {
                Message = "Không thể tải danh sách blog.";
            }
        }

        public class BlogSumVM
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public DateTime CreatedDate { get; set; }
            public string Summary { get; set; }
            public string AuthorDisplayName { get; set; }
            public string Thumbnail { get; set; } // Thumbnail
        }
    }
}