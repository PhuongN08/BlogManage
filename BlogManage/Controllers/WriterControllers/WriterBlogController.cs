using BlogManage.Models;
using BlogManage.Services.WriterServices;
using BlogManage.ViewModel.WriterVM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BlogManage.Services.AuthenServices;

namespace BlogManage.Controllers.WriterControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WriterBlogController : ControllerBase
    {
        private readonly IWriterBlogService _writerBlogService;

        public WriterBlogController(IWriterBlogService writerBlogService)
        {
            _writerBlogService = writerBlogService;
        }

        [HttpGet("getAll")]
        public IActionResult Get([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 5, [FromQuery] string? searchTerm = null, [FromQuery] string sortOrder = "newest", [FromQuery] int? categoryId = null)
        {
            var profileId = JwtHelper.GetProfileId(User);
            var result = _writerBlogService.GetAllBlog(pageNumber, pageSize, profileId, searchTerm, sortOrder, categoryId);
            return Ok(result);
        }

        [HttpGet("{blogId}")]
        public IActionResult GetBlogDetail(int blogId)
        {
            var result = _writerBlogService.GetBlogDetail(blogId);
            if (result == null)
            {
                return NotFound("Blog not found.");
            }
            return Ok(result);
        }

        [HttpPost("AddBlog")]
        public async Task<IActionResult> CreateBlog([FromForm] BlogCreateVM blogVM, IFormFile thumbnail)
        {
            try
            {
                var profileId = JwtHelper.GetProfileId(User);
                await _writerBlogService.CreateBlogAsync(blogVM, thumbnail, profileId);
                return Ok("Blog created successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = "Đã xảy ra lỗi",
                    detail = ex.Message
                });
            }
        }

        [HttpPut("{blogId}")]
        public async Task<IActionResult> UpdateBlog(int blogId, [FromForm] BlogUpdateVM blogVM, IFormFile? thumbnail)
        {
            try
            {
                var profileId = JwtHelper.GetProfileId(User);
                var result = await _writerBlogService.UpdateBlogAsync(blogId, blogVM, thumbnail, profileId);
                return Ok("Blog updated successfully.");
            }
            catch (Exception ex)
            {
                var detailedMessage = ex.InnerException?.InnerException?.Message ??
                                      ex.InnerException?.Message ??
                                      ex.Message;

                return BadRequest(new
                {
                    message = "Không thể cập nhật blog",
                    detail = detailedMessage
                });
            }
        }

        [HttpPatch("{blogId}/status")]
        public async Task<IActionResult> ChangeStatus(int blogId, [FromQuery] int newStatusId)
        {
            try
            {
                var profileId = JwtHelper.GetProfileId(User);
                var result = await _writerBlogService.ChangeStatusAsync(blogId, newStatusId, profileId);
                return Ok("Cập nhật trạng thái thành công.");
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = "Không thể cập nhật trạng thái",
                    detail = ex.Message
                });
            }
        }
    }
}
