using AutoMapper;
using BlogManage.Models;
using BlogManage.ViewModel.PublicVM;
using Microsoft.EntityFrameworkCore;

namespace BlogManage.Services.PublicServices
{
    public class BlogService : IBlogService
    {
        private readonly BlogManageContext _context;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;

        public BlogService(BlogManageContext context, IMapper mapper, IFileService fileService)
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
                .Where(s => s.StatusId == 2);

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

        public BlogVM GetBlogDetail(int blogId)
        {
            var blog = _context.Blogs
                .Include(x => x.Author)
                .Include(x => x.Category)
                .Include(x => x.Comments)
                .FirstOrDefault(s => s.Id == blogId && s.StatusId == 2); // Sử dụng FirstOrDefault để lấy một đối tượng

            return _mapper.Map<BlogVM>(blog); // Trả về một đối tượng duy nhất
        }
    }
}
