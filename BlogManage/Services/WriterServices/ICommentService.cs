using BlogManage.ViewModel.WriterVM;

namespace BlogManage.Services.WriterServices
{
    public interface ICommentService
    {
        Task<CommentVM> CreateCommentAsync(CreateCommentVM createCommentVM, int userId);
        Task<string> UpdateCommentStatusAsync(int commentId, bool status, int userId);
    }
}
