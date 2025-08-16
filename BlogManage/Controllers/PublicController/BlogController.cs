using BlogManage.Services.PublicServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogManage.Controllers.PublicController
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IBlogService _blogService;
        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        [HttpGet("all")]
        public IActionResult GetAllBlogs(
            int pageNumber = 1,
            int pageSize = 10,
            string searchTerm = null,
            string sortOrder = "newest",
            int? categoryId = null)
        {
            var blogs = _blogService.GetAllBlog(pageNumber, pageSize, searchTerm, sortOrder, categoryId);
            return Ok(blogs);
        }

        [HttpGet("detail/{blogId}")]
        public IActionResult GetBlogDetail(int blogId)
        {
            var blogDetail = _blogService.GetBlogDetail(blogId);
            if (blogDetail == null)
            {
                return NotFound();
            }
            return Ok(blogDetail); 
        }


    }
}
