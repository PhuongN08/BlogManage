using AutoMapper;
using BlogManage.Models;
using BlogManage.ViewModel.PublicVM;
using Microsoft.EntityFrameworkCore;

namespace BlogManage.Services.PublicServices
{
    public class MyProfileServices : IMyProfileService
    {
        private readonly BlogManageContext _context;
        private readonly IMapper _mapper;
        public MyProfileServices(BlogManageContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<MyProfileVM> GetMyProfileAsync(int profileId)
        {
            try
            {
                // Truy ngược từ AccountId trong Profiles
                var profile = await _context.Profiles
                    .Include(p => p.Role)
                    .FirstOrDefaultAsync(p => p.Id == profileId);
                if (profile == null)
                    throw new Exception("Không tìm thấy hồ sơ người dùng.");

                var result = _mapper.Map<MyProfileVM>(profile);
                return result;

            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy hồ sơ người dùng: {ex.Message}");
            }
        }
        public async Task<bool> UpdateMyProfileAsync(int profileId, EditMyProfileVM vm, string? avatarPath)
        {
            try
            {
                var profile = await _context.Profiles.FirstOrDefaultAsync(p => p.Id == profileId);
                if (profile == null)
                    throw new Exception("Không tìm thấy hồ sơ người dùng.");

                // Kiểm tra xem số điện thoại và email đã tồn tại chưa
                if (await _context.Profiles.AnyAsync(p => p.Phone == vm.Phone && p.Id != profileId))
                {
                    throw new Exception("Số điện thoại đã tồn tại. Vui lòng sửa đổi.");
                }


                // Hàm trim và kiểm tra khoảng trắng
                string TrimOrThrow(string input, string field)
                {
                    var trimmed = input?.Trim();
                    if (string.IsNullOrWhiteSpace(trimmed))
                        throw new Exception($"{field} không được chỉ chứa khoảng trắng.");
                    return trimmed;
                }

                // Cập nhật thông tin hồ sơ
                profile.Name = TrimOrThrow(vm.Name, "Tên");
                profile.Address = TrimOrThrow(vm.Address, "Địa chỉ");
                profile.Dob = vm.Dob;
                profile.Sex = vm.Sex;
                profile.Phone = TrimOrThrow(vm.Phone, "Số điện thoại");

                if (!string.IsNullOrEmpty(avatarPath))
                    profile.Avatar = avatarPath;

                profile.UpdateDate = DateOnly.FromDateTime(DateTime.Now);

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi cập nhật hồ sơ: {ex.Message}");
            }
        }
    }
}
