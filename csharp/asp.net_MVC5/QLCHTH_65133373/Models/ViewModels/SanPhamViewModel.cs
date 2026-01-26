using System.ComponentModel.DataAnnotations;

namespace QLCHTH_65133373.Models.ViewModels
{
    public class SanPhamViewModel
    {
        public int MaSP { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên sản phẩm")]
        [Display(Name = "Tên sản phẩm")]
        [StringLength(150, ErrorMessage = "Tên sản phẩm không được vượt quá 150 ký tự")]
        public string TenSP { get; set; }

        [Display(Name = "Mô tả")]
        [StringLength(255, ErrorMessage = "Mô tả không được vượt quá 255 ký tự")]
        public string MoTaSP { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập đơn vị tính")]
        [Display(Name = "Đơn vị tính")]
        [StringLength(30, ErrorMessage = "Đơn vị tính không được vượt quá 30 ký tự")]
        public string DonViTinh { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập đơn giá")]
        [Display(Name = "Đơn giá")]
        [Range(1, double.MaxValue, ErrorMessage = "Đơn giá phải lớn hơn 0")]
        [DisplayFormat(DataFormatString = "{0:N0} đ")]
        public decimal DonGia { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số lượng tồn")]
        [Display(Name = "Số lượng tồn")]
        [Range(0, int.MaxValue, ErrorMessage = "Số lượng tồn không được âm")]
        public int SoLuongTon { get; set; }

        [Display(Name = "Ảnh sản phẩm")]
        [StringLength(200, ErrorMessage = "Đường dẫn ảnh không được vượt quá 200 ký tự")]
        public string AnhSanPham { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn danh mục")]
        [Display(Name = "Danh mục")]
        public int MaDanhMuc { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn nhà cung cấp")]
        [Display(Name = "Nhà cung cấp")]
        public int MaNCC { get; set; }

        // Navigation display properties
        public string TenDanhMuc { get; set; }
        public string TenNCC { get; set; }
    }
    
    // ViewModel cho Top sản phẩm bán chạy
    public class TopSanPhamViewModel
    {
        public int MaSP { get; set; }
        public string TenSP { get; set; }
        public int SoLuong { get; set; }
    }
}
