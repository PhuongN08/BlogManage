using BlogManage.ViewModel.PublicVM;
using BlogManage.ViewModel.WriterVM;

namespace BlogManage.Services.WriterServices
{
    public interface IWriterBlogService
    {
        List<BlogSumVM> GetAllBlog(int pageNumber, int pageSize, int profileId, string searchTerm = null, string sortOrder = "newest", int? categoryId = null);
        BlogVM GetBlogDetail(int blogId);
        Task<bool> CreateBlogAsync(BlogCreateVM blogVM, IFormFile thumbnail, int profileId);
        Task<bool> UpdateBlogAsync(int blogId, BlogUpdateVM blogVM, IFormFile? thumbnail, int profileId);
        Task<bool> ChangeStatusAsync(int blogId, int newStatusId, int profileId);

    }
}
