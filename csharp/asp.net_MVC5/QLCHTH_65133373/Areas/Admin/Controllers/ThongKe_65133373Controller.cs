using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using QLCHTH_65133373.Filters;
using QLCHTH_65133373.Models;
using QLCHTH_65133373.Models.ViewModels;

namespace QLCHTH_65133373.Areas.Admin.Controllers
{
    [AdminOnlyFilter_65133373]
    public class ThongKe_65133373Controller : Controller
    {
        private QLCHTH_65133373Entities db = new QLCHTH_65133373Entities();

        // GET: Admin/ThongKe_65133373
        public ActionResult Index(string loaiThongKe = "ngay", string ngay = null, int? thang = null, int? nam = null, string tuNgay = null, string denNgay = null)
        {
            ViewBag.Title = "Thống kê doanh thu";
            ViewBag.LoaiThongKe = loaiThongKe;
            
            // Safe DateTime parsing
            DateTime parsedNgay = DateTime.Today;
            DateTime? parsedTuNgay = null;
            DateTime? parsedDenNgay = null;
            
            if (!string.IsNullOrEmpty(ngay))
            {
                DateTime.TryParse(ngay, out parsedNgay);
            }
            if (!string.IsNullOrEmpty(tuNgay))
            {
                DateTime temp;
                if (DateTime.TryParse(tuNgay, out temp)) parsedTuNgay = temp;
            }
            if (!string.IsNullOrEmpty(denNgay))
            {
                DateTime temp;
                if (DateTime.TryParse(denNgay, out temp)) parsedDenNgay = temp;
            }
            
            var model = new ThongKeViewModel
            {
                LoaiThongKe = loaiThongKe,
                Ngay = parsedNgay,
                Nam = nam ?? DateTime.Today.Year,
                Thang = thang ?? DateTime.Today.Month,
                TuNgay = parsedTuNgay,
                DenNgay = parsedDenNgay
            };
            
            ViewBag.Ngay = model.Ngay?.ToString("yyyy-MM-dd");
            ViewBag.Thang = model.Thang;
            ViewBag.Nam = model.Nam;
            ViewBag.TuNgay = model.TuNgay?.ToString("yyyy-MM-dd");
            ViewBag.DenNgay = model.DenNgay?.ToString("yyyy-MM-dd");
            
            DateTime filterStart, filterEnd;
            
            switch (loaiThongKe)
            {
                case "thang":
                    filterStart = new DateTime(model.Nam.Value, model.Thang.Value, 1);
                    filterEnd = filterStart.AddMonths(1);
                    model.KetQuaDoanhThu = ThongKeTheoThang(model.Thang.Value, model.Nam.Value);
                    model.TopSanPham = TopSanPhamTheoThang(model.Thang.Value, model.Nam.Value, 10);
                    break;
                    
                case "nam":
                    filterStart = new DateTime(model.Nam.Value, 1, 1);
                    filterEnd = new DateTime(model.Nam.Value + 1, 1, 1);
                    model.KetQuaDoanhThu = ThongKeTheoNam(model.Nam.Value);
                    model.TopSanPham = TopSanPhamTheoNam(model.Nam.Value, 10);
                    break;
                    
                case "khoang":
                    if (model.TuNgay.HasValue && model.DenNgay.HasValue)
                    {
                        filterStart = model.TuNgay.Value;
                        filterEnd = model.DenNgay.Value.AddDays(1);
                        model.KetQuaDoanhThu = ThongKeTheoKhoang(model.TuNgay.Value, model.DenNgay.Value);
                        model.TopSanPham = TopSanPhamTheoKhoang(model.TuNgay.Value, model.DenNgay.Value, 10);
                    }
                    else
                    {
                        filterStart = DateTime.Today;
                        filterEnd = DateTime.Today.AddDays(1);
                        model.KetQuaDoanhThu = ThongKeTheoNgay(DateTime.Today);
                        model.TopSanPham = TopSanPhamTheoNgay(DateTime.Today, 10);
                    }
                    break;
                    
                default: // "ngay"
                    filterStart = model.Ngay.Value.Date;
                    filterEnd = filterStart.AddDays(1);
                    model.KetQuaDoanhThu = ThongKeTheoNgay(model.Ngay.Value);
                    model.TopSanPham = TopSanPhamTheoNgay(model.Ngay.Value, 10);
                    break;
            }
            
            // Populate shortcut properties for View
            if (model.KetQuaDoanhThu != null)
            {
                model.TongDoanhThu = model.KetQuaDoanhThu.ThucThu;
                model.SoHoaDon = model.KetQuaDoanhThu.TongSoHoaDon;
                model.TongGiamGia = model.KetQuaDoanhThu.TongGiamGia;
            }
            
            // Số sản phẩm bán
            model.SoSanPhamBan = db.ChiTietHoaDons
                .Where(ct => ct.HoaDon.NgayBan >= filterStart && ct.HoaDon.NgayBan < filterEnd)
                .Sum(ct => (int?)ct.SoLuong) ?? 0;
            
            // Doanh thu theo danh mục
            model.DoanhThuTheoDanhMuc = db.ChiTietHoaDons
                .Where(ct => ct.HoaDon.NgayBan >= filterStart && ct.HoaDon.NgayBan < filterEnd)
                .GroupBy(ct => new { ct.SanPham.MaDanhMuc, ct.SanPham.DanhMuc.TenDanhMuc })
                .Select(g => new DoanhThuDanhMucResult
                {
                    MaDanhMuc = g.Key.MaDanhMuc,
                    TenDanhMuc = g.Key.TenDanhMuc,
                    DoanhThu = g.Sum(ct => ct.ThanhTien)
                })
                .OrderByDescending(x => x.DoanhThu)
                .ToList();
            
            return View(model);
        }

        private ThongKeDoanhThuResult ThongKeTheoNgay(DateTime ngay)
        {
            var hoaDons = db.HoaDons
                .Where(h => System.Data.Entity.DbFunctions.TruncateTime(h.NgayBan) == ngay.Date)
                .ToList();
            
            return new ThongKeDoanhThuResult
            {
                TieuDe = "Thống kê doanh thu",
                ThoiGian = ngay.ToString("dd/MM/yyyy"),
                TongSoHoaDon = hoaDons.Count,
                TongDoanhThu = hoaDons.Sum(h => h.TongTien),
                TongGiamGia = hoaDons.Sum(h => h.TongGiamGia),
                ThucThu = hoaDons.Sum(h => h.ThanhToan),
                ChiTiet = hoaDons
                    .GroupBy(h => h.NgayBan.Hour)
                    .OrderBy(g => g.Key)
                    .Select(g => new DoanhThuChiTiet
                    {
                        ThoiGian = $"{g.Key}:00 - {g.Key}:59",
                        SoHoaDon = g.Count(),
                        DoanhThu = g.Sum(h => h.TongTien),
                        GiamGia = g.Sum(h => h.TongGiamGia),
                        ThucThu = g.Sum(h => h.ThanhToan)
                    })
                    .ToList()
            };
        }

        private ThongKeDoanhThuResult ThongKeTheoThang(int thang, int nam)
        {
            var firstDay = new DateTime(nam, thang, 1);
            var lastDay = firstDay.AddMonths(1);
            
            var hoaDons = db.HoaDons
                .Where(h => h.NgayBan >= firstDay && h.NgayBan < lastDay)
                .ToList();
            
            return new ThongKeDoanhThuResult
            {
                TieuDe = "Thống kê doanh thu",
                ThoiGian = $"Tháng {thang}/{nam}",
                TongSoHoaDon = hoaDons.Count,
                TongDoanhThu = hoaDons.Sum(h => h.TongTien),
                TongGiamGia = hoaDons.Sum(h => h.TongGiamGia),
                ThucThu = hoaDons.Sum(h => h.ThanhToan),
                ChiTiet = hoaDons
                    .GroupBy(h => h.NgayBan.Day)
                    .OrderBy(g => g.Key)
                    .Select(g => new DoanhThuChiTiet
                    {
                        ThoiGian = $"Ngày {g.Key}",
                        SoHoaDon = g.Count(),
                        DoanhThu = g.Sum(h => h.TongTien),
                        GiamGia = g.Sum(h => h.TongGiamGia),
                        ThucThu = g.Sum(h => h.ThanhToan)
                    })
                    .ToList()
            };
        }

        private ThongKeDoanhThuResult ThongKeTheoNam(int nam)
        {
            var firstDay = new DateTime(nam, 1, 1);
            var lastDay = new DateTime(nam + 1, 1, 1);
            
            var hoaDons = db.HoaDons
                .Where(h => h.NgayBan >= firstDay && h.NgayBan < lastDay)
                .ToList();
            
            return new ThongKeDoanhThuResult
            {
                TieuDe = "Thống kê doanh thu",
                ThoiGian = $"Năm {nam}",
                TongSoHoaDon = hoaDons.Count,
                TongDoanhThu = hoaDons.Sum(h => h.TongTien),
                TongGiamGia = hoaDons.Sum(h => h.TongGiamGia),
                ThucThu = hoaDons.Sum(h => h.ThanhToan),
                ChiTiet = hoaDons
                    .GroupBy(h => h.NgayBan.Month)
                    .OrderBy(g => g.Key)
                    .Select(g => new DoanhThuChiTiet
                    {
                        ThoiGian = $"Tháng {g.Key}",
                        SoHoaDon = g.Count(),
                        DoanhThu = g.Sum(h => h.TongTien),
                        GiamGia = g.Sum(h => h.TongGiamGia),
                        ThucThu = g.Sum(h => h.ThanhToan)
                    })
                    .ToList()
            };
        }

        private ThongKeDoanhThuResult ThongKeTheoKhoang(DateTime tuNgay, DateTime denNgay)
        {
            var endDate = denNgay.AddDays(1);
            
            var hoaDons = db.HoaDons
                .Where(h => h.NgayBan >= tuNgay && h.NgayBan < endDate)
                .ToList();
            
            return new ThongKeDoanhThuResult
            {
                TieuDe = "Thống kê doanh thu",
                ThoiGian = $"{tuNgay:dd/MM/yyyy} - {denNgay:dd/MM/yyyy}",
                TongSoHoaDon = hoaDons.Count,
                TongDoanhThu = hoaDons.Sum(h => h.TongTien),
                TongGiamGia = hoaDons.Sum(h => h.TongGiamGia),
                ThucThu = hoaDons.Sum(h => h.ThanhToan),
                ChiTiet = hoaDons
                    .GroupBy(h => h.NgayBan.Date)
                    .OrderBy(g => g.Key)
                    .Select(g => new DoanhThuChiTiet
                    {
                        ThoiGian = g.Key.ToString("dd/MM/yyyy"),
                        SoHoaDon = g.Count(),
                        DoanhThu = g.Sum(h => h.TongTien),
                        GiamGia = g.Sum(h => h.TongGiamGia),
                        ThucThu = g.Sum(h => h.ThanhToan)
                    })
                    .ToList()
            };
        }

        private List<TopSanPhamResult> TopSanPhamTheoNgay(DateTime ngay, int top)
        {
            return db.ChiTietHoaDons
                .Where(ct => System.Data.Entity.DbFunctions.TruncateTime(ct.HoaDon.NgayBan) == ngay.Date)
                .GroupBy(ct => new { ct.MaSP, ct.SanPham.TenSP, ct.SanPham.DonViTinh })
                .Select(g => new
                {
                    g.Key.MaSP,
                    g.Key.TenSP,
                    g.Key.DonViTinh,
                    SoLuongBan = g.Sum(x => x.SoLuong),
                    DoanhThu = g.Sum(x => x.ThanhTien)
                })
                .OrderByDescending(x => x.SoLuongBan)
                .Take(top)
                .ToList()
                .Select((x, i) => new TopSanPhamResult
                {
                    STT = i + 1,
                    MaSP = x.MaSP,
                    TenSP = x.TenSP,
                    DonViTinh = x.DonViTinh,
                    SoLuongBan = x.SoLuongBan,
                    DoanhThu = x.DoanhThu
                })
                .ToList();
        }

        private List<TopSanPhamResult> TopSanPhamTheoThang(int thang, int nam, int top)
        {
            var firstDay = new DateTime(nam, thang, 1);
            var lastDay = firstDay.AddMonths(1);
            
            return db.ChiTietHoaDons
                .Where(ct => ct.HoaDon.NgayBan >= firstDay && ct.HoaDon.NgayBan < lastDay)
                .GroupBy(ct => new { ct.MaSP, ct.SanPham.TenSP, ct.SanPham.DonViTinh })
                .Select(g => new
                {
                    g.Key.MaSP,
                    g.Key.TenSP,
                    g.Key.DonViTinh,
                    SoLuongBan = g.Sum(x => x.SoLuong),
                    DoanhThu = g.Sum(x => x.ThanhTien)
                })
                .OrderByDescending(x => x.SoLuongBan)
                .Take(top)
                .ToList()
                .Select((x, i) => new TopSanPhamResult
                {
                    STT = i + 1,
                    MaSP = x.MaSP,
                    TenSP = x.TenSP,
                    DonViTinh = x.DonViTinh,
                    SoLuongBan = x.SoLuongBan,
                    DoanhThu = x.DoanhThu
                })
                .ToList();
        }

        private List<TopSanPhamResult> TopSanPhamTheoNam(int nam, int top)
        {
            var firstDay = new DateTime(nam, 1, 1);
            var lastDay = new DateTime(nam + 1, 1, 1);
            
            return db.ChiTietHoaDons
                .Where(ct => ct.HoaDon.NgayBan >= firstDay && ct.HoaDon.NgayBan < lastDay)
                .GroupBy(ct => new { ct.MaSP, ct.SanPham.TenSP, ct.SanPham.DonViTinh })
                .Select(g => new
                {
                    g.Key.MaSP,
                    g.Key.TenSP,
                    g.Key.DonViTinh,
                    SoLuongBan = g.Sum(x => x.SoLuong),
                    DoanhThu = g.Sum(x => x.ThanhTien)
                })
                .OrderByDescending(x => x.SoLuongBan)
                .Take(top)
                .ToList()
                .Select((x, i) => new TopSanPhamResult
                {
                    STT = i + 1,
                    MaSP = x.MaSP,
                    TenSP = x.TenSP,
                    DonViTinh = x.DonViTinh,
                    SoLuongBan = x.SoLuongBan,
                    DoanhThu = x.DoanhThu
                })
                .ToList();
        }

        private List<TopSanPhamResult> TopSanPhamTheoKhoang(DateTime tuNgay, DateTime denNgay, int top)
        {
            var endDate = denNgay.AddDays(1);
            
            return db.ChiTietHoaDons
                .Where(ct => ct.HoaDon.NgayBan >= tuNgay && ct.HoaDon.NgayBan < endDate)
                .GroupBy(ct => new { ct.MaSP, ct.SanPham.TenSP, ct.SanPham.DonViTinh })
                .Select(g => new
                {
                    g.Key.MaSP,
                    g.Key.TenSP,
                    g.Key.DonViTinh,
                    SoLuongBan = g.Sum(x => x.SoLuong),
                    DoanhThu = g.Sum(x => x.ThanhTien)
                })
                .OrderByDescending(x => x.SoLuongBan)
                .Take(top)
                .ToList()
                .Select((x, i) => new TopSanPhamResult
                {
                    STT = i + 1,
                    MaSP = x.MaSP,
                    TenSP = x.TenSP,
                    DonViTinh = x.DonViTinh,
                    SoLuongBan = x.SoLuongBan,
                    DoanhThu = x.DoanhThu
                })
                .ToList();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
