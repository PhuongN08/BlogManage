namespace BlogManage.ViewModel.AuthenVM
{
    public class LoginVM
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
    public class LoginResponse
    {
        public string Token { get; set; }
        public int UserId { get; set; }
        public string Role { get; set; }
    }

}
