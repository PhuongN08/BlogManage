using BlogManage.Services.AuthenServices;
using BlogManage.ViewModel.AuthenVM;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BlogManage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenController : ControllerBase
    {
        private readonly IAuthenService _authenService;

        public AuthenController(IAuthenService authenService)
        {
            _authenService = authenService;
        }

        [HttpPost("register")]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(typeof(object), 400)]
        [ProducesResponseType(typeof(object), 500)]
        public async Task<IActionResult> Register([FromBody] RegisterVM vm)
        {
            if (vm == null)
                return BadRequest("Dữ liệu không hợp lệ");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authenService.RegisterAsync(vm);
            if (result.StartsWith("Đăng ký thành công"))
            {
                return Ok(new { message = result });
            }

            return BadRequest(result); 
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginResponse), 200)]
        [ProducesResponseType(typeof(object), 401)]
        public async Task<IActionResult> Login([FromBody] LoginVM vm)
        {
            if (vm == null)
                return BadRequest("Thiếu thông tin đăng nhập");

            var response = await _authenService.LoginAsync(vm.Username, vm.Password);
            if (response == null)
                return Unauthorized(new { message = "Sai tài khoản hoặc mật khẩu" });

            return Ok(response);
        }
    }
}