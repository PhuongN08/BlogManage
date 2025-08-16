namespace BlogManage.ViewModel.PublicVM
{
    public class BlogSumVM
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string Summary { get; set; } = null!;
        public int AuthorId { get; set; }
        public string AuthorDisplayName { get; set; }
        public string? Thumbnail { get; set; }

        public int StatusId { get; set; }

        public string StatusName { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        public DateOnly CreatedDate { get; set; }
    }
}
