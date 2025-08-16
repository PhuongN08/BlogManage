using BlogManage.Models;
using Microsoft.EntityFrameworkCore;
using BlogManage.ViewModel.AuthenVM;
using BlogManage.AutoMapper;
using AutoMapper;
using BlogManage.Services.AuthenServices;

namespace BlogManage.Services.AuthenServices
{
    public class AuthenSerivce : IAuthenService
    {
        private readonly BlogManageContext _context;
        private readonly IMapper _mapper;
        private readonly JwtTokenService _jwtTokenService;

        public AuthenSerivce(BlogManageContext context, IMapper mapper, JwtTokenService jwtTokenService)
        {
            _context = context;
            _mapper = mapper;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<string> RegisterAsync(RegisterVM vm)
        {
            var existingAccount = await _context.Accounts
                .FirstOrDefaultAsync(a => a.Username == vm.Username);

            if (existingAccount != null)
            {
                return "Tên đăng nhập đã tồn tại.";
            }

            var existingProfile = await _context.Profiles
                .FirstOrDefaultAsync(p => p.Phone == vm.Phone ||
                                           p.DisplayName == vm.DisplayName ||
                                           p.Email == vm.Email);

            if (existingProfile != null)
            {
                if (existingProfile.Phone == vm.Phone)
                    return "Số điện thoại đã được sử dụng.";

                if (existingProfile.DisplayName == vm.DisplayName)
                    return "Tên hiển thị đã được sử dụng.";

                if (existingProfile.Email == vm.Email)
                    return "Email đã được sử dụng.";
            }

            var today = DateOnly.FromDateTime(DateTime.Today);
            if (vm.Dob >= today)
            {
                return "Ngày sinh phải trước ngày tạo tài khoản.";
            }

            var account = _mapper.Map<Account>(vm);
            account.Password = PasswordHasher.HashPassword(vm.Password);
            account.Status = true;
            account.CreatedDate = today;

            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();

            var profile = _mapper.Map<Models.Profile>(vm);
            profile.CreatedDate = today;
            profile.RoleId = 2; // 2 = User
            profile.AccountId = account.Id;

            _context.Profiles.Add(profile);
            await _context.SaveChangesAsync();

            return "Đăng ký thành công.";
        }
        public async Task<LoginResponse?> LoginAsync(string username, string password)
        {
            try
            {
                var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Username == username);
                if (account == null || !PasswordHasher.VerifyPassword(password, account.Password))
                    return null;

                var profile = await _context.Profiles
                    .Include(p => p.Role)
                    .FirstOrDefaultAsync(p => p.AccountId == account.Id);
                if (profile == null) return null;

                var token = _jwtTokenService.GenerateToken(profile);

                return new LoginResponse
                {
                    Token = token,
                    UserId = profile.Id,
                    Role = profile.Role?.RoleName ?? "Writer",
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in LoginAsync: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                throw; 
            }
        }

    }
}
