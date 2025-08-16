using System.ComponentModel.DataAnnotations;

namespace BlogManage.ViewModel.WriterVM
{
    public class BlogCreateVM
    {
        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Content is required.")]
        public string Content { get; set; }

        [Required(ErrorMessage = "Summary is required.")]
        public string Summary { get; set; }

        public int StatusId { get; set; }
        public int CategoryId { get; set; }
    }
}
