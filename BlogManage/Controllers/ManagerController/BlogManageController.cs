using AutoMapper;
using BlogManage.Models;
using BlogManage.Services.ManagerServices;
using BlogManage.ViewModel.PublicVM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogManage.Controllers.ManagerController
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogManageController : ControllerBase
    {
        private readonly IBlogManageService _blogService;

        public BlogManageController(IBlogManageService blogService)
        {
            _blogService = blogService;
        }

        [HttpGet("all")]
        [Authorize(Roles = "Manager")]
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

        [HttpGet("{blogId}")]
        [Authorize(Roles = "Manager")]
        public IActionResult GetBlogDetail(int blogId)
        {
            var result = _blogService.GetBlogDetail(blogId);
            if (result == null || !result.Any())
                return NotFound("Blog không tồn tại.");
            return Ok(result);
        }
        [HttpPut("status/{blogId}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> UpdateBlogStatus(int blogId, [FromQuery] int status)
        {
            try
            {
                var result = await _blogService.UpdateBlogStatusAsync(blogId, status);
                return Ok(new { message = "Cập nhật trạng thái blog thành công." });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = "Không thể cập nhật trạng thái blog.",
                    detail = ex.Message
                });
            }
        }



    }
}
