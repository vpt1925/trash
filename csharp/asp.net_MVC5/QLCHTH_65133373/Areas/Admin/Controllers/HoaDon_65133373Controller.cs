using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using QLCHTH_65133373.Filters;
using QLCHTH_65133373.Models;
using QLCHTH_65133373.Models.ViewModels;

namespace QLCHTH_65133373.Areas.Admin.Controllers
{
    [AdminOnlyFilter_65133373]
    public class HoaDon_65133373Controller : Controller
    {
        private QLCHTH_65133373Entities db = new QLCHTH_65133373Entities();
        private const int PageSize = 10;

        // GET: Admin/HoaDon_65133373
        public ActionResult Index(DateTime? tuNgay, DateTime? denNgay, int? nhanVien, int page = 1)
        {
            ViewBag.Title = "Quản lý hóa đơn";
            
            var query = db.HoaDons.AsQueryable();
            
            if (tuNgay.HasValue)
            {
                query = query.Where(h => h.NgayBan >= tuNgay.Value);
                ViewBag.TuNgay = tuNgay.Value.ToString("yyyy-MM-dd");
            }
            
            if (denNgay.HasValue)
            {
                var endDate = denNgay.Value.AddDays(1);
                query = query.Where(h => h.NgayBan < endDate);
                ViewBag.DenNgay = denNgay.Value.ToString("yyyy-MM-dd");
            }
            
            if (nhanVien.HasValue)
            {
                query = query.Where(h => h.MaTK == nhanVien.Value);
                ViewBag.NhanVien = nhanVien;
            }
            
            int totalCount = query.Count();
            
            var hoaDons = query
                .OrderBy(h => h.MaHD)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .Select(h => new HoaDonViewModel
                {
                    MaHD = h.MaHD,
                    NgayBan = h.NgayBan,
                    MaTK = h.MaTK,
                    TongTien = h.TongTien,
                    TongGiamGia = h.TongGiamGia,
                    ThanhToan = h.ThanhToan,
                    GhiChu = h.GhiChu,
                    TenNhanVien = h.TaiKhoan.TenHienThi
                })
                .ToList();
            
            ViewBag.NhanVienList = new SelectList(db.TaiKhoans.OrderBy(t => t.TenHienThi), "MaTK", "TenHienThi", nhanVien);
            
            return View(new PagedList<HoaDonViewModel>(hoaDons, page, PageSize, totalCount));
        }

        // GET: Admin/HoaDon_65133373/Details/5
        public ActionResult Details(int id)
        {
            var hoaDon = db.HoaDons.Find(id);
            if (hoaDon == null)
            {
                return HttpNotFound();
            }
            
            var viewModel = new HoaDonViewModel
            {
                MaHD = hoaDon.MaHD,
                NgayBan = hoaDon.NgayBan,
                MaTK = hoaDon.MaTK,
                TongTien = hoaDon.TongTien,
                TongGiamGia = hoaDon.TongGiamGia,
                ThanhToan = hoaDon.ThanhToan,
                GhiChu = hoaDon.GhiChu,
                TenNhanVien = hoaDon.TaiKhoan.TenHienThi,
                ChiTietHoaDons = hoaDon.ChiTietHoaDons.Select(ct => new ChiTietHoaDonViewModel
                {
                    MaHD = ct.MaHD,
                    MaSP = ct.MaSP,
                    SoLuong = ct.SoLuong,
                    DonGia = ct.DonGia,
                    PhanTramGiam = ct.PhanTramGiam,
                    TienGiam = ct.TienGiam,
                    ThanhTien = ct.ThanhTien,
                    MaKM = ct.MaKM,
                    TenSP = ct.SanPham.TenSP,
                    DonViTinh = ct.SanPham.DonViTinh,
                    TenKM = ct.ChiTietKhuyenMai != null ? ct.ChiTietKhuyenMai.KhuyenMai.TenKM : null
                }).ToList()
            };
            
            ViewBag.Title = "Chi tiết hóa đơn";
            return View(viewModel);
        }

        // GET: Admin/HoaDon_65133373/Print/5
        public ActionResult Print(int id)
        {
            var hoaDon = db.HoaDons.Find(id);
            if (hoaDon == null)
            {
                return HttpNotFound();
            }
            
            var viewModel = new HoaDonViewModel
            {
                MaHD = hoaDon.MaHD,
                NgayBan = hoaDon.NgayBan,
                MaTK = hoaDon.MaTK,
                TongTien = hoaDon.TongTien,
                TongGiamGia = hoaDon.TongGiamGia,
                ThanhToan = hoaDon.ThanhToan,
                GhiChu = hoaDon.GhiChu,
                TenNhanVien = hoaDon.TaiKhoan.TenHienThi,
                ChiTietHoaDons = hoaDon.ChiTietHoaDons.Select(ct => new ChiTietHoaDonViewModel
                {
                    MaHD = ct.MaHD,
                    MaSP = ct.MaSP,
                    SoLuong = ct.SoLuong,
                    DonGia = ct.DonGia,
                    PhanTramGiam = ct.PhanTramGiam,
                    TienGiam = ct.TienGiam,
                    ThanhTien = ct.ThanhTien,
                    MaKM = ct.MaKM,
                    TenSP = ct.SanPham.TenSP,
                    DonViTinh = ct.SanPham.DonViTinh,
                    TenKM = ct.ChiTietKhuyenMai != null ? ct.ChiTietKhuyenMai.KhuyenMai.TenKM : null
                }).ToList()
            };
            
            return View(viewModel);
        }

        // GET: Admin/HoaDon_65133373/Create
        public ActionResult Create()
        {
            ViewBag.Title = "Tạo hóa đơn mới";
            return View(new TaoHoaDonViewModel());
        }

        // POST: Admin/HoaDon_65133373/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TaoHoaDonViewModel model)
        {
            if (model.ChiTietHoaDons == null || !model.ChiTietHoaDons.Any())
            {
                TempData["ErrorMessage"] = "Hóa đơn phải có ít nhất một sản phẩm!";
                return View(model);
            }
            
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var maTK = Convert.ToInt32(Session["MaTK"]);
                    var today = DateTime.Today;
                    
                    // Tạo hóa đơn
                    var hoaDon = new HoaDon
                    {
                        NgayBan = DateTime.Now,
                        MaTK = maTK,
                        TongTien = 0,
                        TongGiamGia = 0,
                        ThanhToan = 0,
                        GhiChu = model.GhiChu
                    };
                    
                    db.HoaDons.Add(hoaDon);
                    db.SaveChanges();
                    
                    decimal tongTien = 0;
                    decimal tongGiamGia = 0;
                    
                    foreach (var item in model.ChiTietHoaDons)
                    {
                        var sanPham = db.SanPhams.Find(item.MaSP);
                        if (sanPham == null)
                        {
                            throw new Exception($"Không tìm thấy sản phẩm với mã {item.MaSP}");
                        }
                        
                        if (sanPham.SoLuongTon < item.SoLuong)
                        {
                            throw new Exception($"Sản phẩm {sanPham.TenSP} không đủ số lượng (còn {sanPham.SoLuongTon})");
                        }
                        
                        // Kiểm tra khuyến mãi
                        decimal phanTramGiam = 0;
                        int? maKM = null;
                        
                        var khuyenMai = db.ChiTietKhuyenMais
                            .Where(km => km.MaSP == item.MaSP &&
                                         km.KhuyenMai.NgayBatDau <= today &&
                                         km.KhuyenMai.NgayKetThuc >= today)
                            .OrderByDescending(km => km.PhanTramGiam)
                            .FirstOrDefault();
                        
                        if (khuyenMai != null)
                        {
                            phanTramGiam = khuyenMai.PhanTramGiam;
                            maKM = khuyenMai.MaKM;
                        }
                        
                        decimal thanhTienChuaGiam = sanPham.DonGia * item.SoLuong;
                        decimal tienGiam = thanhTienChuaGiam * phanTramGiam / 100;
                        decimal thanhTien = thanhTienChuaGiam - tienGiam;
                        
                        var chiTiet = new ChiTietHoaDon
                        {
                            MaHD = hoaDon.MaHD,
                            MaSP = item.MaSP,
                            SoLuong = item.SoLuong,
                            DonGia = sanPham.DonGia,
                            PhanTramGiam = phanTramGiam,
                            TienGiam = tienGiam,
                            ThanhTien = thanhTien,
                            MaKM = maKM
                        };
                        
                        db.ChiTietHoaDons.Add(chiTiet);
                        
                        // Cập nhật số lượng tồn
                        sanPham.SoLuongTon -= item.SoLuong;
                        
                        tongTien += thanhTienChuaGiam;
                        tongGiamGia += tienGiam;
                    }
                    
                    // Cập nhật tổng hóa đơn
                    hoaDon.TongTien = tongTien;
                    hoaDon.TongGiamGia = tongGiamGia;
                    hoaDon.ThanhToan = tongTien - tongGiamGia;
                    
                    db.SaveChanges();
                    transaction.Commit();
                    
                    TempData["SuccessMessage"] = $"Tạo hóa đơn #{hoaDon.MaHD} thành công! Thành tiền: {hoaDon.ThanhToan:N0} đ";
                    return RedirectToAction("Details", new { id = hoaDon.MaHD });
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    TempData["ErrorMessage"] = "Lỗi: " + ex.Message;
                    return View(model);
                }
            }
        }

        // AJAX: Tìm kiếm sản phẩm
        public JsonResult SearchSanPham(string term)
        {
            var today = DateTime.Today;
            
            var results = db.SanPhams
                .Where(s => s.SoLuongTon > 0 && 
                           (s.MaSP.ToString().Contains(term) || s.TenSP.Contains(term)))
                .Take(10)
                .Select(s => new
                {
                    id = s.MaSP,
                    text = s.TenSP,
                    code = s.MaSP,
                    donGia = s.DonGia,
                    donViTinh = s.DonViTinh,
                    soLuongTon = s.SoLuongTon,
                    phanTramGiam = db.ChiTietKhuyenMais
                        .Where(km => km.MaSP == s.MaSP &&
                                     km.KhuyenMai.NgayBatDau <= today &&
                                     km.KhuyenMai.NgayKetThuc >= today)
                        .OrderByDescending(km => km.PhanTramGiam)
                        .Select(km => (decimal?)km.PhanTramGiam)
                        .FirstOrDefault() ?? 0
                })
                .ToList();
            
            return Json(results, JsonRequestBehavior.AllowGet);
        }

        // AJAX: Lấy thông tin sản phẩm
        public JsonResult GetSanPham(int id)
        {
            var today = DateTime.Today;
            
            var sanPham = db.SanPhams.Where(s => s.MaSP == id)
                .Select(s => new
                {
                    id = s.MaSP,
                    tenSP = s.TenSP,
                    donGia = s.DonGia,
                    donViTinh = s.DonViTinh,
                    soLuongTon = s.SoLuongTon,
                    phanTramGiam = db.ChiTietKhuyenMais
                        .Where(km => km.MaSP == s.MaSP &&
                                     km.KhuyenMai.NgayBatDau <= today &&
                                     km.KhuyenMai.NgayKetThuc >= today)
                        .OrderByDescending(km => km.PhanTramGiam)
                        .Select(km => (decimal?)km.PhanTramGiam)
                        .FirstOrDefault() ?? 0
                })
                .FirstOrDefault();
            
            if (sanPham == null)
            {
                return Json(new { success = false, message = "Không tìm thấy sản phẩm" }, JsonRequestBehavior.AllowGet);
            }
            
            return Json(new { success = true, data = sanPham }, JsonRequestBehavior.AllowGet);
        }

        // AJAX: Lấy danh sách sản phẩm
        public JsonResult GetSanPhamList()
        {
            var sanPhams = db.SanPhams
                .Where(s => s.SoLuongTon > 0)
                .OrderByDescending(s => s.MaSP)
                .Select(s => new
                {
                    maSP = s.MaSP,
                    tenSP = s.TenSP,
                    donGia = s.DonGia,
                    donViTinh = s.DonViTinh,
                    soLuongTon = s.SoLuongTon
                })
                .ToList();
            
            return Json(sanPhams, JsonRequestBehavior.AllowGet);
        }

        // AJAX: Lấy danh sách khuyến mãi hiện tại
        public JsonResult GetKhuyenMaiHienTai()
        {
            var today = DateTime.Today;
            
            var khuyenMais = db.KhuyenMais
                .Where(k => k.NgayBatDau <= today && k.NgayKetThuc >= today)
                .Select(k => new
                {
                    maKhuyenMai = k.MaKM,
                    tenKhuyenMai = k.TenKM,
                    chiTiet = k.ChiTietKhuyenMais.Select(ct => new
                    {
                        maSanPham = ct.MaSP,
                        phanTramGiam = ct.PhanTramGiam
                    }).ToList()
                })
                .ToList();
            
            return Json(khuyenMais, JsonRequestBehavior.AllowGet);
        }

        // POST: Admin/HoaDon_65133373/CreateApi (AJAX)
        [HttpPost]
        public JsonResult CreateApi(TaoHoaDonViewModel model)
        {
            if (model.ChiTiet == null || !model.ChiTiet.Any())
            {
                return Json(new { success = false, message = "Hóa đơn phải có ít nhất một sản phẩm!" });
            }
            
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var maTK = Convert.ToInt32(Session["MaTK"]);
                    var today = DateTime.Today;
                    
                    var hoaDon = new HoaDon
                    {
                        NgayBan = DateTime.Now,
                        MaTK = maTK,
                        TongTien = 0,
                        TongGiamGia = 0,
                        ThanhToan = 0,
                        GhiChu = model.GhiChu
                    };
                    
                    db.HoaDons.Add(hoaDon);
                    db.SaveChanges();
                    
                    decimal tongTien = 0;
                    decimal tongGiamGia = 0;
                    
                    foreach (var item in model.ChiTiet)
                    {
                        var sanPham = db.SanPhams.Find(item.MaSanPham);
                        if (sanPham == null)
                        {
                            throw new Exception($"Không tìm thấy sản phẩm với mã {item.MaSanPham}");
                        }
                        
                        if (sanPham.SoLuongTon < item.SoLuong)
                        {
                            throw new Exception($"Sản phẩm {sanPham.TenSP} không đủ số lượng (còn {sanPham.SoLuongTon})");
                        }
                        
                        // Kiểm tra khuyến mãi
                        decimal phanTramGiam = 0;
                        int? maKM = null;
                        
                        var khuyenMai = db.ChiTietKhuyenMais
                            .Where(km => km.MaSP == item.MaSanPham &&
                                         km.KhuyenMai.NgayBatDau <= today &&
                                         km.KhuyenMai.NgayKetThuc >= today)
                            .OrderByDescending(km => km.PhanTramGiam)
                            .FirstOrDefault();
                        
                        if (khuyenMai != null)
                        {
                            phanTramGiam = khuyenMai.PhanTramGiam;
                            maKM = khuyenMai.MaKM;
                        }
                        
                        decimal thanhTienChuaGiam = sanPham.DonGia * item.SoLuong;
                        decimal tienGiam = thanhTienChuaGiam * phanTramGiam / 100;
                        decimal thanhTien = thanhTienChuaGiam - tienGiam;
                        
                        var chiTiet = new ChiTietHoaDon
                        {
                            MaHD = hoaDon.MaHD,
                            MaSP = item.MaSanPham,
                            SoLuong = item.SoLuong,
                            DonGia = sanPham.DonGia,
                            PhanTramGiam = phanTramGiam,
                            TienGiam = tienGiam,
                            ThanhTien = thanhTien,
                            MaKM = maKM
                        };
                        
                        db.ChiTietHoaDons.Add(chiTiet);
                        sanPham.SoLuongTon -= item.SoLuong;
                        
                        tongTien += thanhTienChuaGiam;
                        tongGiamGia += tienGiam;
                    }
                    
                    hoaDon.TongTien = tongTien;
                    hoaDon.TongGiamGia = tongGiamGia;
                    hoaDon.ThanhToan = tongTien - tongGiamGia;
                    
                    db.SaveChanges();
                    transaction.Commit();
                    
                    return Json(new { success = true, maHD = hoaDon.MaHD, thanhToan = hoaDon.ThanhToan });
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return Json(new { success = false, message = ex.Message });
                }
            }
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
