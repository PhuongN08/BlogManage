using BlogManage.ViewModel.PublicVM;

namespace BlogManage.Services.PublicServices
{
    public interface IMyProfileService
    {
        Task<MyProfileVM> GetMyProfileAsync(int profileId);
        Task<bool> UpdateMyProfileAsync(int profileId, EditMyProfileVM vm, string? avatarPath);
    }
}
