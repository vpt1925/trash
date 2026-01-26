using System.ComponentModel.DataAnnotations;

namespace QLCHTH_65133373.Models.ViewModels
{
    public class NhaCungCapViewModel
    {
        public int MaNCC { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên nhà cung cấp")]
        [Display(Name = "Tên nhà cung cấp")]
        [StringLength(150, ErrorMessage = "Tên nhà cung cấp không được vượt quá 150 ký tự")]
        public string TenNCC { get; set; }

        [Display(Name = "Địa chỉ")]
        [StringLength(255, ErrorMessage = "Địa chỉ không được vượt quá 255 ký tự")]
        public string DiaChi { get; set; }

        [Display(Name = "Điện thoại")]
        [StringLength(11, ErrorMessage = "Số điện thoại không được vượt quá 11 ký tự")]
        [RegularExpression(@"^[0-9]{10,11}$", ErrorMessage = "Số điện thoại không hợp lệ")]
        public string DienThoai { get; set; }

        [Display(Name = "Email")]
        [StringLength(100, ErrorMessage = "Email không được vượt quá 100 ký tự")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; }

        // Display property
        public int SoSanPham { get; set; }
    }
}
