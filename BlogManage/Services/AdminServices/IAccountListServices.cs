using BlogManage.ViewModel.AdminVM;

namespace BlogManage.Services.AdminServices
{
    public interface IAccountListServices
    {
        List<AccountVM> GetAccounts(int page, int pageSize, string? search, bool? status);
        void UpdateAccountStatus(int accountId, bool status);
        AccountDetailsVM? GetAccountDetailsById(int accountId);
        void CreateAccountByAdmin(CreateAccountVM model);
        List<RoleDropdownVM> GetValidRoles();
    }
}
