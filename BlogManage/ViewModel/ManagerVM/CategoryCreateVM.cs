namespace BlogManage.ViewModel.ManagerVM
{
    public class CategoryCreateVM
    {

        public string Name { get; set; } = null!;

        public DateOnly? CreatedDate { get; set; }
        public bool? Status { get; set; }
    }
}
