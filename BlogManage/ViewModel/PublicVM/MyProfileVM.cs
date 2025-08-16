namespace BlogManage.ViewModel.PublicVM
{
    public class MyProfileVM
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public DateOnly Dob { get; set; }
        public string Sex { get; set; } // "Nam" | "Nữ"
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Avatar { get; set; }
        public string RoleName { get; set; }
    }
}
