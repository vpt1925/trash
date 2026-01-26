using System.ComponentModel.DataAnnotations;

namespace QLCHTH_65133373.Models.ViewModels
{
    public class DanhMucViewModel
    {
        public int MaDanhMuc { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên danh mục")]
        [Display(Name = "Tên danh mục")]
        [StringLength(100, ErrorMessage = "Tên danh mục không được vượt quá 100 ký tự")]
        public string TenDanhMuc { get; set; }

        [Display(Name = "Mô tả")]
        [StringLength(255, ErrorMessage = "Mô tả không được vượt quá 255 ký tự")]
        public string MoTa { get; set; }

        // Display property
        public int SoSanPham { get; set; }
    }
}
