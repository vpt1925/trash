using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QLCHTH_65133373.Models.ViewModels
{
    public class ThongKeViewModel
    {
        [Display(Name = "Loại thống kê")]
        public string LoaiThongKe { get; set; } // "ngay", "thang", "nam", "khoang"

        [Display(Name = "Ngày")]
        [DataType(DataType.Date)]
        public DateTime? Ngay { get; set; }

        [Display(Name = "Tháng")]
        [Range(1, 12, ErrorMessage = "Tháng phải từ 1-12")]
        public int? Thang { get; set; }

        [Display(Name = "Năm")]
        [Range(2000, 2100, ErrorMessage = "Năm không hợp lệ")]
        public int? Nam { get; set; }

        [Display(Name = "Từ ngày")]
        [DataType(DataType.Date)]
        public DateTime? TuNgay { get; set; }

        [Display(Name = "Đến ngày")]
        [DataType(DataType.Date)]
        public DateTime? DenNgay { get; set; }

        // Kết quả thống kê - Shortcut properties cho View
        [Display(Name = "Tổng doanh thu")]
        public decimal TongDoanhThu { get; set; }
        
        [Display(Name = "Số hóa đơn")]
        public int SoHoaDon { get; set; }
        
        [Display(Name = "Số sản phẩm bán")]
        public int SoSanPhamBan { get; set; }
        
        [Display(Name = "Tổng giảm giá")]
        public decimal TongGiamGia { get; set; }

        // Kết quả thống kê thu nhập (chi tiết)
        public ThongKeDoanhThuResult KetQuaDoanhThu { get; set; }

        // Kết quả top sản phẩm bán chạy
        public List<TopSanPhamResult> TopSanPham { get; set; } = new List<TopSanPhamResult>();
        
        // Alias for View compatibility
        public List<TopSanPhamResult> TopSanPhamBanChay => TopSanPham;
        
        // Doanh thu theo nhân viên
        public List<DoanhThuNhanVienResult> DoanhThuTheoNhanVien { get; set; } = new List<DoanhThuNhanVienResult>();
        
        // Doanh thu theo danh mục
        public List<DoanhThuDanhMucResult> DoanhThuTheoDanhMuc { get; set; } = new List<DoanhThuDanhMucResult>();
    }

    public class ThongKeDoanhThuResult
    {
        [Display(Name = "Tổng số hóa đơn")]
        public int TongSoHoaDon { get; set; }

        [Display(Name = "Tổng doanh thu")]
        [DisplayFormat(DataFormatString = "{0:N0} đ")]
        public decimal TongDoanhThu { get; set; }

        [Display(Name = "Tổng giảm giá")]
        [DisplayFormat(DataFormatString = "{0:N0} đ")]
        public decimal TongGiamGia { get; set; }

        [Display(Name = "Thực thu")]
        [DisplayFormat(DataFormatString = "{0:N0} đ")]
        public decimal ThucThu { get; set; }

        public string TieuDe { get; set; }
        public string ThoiGian { get; set; }

        // Chi tiết theo ngày/tháng/năm
        public List<DoanhThuChiTiet> ChiTiet { get; set; } = new List<DoanhThuChiTiet>();
    }

    public class DoanhThuChiTiet
    {
        public string ThoiGian { get; set; }
        public int SoHoaDon { get; set; }
        public decimal DoanhThu { get; set; }
        public decimal GiamGia { get; set; }
        public decimal ThucThu { get; set; }
    }

    public class TopSanPhamResult
    {
        public int STT { get; set; }
        public int MaSP { get; set; }
        public string TenSP { get; set; }
        // Alias for View compatibility
        public string TenSanPham => TenSP;
        public string DonViTinh { get; set; }
        public int SoLuongBan { get; set; }
        public decimal DoanhThu { get; set; }
    }
    
    public class DoanhThuNhanVienResult
    {
        public int MaTK { get; set; }
        public string TenNhanVien { get; set; }
        public int SoHoaDon { get; set; }
        public decimal DoanhThu { get; set; }
    }
    
    public class DoanhThuDanhMucResult
    {
        public int MaDanhMuc { get; set; }
        public string TenDanhMuc { get; set; }
        public decimal DoanhThu { get; set; }
    }
}
