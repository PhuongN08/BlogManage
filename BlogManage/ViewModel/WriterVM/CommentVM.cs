namespace BlogManage.ViewModel.WriterVM
{
    public class CommentVM
    {
        public int Id { get; set; }

        public int BlogId { get; set; }

        public int UserId { get; set; }
        public string DisplayName {  get; set; }

        public string Content { get; set; } = null!;

        public bool Status { get; set; }

        public DateOnly CreatedDate { get; set; }

        public DateOnly? UpdateDate { get; set; }
    }
}
