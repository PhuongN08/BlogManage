namespace BlogManage.ViewModel.WriterVM
{
    public class BlogUpdateVM
    {
        public string? Title { get; set; }
        public string? Summary { get; set; }
        public string? Content { get; set; }
        public int? CategoryId { get; set; }
        public int? StatusId { get; set; } // nếu cho phép sửa trạng thái
    }
}
