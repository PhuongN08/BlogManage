namespace BlogManage.ViewModel.ManagerVM
{
    public class CategoryVM
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public DateOnly? CreatedDate { get; set; }

        public DateOnly? UpdateDate { get; set; }

        public bool? Status { get; set; }
    }
}
