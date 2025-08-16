using AutoMapper;
using BlogManage.Models;
using BlogManage.Services.PublicServices;
using BlogManage.ViewModel.PublicVM;
using Microsoft.EntityFrameworkCore;
namespace BlogManage.Services.ManagerServices
{
    public class BlogManageService : IBlogManageService
    {
        private readonly BlogManageContext _context;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        public BlogManageService(BlogManageContext context, IMapper mapper, IFileService fileService)
        {
            _context = context;
            _mapper = mapper;
            _fileService = fileService;
        }
        private IQueryable<Blog> GetBlogsQuery(string searchTerm = null, string sortOrder = "newest", int? categoryId = null)
        {
            var query = _context.Blogs
                .Include(x => x.Author)
                .Include(x => x.Category)
                .Include(x => x.Status)
                .Include(x => x.Comments)
                .Where(s => s.StatusId == 2 || s.StatusId == 3 || s.StatusId == 4 );

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(b => b.Title.Contains(searchTerm));
            }

            if (categoryId.HasValue)
            {
                query = query.Where(b => b.CategoryId == categoryId.Value);
            }

            if (sortOrder.ToLower() == "oldest")
            {
                query = query.OrderBy(b => b.CreatedDate);
            }
            else
            {
                query = query.OrderByDescending(b => b.CreatedDate);
            }

            return query;
        }


        public List<BlogSumVM> GetAllBlog(int pageNumber, int pageSize, string searchTerm = null, string sortOrder = "newest", int? categoryId = null)
        {
            var query = GetBlogsQuery(searchTerm, sortOrder, categoryId);
            var paginatedBlogs = query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return _mapper.Map<List<BlogSumVM>>(paginatedBlogs);
        }


        public List<BlogVM> GetBlogDetail(int blogId)
        {
            var blog = _context.Blogs
                .Include(x => x.Author)
                .Where(s => s.StatusId == 2 || s.StatusId == 3 || s.StatusId == 4)
                .ToList();

            return _mapper.Map<List<BlogVM>>(blog);
        }

        public async Task<bool> UpdateBlogStatusAsync(int blogId, int newStatusId)
        {
            var validStatusIds = new List<int> { 2, 4 }; // 2: Active, 4: Locked
            if (!validStatusIds.Contains(newStatusId))
                throw new Exception("Trạng thái không hợp lệ. Chỉ được phép đặt là 2 (Active) hoặc 4 (Locked).");

            var blog = await _context.Blogs.FirstOrDefaultAsync(b => b.Id == blogId);
            if (blog == null)
                throw new Exception("Blog không tồn tại.");

            blog.StatusId = newStatusId;
            blog.UpdateDate = DateOnly.FromDateTime(DateTime.Now);

            await _context.SaveChangesAsync();
            return true;
        }


    }
}
