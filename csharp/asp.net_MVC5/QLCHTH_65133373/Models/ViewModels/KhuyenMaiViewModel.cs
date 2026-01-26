using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QLCHTH_65133373.Models.ViewModels
{
    public class KhuyenMaiViewModel
    {
        public int MaKM { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên khuyến mãi")]
        [Display(Name = "Tên khuyến mãi")]
        [StringLength(200, ErrorMessage = "Tên khuyến mãi không được vượt quá 200 ký tự")]
        public string TenKM { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn ngày bắt đầu")]
        [Display(Name = "Ngày bắt đầu")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime NgayBatDau { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn ngày kết thúc")]
        [Display(Name = "Ngày kết thúc")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime NgayKetThuc { get; set; }

        [Display(Name = "Mô tả")]
        [StringLength(255, ErrorMessage = "Mô tả không được vượt quá 255 ký tự")]
        public string MoTa { get; set; }

        // Chi tiết khuyến mãi
        public List<ChiTietKhuyenMaiViewModel> ChiTietKhuyenMais { get; set; } = new List<ChiTietKhuyenMaiViewModel>();

        // Display property - Trạng thái khuyến mãi
        public string TrangThai
        {
            get
            {
                var today = DateTime.Today;
                if (NgayBatDau > today) return "Chưa bắt đầu";
                if (NgayKetThuc < today) return "Đã kết thúc";
                return "Đang diễn ra";
            }
        }

        public string TrangThaiClass
        {
            get
            {
                var today = DateTime.Today;
                if (NgayBatDau > today) return "bg-warning";
                if (NgayKetThuc < today) return "bg-secondary";
                return "bg-success";
            }
        }

        public bool ChuaBatDau => NgayBatDau > DateTime.Today;
        public bool DangDienRa => NgayBatDau <= DateTime.Today && NgayKetThuc >= DateTime.Today;
        public bool DaKetThuc => NgayKetThuc < DateTime.Today;

        public int SoSanPham { get; set; }
    }

    public class ChiTietKhuyenMaiViewModel
    {
        public int MaKM { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn sản phẩm")]
        [Display(Name = "Sản phẩm")]
        public int MaSP { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập phần trăm giảm")]
        [Display(Name = "Phần trăm giảm")]
        [Range(0.01, 100, ErrorMessage = "Phần trăm giảm phải từ 0.01-100")]
        public decimal PhanTramGiam { get; set; }

        // Navigation display properties
        public string TenSP { get; set; }
        public decimal DonGia { get; set; }
        public string DonViTinh { get; set; }
        
        // Computed properties for display
        public decimal GiaGoc => DonGia;
        public decimal GiaSauGiam => DonGia * (1 - PhanTramGiam / 100);
    }

    public class TaoKhuyenMaiViewModel : KhuyenMaiViewModel, IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (NgayKetThuc < NgayBatDau)
            {
                yield return new ValidationResult(
                    "Ngày kết thúc phải sau hoặc bằng ngày bắt đầu",
                    new[] { nameof(NgayKetThuc) });
            }
        }
    }
}
