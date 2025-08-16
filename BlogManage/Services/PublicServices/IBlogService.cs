using BlogManage.Models;
using BlogManage.ViewModel.PublicVM;

namespace BlogManage.Services.PublicServices
{
    public interface IBlogService
    {
        List<BlogSumVM> GetAllBlog(int pageNumber, int pageSize, string searchTerm = null, string sortOrder = "newest", int? categoryId = null);
        BlogVM GetBlogDetail(int blogId);

    }
}
