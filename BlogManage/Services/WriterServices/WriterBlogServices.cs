using AutoMapper;
using BlogManage.Models;
using BlogManage.Services.PublicServices;
using BlogManage.Services.WriterServices;
using BlogManage.ViewModel.PublicVM;
using BlogManage.ViewModel.WriterVM;
using Microsoft.EntityFrameworkCore;


namespace BlogManage.Services.WriterServices
{
    public class WriterBlogServices : IWriterBlogService
    {
        private readonly BlogManageContext _context;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;

        public WriterBlogServices(BlogManageContext context, IMapper mapper, IFileService fileService)
        {
            _context = context;
            _mapper = mapper;
            _fileService = fileService;
        }

        private IQueryable<Blog> GetBlogsQuery(int profileId, string searchTerm = null, string sortOrder = "newest", int? categoryId = null)
        {
            var query = _context.Blogs
                .Include(x => x.Author)
                .Include(x => x.Category)
                .Include(x => x.Status)
                .Where(s => s.AuthorId == profileId);

            if (!string.IsNullOrEmpty(searchTerm))
                query = query.Where(b => b.Title.Contains(searchTerm));

            if (categoryId.HasValue)
                query = query.Where(b => b.CategoryId == categoryId.Value);

            query = sortOrder.ToLower() == "oldest"
                ? query.OrderBy(b => b.CreatedDate)
                : query.OrderByDescending(b => b.CreatedDate);

            return query;
        }

        public List<BlogSumVM> GetAllBlog(int pageNumber, int pageSize, int profileId, string searchTerm = null, string sortOrder = "newest", int? categoryId = null)
        {
            var query = GetBlogsQuery(profileId, searchTerm, sortOrder, categoryId);
            var paginatedBlogs = query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return _mapper.Map<List<BlogSumVM>>(paginatedBlogs);
        }

        public BlogVM GetBlogDetail(int blogId)
        {
            var blog = _context.Blogs
                .Include(x => x.Author)
                .Include(x => x.Category)
                .Include(x => x.Status)
                .Include(x => x.Comments)
                .FirstOrDefault(s => s.Id == blogId);

            return blog == null ? null : _mapper.Map<BlogVM>(blog);
        }

        public async Task<bool> CreateBlogAsync(BlogCreateVM blogVM, IFormFile thumbnail, int profileId)
        {
            var validStatusIds = new List<int> { 1, 2 }; // draft or active

            if (!validStatusIds.Contains(blogVM.StatusId))
                throw new Exception("Status không hợp lệ.");

            if (string.IsNullOrWhiteSpace(blogVM.Title))
                throw new Exception("Title không được để trống.");

            if (string.IsNullOrWhiteSpace(blogVM.Content))
                throw new Exception("Content không được để trống.");

            if (string.IsNullOrWhiteSpace(blogVM.Summary))
                throw new Exception("Summary không được để trống.");

            if (thumbnail == null || thumbnail.Length == 0)
                throw new Exception("Thumbnail không hợp lệ.");

            var existingTitle = await _context.Blogs.AnyAsync(b => b.Title == blogVM.Title);
            if (existingTitle)
                throw new Exception("Title đã tồn tại, vui lòng chọn Title khác.");

            var thumbnailPath = await _fileService.UploadImageAsync(thumbnail, "blog");

            var newBlog = new Blog
            {
                Title = blogVM.Title,
                Content = blogVM.Content,
                Summary = blogVM.Summary,
                AuthorId = profileId,
                StatusId = blogVM.StatusId,
                CategoryId = blogVM.CategoryId,
                CreatedDate = DateOnly.FromDateTime(DateTime.Now),
                Thumbnail = thumbnailPath
            };

            _context.Blogs.Add(newBlog);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateBlogAsync(int blogId, BlogUpdateVM blogVM, IFormFile? thumbnail, int profileId)
        {
            var blog = await _context.Blogs.FindAsync(blogId);
            if (blog == null)
                throw new Exception("Không tìm thấy blog.");

            if (blog.AuthorId != profileId)
                throw new Exception("Bạn không có quyền chỉnh sửa blog này.");

            if (blog.StatusId == 4)
                throw new Exception("Bài viết đã bị khóa và không thể chỉnh sửa.");

            // Cập nhật nội dung
            if (!string.IsNullOrWhiteSpace(blogVM.Title)) blog.Title = blogVM.Title;
            if (!string.IsNullOrWhiteSpace(blogVM.Summary)) blog.Summary = blogVM.Summary;
            if (!string.IsNullOrWhiteSpace(blogVM.Content)) blog.Content = blogVM.Content;
            if (blogVM.CategoryId.HasValue) blog.CategoryId = blogVM.CategoryId.Value;

            if (blogVM.StatusId.HasValue && blogVM.StatusId.Value != 4)
                blog.StatusId = blogVM.StatusId.Value;

            if (thumbnail != null && thumbnail.Length > 0)
            {
                var thumbnailPath = await _fileService.UploadImageAsync(thumbnail, "blog");
                blog.Thumbnail = thumbnailPath;
            }

            blog.UpdateDate = DateOnly.FromDateTime(DateTime.Now);
            _context.Blogs.Update(blog);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ChangeStatusAsync(int blogId, int newStatusId, int userProfileId)
        {
            // Danh sách trạng thái hợp lệ
            var validUserStatus = new List<int> { 1, 2, 3 }; // draft, active, inactive
            if (!validUserStatus.Contains(newStatusId))
                throw new Exception("Trạng thái không hợp lệ. Chỉ cho phép: Nháp, Hoạt động, Ẩn.");

            var blog = await _context.Blogs.FindAsync(blogId);
            if (blog == null)
                throw new Exception("Không tìm thấy blog.");

            // Kiểm tra xem người dùng có phải là tác giả của bài viết không
            if (blog.AuthorId != userProfileId)
                throw new Exception("Bạn không có quyền chỉnh sửa trạng thái blog này.");

            // Kiểm tra trạng thái của blog trước khi thay đổi
            if (blog.StatusId == 4)
                throw new Exception("Bài viết đã bị khóa. Không thể thay đổi trạng thái.");

            // Cập nhật trạng thái
            blog.StatusId = newStatusId;
            blog.UpdateDate = DateOnly.FromDateTime(DateTime.Now);

            _context.Blogs.Update(blog);
            await _context.SaveChangesAsync();
            return true;
        }
    }

}

