using BlogManage.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace BMFE.Pages.BlogManage
{
    public class BlogDetailModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public BlogDetailModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public int UserId { get; set; }
        public string Role { get; set; }
        public string Message { get; set; }
        public BlogDetailVM Blog { get; set; }

        public async Task OnGetAsync(int id)
        {
            // Kiểm tra người dùng
            UserId = int.TryParse(HttpContext.Session.GetString("UserId"), out var userId) ? userId : 0;
            Role = HttpContext.Session.GetString("Role");

            if (UserId == 0 || string.IsNullOrEmpty(Role))
            {
                Message = "Bạn cần đăng nhập để xem trang này.";
                Response.Redirect("/BlogManage/Login");
                return;
            }

            // Tải chi tiết blog
            await LoadBlogDetailAsync(id);
        }

        private async Task LoadBlogDetailAsync(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"http://localhost:5000/api/blog/detail/{id}");

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                Blog = JsonConvert.DeserializeObject<BlogDetailVM>(jsonResponse);
            }
            else
            {
                Message = "Không thể tải chi tiết blog.";
            }
        }
    }

    public class BlogDetailVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string AuthorDisplayName { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<CommentVM> Comments { get; set; }
        public string CategoryName { get; set; } // Thêm thuộc tính CategoryName
    }

    public class CommentVM
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}