using System;
using System.ComponentModel.DataAnnotations;

namespace QLCHTH_65133373.Models.ViewModels
{
    public class TaiKhoanViewModel
    {
        public int MaTK { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên đăng nhập")]
        [Display(Name = "Tên đăng nhập")]
        [StringLength(50, ErrorMessage = "Tên đăng nhập không được vượt quá 50 ký tự")]
        [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "Tên đăng nhập chỉ được chứa chữ cái, số và dấu gạch dưới")]
        public string TenDangNhap { get; set; }

        [Display(Name = "Mật khẩu")]
        [StringLength(255, MinimumLength = 3, ErrorMessage = "Mật khẩu phải từ 3-255 ký tự")]
        [DataType(DataType.Password)]
        public string MatKhau { get; set; }

        [Display(Name = "Xác nhận mật khẩu")]
        [DataType(DataType.Password)]
        [Compare("MatKhau", ErrorMessage = "Mật khẩu xác nhận không khớp")]
        public string XacNhanMatKhau { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn loại tài khoản")]
        [Display(Name = "Loại tài khoản")]
        [Range(0, 1, ErrorMessage = "Loại tài khoản không hợp lệ")]
        public byte LoaiTaiKhoan { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên hiển thị")]
        [Display(Name = "Tên hiển thị")]
        [StringLength(50, ErrorMessage = "Tên hiển thị không được vượt quá 50 ký tự")]
        public string TenHienThi { get; set; }

        [Display(Name = "Ngày tạo")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime NgayTao { get; set; }

        [Display(Name = "Ghi chú")]
        [StringLength(255, ErrorMessage = "Ghi chú không được vượt quá 255 ký tự")]
        public string GhiChu { get; set; }

        // Display property
        public string LoaiTaiKhoanText => LoaiTaiKhoan == 0 ? "Quản trị viên" : "Thu ngân";
    }

    public class DoiMatKhauViewModel
    {
        public int MaTK { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu cũ")]
        [Display(Name = "Mật khẩu cũ")]
        [DataType(DataType.Password)]
        public string MatKhauCu { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu mới")]
        [Display(Name = "Mật khẩu mới")]
        [StringLength(255, MinimumLength = 3, ErrorMessage = "Mật khẩu phải từ 3-255 ký tự")]
        [DataType(DataType.Password)]
        public string MatKhauMoi { get; set; }

        [Required(ErrorMessage = "Vui lòng xác nhận mật khẩu mới")]
        [Display(Name = "Xác nhận mật khẩu mới")]
        [DataType(DataType.Password)]
        [Compare("MatKhauMoi", ErrorMessage = "Mật khẩu xác nhận không khớp")]
        public string XacNhanMatKhauMoi { get; set; }
    }
}
