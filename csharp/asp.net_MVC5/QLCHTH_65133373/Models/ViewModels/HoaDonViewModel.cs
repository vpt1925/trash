using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QLCHTH_65133373.Models.ViewModels
{
    public class HoaDonViewModel
    {
        public int MaHD { get; set; }

        [Display(Name = "Ngày bán")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime NgayBan { get; set; }

        [Display(Name = "Nhân viên")]
        public int MaTK { get; set; }

        [Display(Name = "Tổng tiền")]
        [DisplayFormat(DataFormatString = "{0:N0} đ")]
        public decimal TongTien { get; set; }

        [Display(Name = "Tổng giảm giá")]
        [DisplayFormat(DataFormatString = "{0:N0} đ")]
        public decimal TongGiamGia { get; set; }

        [Display(Name = "Thanh toán")]
        [DisplayFormat(DataFormatString = "{0:N0} đ")]
        public decimal ThanhToan { get; set; }

        [Display(Name = "Ghi chú")]
        [StringLength(255, ErrorMessage = "Ghi chú không được vượt quá 255 ký tự")]
        public string GhiChu { get; set; }

        // Navigation display properties
        public string TenNhanVien { get; set; }

        // Chi tiết hóa đơn
        public List<ChiTietHoaDonViewModel> ChiTietHoaDons { get; set; } = new List<ChiTietHoaDonViewModel>();
    }

    public class ChiTietHoaDonViewModel
    {
        public int MaHD { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn sản phẩm")]
        [Display(Name = "Sản phẩm")]
        public int MaSP { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số lượng")]
        [Display(Name = "Số lượng")]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")]
        public int SoLuong { get; set; }

        [Display(Name = "Đơn giá")]
        [DisplayFormat(DataFormatString = "{0:N0} đ")]
        public decimal DonGia { get; set; }

        [Display(Name = "Phần trăm giảm")]
        [Range(0, 100, ErrorMessage = "Phần trăm giảm phải từ 0-100")]
        public decimal PhanTramGiam { get; set; }

        [Display(Name = "Tiền giảm")]
        [DisplayFormat(DataFormatString = "{0:N0} đ")]
        public decimal TienGiam { get; set; }

        [Display(Name = "Thành tiền")]
        [DisplayFormat(DataFormatString = "{0:N0} đ")]
        public decimal ThanhTien { get; set; }

        public int? MaKM { get; set; }

        // Navigation display properties
        public string TenSP { get; set; }
        public string DonViTinh { get; set; }
        public int SoLuongTon { get; set; }
        public string TenKM { get; set; }
    }

    public class TaoHoaDonViewModel
    {
        [Display(Name = "Ghi chú")]
        [StringLength(255, ErrorMessage = "Ghi chú không được vượt quá 255 ký tự")]
        public string GhiChu { get; set; }

        public List<ChiTietHoaDonViewModel> ChiTietHoaDons { get; set; } = new List<ChiTietHoaDonViewModel>();

        // For JSON API
        public List<ChiTietThanhToanItem> ChiTiet { get; set; } = new List<ChiTietThanhToanItem>();

        // Summary
        public decimal TongTien { get; set; }
        public decimal TongGiamGia { get; set; }
        public decimal ThanhToan { get; set; }
    }

    public class ChiTietThanhToanItem
    {
        public int MaSanPham { get; set; }
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }
        public decimal GiamGia { get; set; }
    }
}
