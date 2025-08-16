using BlogManage.Services.AuthenServices;
using BlogManage.Services.PublicServices;
using BlogManage.ViewModel.PublicVM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;

namespace BlogManage.Controllers.PublicController
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IMyProfileService _myProfileService;

        public ProfileController(IMyProfileService myProfileService)
        {
            _myProfileService = myProfileService;
        }
        [Authorize]
        [HttpGet("my-profile")]
        public async Task<IActionResult> GetMyProfile()
        {
            
            int userId = JwtHelper.GetProfileId(User);
            var profile = await _myProfileService.GetMyProfileAsync(userId);
            return Ok(profile);
        }
        [Authorize]
        [HttpPut("my-profile")]
        public async Task<IActionResult> UpdateMyProfile([FromForm] EditMyProfileVM vm, IFormFile? avatar)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                .Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { message = "Dữ liệu không hợp lệ.", errors });
            }

            int userId = JwtHelper.GetProfileId(User);

            try
            {
                // xử lý lưu ảnh nếu có
                string? avatarPath = null;
                if (avatar != null && avatar.Length > 0)
                {
                    var ext = Path.GetExtension(avatar.FileName);
                    var fileName = $"avatar_{userId}_{Guid.NewGuid():N}{ext}";
                    var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Profile");
                    if (!Directory.Exists(folderPath))
                        Directory.CreateDirectory(folderPath);

                    var fullPath = Path.Combine(folderPath, fileName);
                    using var stream = new FileStream(fullPath, FileMode.Create);
                    await avatar.CopyToAsync(stream);

                    avatarPath = $"/Profile/{fileName}";
                }

                var success = await _myProfileService.UpdateMyProfileAsync(userId, vm, avatarPath);
                return success ? Ok("Cập nhật thành công.") : StatusCode(500, "Cập nhật thất bại.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Lỗi cập nhật: {ex.Message}");
            }
        }
    }
}
