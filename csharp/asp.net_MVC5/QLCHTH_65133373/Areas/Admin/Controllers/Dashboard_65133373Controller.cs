using System;
using System.Linq;
using System.Web.Mvc;
using QLCHTH_65133373.Filters;
using QLCHTH_65133373.Models;
using QLCHTH_65133373.Models.ViewModels;

namespace QLCHTH_65133373.Areas.Admin.Controllers
{
    [AdminOnlyFilter_65133373]
    public class Dashboard_65133373Controller : Controller
    {
        private QLCHTH_65133373Entities db = new QLCHTH_65133373Entities();

        public ActionResult Index()
        {
            ViewBag.Title = "Dashboard";
            
            // Thống kê tổng quan
            ViewBag.TongSanPham = db.SanPhams.Count();
            ViewBag.TongDanhMuc = db.DanhMucs.Count();
            ViewBag.TongNhaCungCap = db.NhaCungCaps.Count();
            ViewBag.TongTaiKhoan = db.TaiKhoans.Count();
            
            // Thống kê hôm nay
            var today = DateTime.Today;
            var hoaDonHomNay = db.HoaDons.Where(h => System.Data.Entity.DbFunctions.TruncateTime(h.NgayBan) == today);
            ViewBag.SoHoaDonHomNay = hoaDonHomNay.Count();
            ViewBag.DoanhThuHomNay = hoaDonHomNay.Sum(h => (decimal?)h.ThanhToan) ?? 0;
            
            // Thống kê tháng này
            var firstDayOfMonth = new DateTime(today.Year, today.Month, 1);
            var hoaDonThangNay = db.HoaDons.Where(h => h.NgayBan >= firstDayOfMonth && h.NgayBan <= today);
            ViewBag.SoHoaDonThangNay = hoaDonThangNay.Count();
            ViewBag.DoanhThuThangNay = hoaDonThangNay.Sum(h => (decimal?)h.ThanhToan) ?? 0;
            
            // Sản phẩm sắp hết hàng (dưới 10)
            ViewBag.SanPhamSapHet = db.SanPhams.Where(s => s.SoLuongTon < 10).OrderBy(s => s.SoLuongTon).Take(10).ToList();
            
            // Top 5 sản phẩm bán chạy trong tháng
            ViewBag.TopSanPham = db.ChiTietHoaDons
                .Where(ct => ct.HoaDon.NgayBan >= firstDayOfMonth)
                .GroupBy(ct => new { ct.MaSP, ct.SanPham.TenSP })
                .Select(g => new TopSanPhamViewModel { MaSP = g.Key.MaSP, TenSP = g.Key.TenSP, SoLuong = g.Sum(x => x.SoLuong) })
                .OrderByDescending(x => x.SoLuong)
                .Take(5)
                .ToList();
            
            // Khuyến mãi đang diễn ra
            ViewBag.KhuyenMaiDangDienRa = db.KhuyenMais
                .Where(km => km.NgayBatDau <= today && km.NgayKetThuc >= today)
                .Count();
            
            // 5 hóa đơn gần nhất
            ViewBag.HoaDonGanNhat = db.HoaDons
                .OrderByDescending(h => h.NgayBan)
                .Take(5)
                .ToList();
            
            return View();
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
