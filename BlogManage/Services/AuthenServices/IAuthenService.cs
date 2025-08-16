using BlogManage.ViewModel.AuthenVM;

namespace BlogManage.Services.AuthenServices
{
    public interface IAuthenService
    {
        Task<string> RegisterAsync(RegisterVM vm);
        Task<LoginResponse?> LoginAsync(string username, string password);

    }
}