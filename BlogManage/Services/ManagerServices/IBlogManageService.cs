using BlogManage.ViewModel.PublicVM;
using System.Threading.Tasks;

namespace BlogManage.Services.ManagerServices
{
    public interface IBlogManageService
    {
        List<BlogSumVM> GetAllBlog(int pageNumber, int pageSize, string searchTerm = null, string sortOrder = "newest", int? categoryId = null);
        List<BlogVM> GetBlogDetail(int blogId);
        Task<bool> UpdateBlogStatusAsync(int blogId, int newStatusId);
    }
}
