using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BlogManage.Services.AdminServices;
using BlogManage.ViewModel.AdminVM;

namespace BlogManage.Controllers.AdminController
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountListController : ControllerBase
    {
        private readonly IAccountListServices _accountListService;

        public AccountListController(IAccountListServices accountListService)
        {
            _accountListService = accountListService;
        }

        // Chỉ Admin mới được xem danh sách account
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAccounts(int page = 1, int pageSize = 10, string? search = null, bool? status = null)
        {
            var accounts = _accountListService.GetAccounts(page, pageSize, search, status);
            return Ok(accounts);
        }

        // Chỉ Admin mới được ban/unban
        [HttpPut("ban/unban/{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateAccountStatus(int id, [FromQuery] bool status)
        {
            _accountListService.UpdateAccountStatus(id, status);
            var action = status ? "đã được mở khóa" : "đã bị khóa";
            return Content($"Tài khoản {action} thành công.");
        }

        // Admin và Moderator được xem chi tiết
        [HttpGet("details/{id}")]
        [Authorize(Roles = "Admin,Moderator")]
        public IActionResult GetAccountDetails(int id)
        {
            var details = _accountListService.GetAccountDetailsById(id);
            if (details == null) return NotFound("Không tìm thấy tài khoản!");
            return Ok(details);
        }

        // Chỉ Admin mới được tạo account
        [HttpPost("create")]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateAccount([FromBody] CreateAccountVM model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            _accountListService.CreateAccountByAdmin(model);
            return Ok(new { message = "Tạo tài khoản thành công." });
        }

        // Ai cũng có thể xem các role hợp lệ (nếu cần public)
        [HttpGet("valid-roles")]
        [AllowAnonymous] // hoặc [Authorize(Roles = "Admin")] nếu bạn muốn giới hạn
        public IActionResult GetValidRoles()
        {
            var roles = _accountListService.GetValidRoles();
            return Ok(roles);
        }
    }
}
