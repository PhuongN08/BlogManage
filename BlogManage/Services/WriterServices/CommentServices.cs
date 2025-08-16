using AutoMapper;
using BlogManage.Models;
using BlogManage.Services.PublicServices;
using BlogManage.Services.WriterServices;
using BlogManage.ViewModel.PublicVM;
using BlogManage.ViewModel.WriterVM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace BlogManage.Services.WriterServices
{
    public class CommentServices : ICommentService
    {
        private readonly BlogManageContext _context;
        private readonly IMapper _mapper;

        public CommentServices(BlogManageContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Hàm tạo comment
        public async Task<CommentVM> CreateCommentAsync(CreateCommentVM createCommentVM, int userId)
        {
            if (createCommentVM.BlogId <= 0)
                throw new Exception("BlogId không hợp lệ.");

            if (string.IsNullOrWhiteSpace(createCommentVM.Content))
                throw new Exception("Content không được để trống.");

            var blogExists = await _context.Blogs.AnyAsync(b => b.Id == createCommentVM.BlogId);
            if (!blogExists)
                throw new Exception("Blog không tồn tại.");

            // Lấy thông tin của người dùng để lấy DisplayName
            var user = await _context.Profiles.FindAsync(userId);
            if (user == null)
                throw new Exception("Người dùng không tồn tại.");

            var comment = new Comment
            {
                BlogId = createCommentVM.BlogId,
                Content = createCommentVM.Content,
                Status = true,
                CreatedDate = DateOnly.FromDateTime(DateTime.UtcNow),
                UserId = userId
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            // Ánh xạ comment thành CommentVM
            var commentVM = _mapper.Map<CommentVM>(comment);
            commentVM.DisplayName = user.DisplayName; // Gán DisplayName từ người dùng

            return commentVM;
        }

        // Hàm cập nhật trạng thái comment
        public async Task<string> UpdateCommentStatusAsync(int commentId, bool status, int userId)
        {
            var comment = await _context.Comments.FindAsync(commentId);
            if (comment == null)
            {
                return "Bình luận không tồn tại.";
            }

            // Kiểm tra quyền sở hữu
            if (comment.UserId != userId)
            {
                return "Bạn không có quyền sửa đổi bình luận này.";
            }

            // Cập nhật trạng thái và ngày cập nhật
            comment.Status = status;
            comment.UpdateDate = DateOnly.FromDateTime(DateTime.UtcNow);

            _context.Comments.Update(comment);
            await _context.SaveChangesAsync();

            return "Cập nhật trạng thái bình luận thành công.";
        }
    }
}