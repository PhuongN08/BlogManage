using System.ComponentModel.DataAnnotations;

namespace BlogManage.ViewModel.AdminVM
{
    public class CreateAccountVM : IValidatableObject
    {
        [Required(ErrorMessage = "Tên đăng nhập không được để trống.")]
        [RegularExpression("^[a-z]+$", ErrorMessage = "Tên đăng nhập chỉ được chứa chữ cái thường và không có ký tự đặc biệt.")]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage = "Mật khẩu không được để trống.")]
        [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự.")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Họ tên không được để trống.")]
        public string Name { get; set; } = null!;
        [Required(ErrorMessage = "Tên tài khoản không được để trống.")]
        public string DisplayName {  get; set; } = null!;

        [Required(ErrorMessage = "Email không được để trống.")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Địa chỉ không được để trống.")]
        public string Address { get; set; } = null!;

        [Required(ErrorMessage = "Ngày sinh không được để trống.")]
        public DateOnly Dob { get; set; }

        [Required(ErrorMessage = "Giới tính không được để trống.")]
        public Gender Sex { get; set; }


        [Required(ErrorMessage = "Số điện thoại không được để trống.")]
        [RegularExpression(@"^0\d{9}$", ErrorMessage = "Số điện thoại phải bắt đầu bằng số 0 và có đúng 10 chữ số.")]
        public string Phone { get; set; } = null!;

        [Required(ErrorMessage = "Vai trò không được để trống.")]
        public string RoleName { get; set; } = null!;

        // Validate ngày sinh phải trước ngày tạo (hôm nay)
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            if (Dob >= today)
            {
                yield return new ValidationResult("Ngày sinh phải trước ngày tạo tài khoản.", new[] { nameof(Dob) });
            }
        }
    }

    public enum Gender
    {
        Nữ = 0,
        Nam = 1
    }
}


