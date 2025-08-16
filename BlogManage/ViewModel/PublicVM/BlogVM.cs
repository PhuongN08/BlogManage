using System;
using System.Collections.Generic;
using BlogManage.ViewModel.WriterVM;
namespace BlogManage.ViewModel.PublicVM
{
    public class BlogVM
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string Summary { get; set; } = null!;

        public string Content { get; set; } = null!;

        public string? Thumbnail { get; set; }

        public int AuthorId { get; set; }
        public string AuthorDisplayName { get; set; }

        public int StatusId { get; set; }

        public string StatusName {  get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        public DateOnly CreatedDate { get; set; }

        public DateOnly? UpdateDate { get; set; }
        public List<CommentVM> Comments { get; set; }
    }
}
