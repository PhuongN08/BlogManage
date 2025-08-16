using BlogManage.Services.AuthenServices;
using BlogManage.Services.WriterServices;
using BlogManage.ViewModel.WriterVM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BlogManage.Controllers.WriterControllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Writer")]

    public class WriterCommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public WriterCommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        // POST: api/writercomment
        [HttpPost]
        public async Task<IActionResult> CreateComment([FromBody] CreateCommentVM createCommentVM)
        {
            if (createCommentVM == null)
            {
                return BadRequest("Comment không thể null.");
            }

            if (string.IsNullOrWhiteSpace(createCommentVM.Content))
            {
                return BadRequest("Nội dung comment không được để trống.");
            }

            try
            {
                var profileId = JwtHelper.GetProfileId(User);
                var createdComment = await _commentService.CreateCommentAsync(createCommentVM, profileId);

                // Đảm bảo rằng createdComment có thuộc tính Id
                return CreatedAtAction(nameof(CreateComment), new { id = createdComment.Id }, createdComment);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/writercomment/{id}/status
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateCommentStatus(int id, [FromBody] bool status)
        {
            var profileId = JwtHelper.GetProfileId(User);
            var resultMessage = await _commentService.UpdateCommentStatusAsync(id, status, profileId);

            // Kiểm tra thông báo trả về
            if (resultMessage == "Bình luận không tồn tại.")
            {
                return NotFound(resultMessage);
            }
            else if (resultMessage == "Bạn không có quyền sửa đổi bình luận này.")
            {
                return Forbid(resultMessage); // Trả về 403 Forbidden
            }

            return NoContent(); // Trả về 204 No Content nếu cập nhật thành công
        }
    }
}