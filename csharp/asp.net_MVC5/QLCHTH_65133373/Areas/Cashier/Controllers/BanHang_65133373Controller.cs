using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using QLCHTH_65133373.Filters;
using QLCHTH_65133373.Helpers;
using QLCHTH_65133373.Models;
using QLCHTH_65133373.Models.ViewModels;

namespace QLCHTH_65133373.Areas.Cashier.Controllers
{
    [StaffFilter_65133373]
    public class BanHang_65133373Controller : Controller
    {
        private QLCHTH_65133373Entities db = new QLCHTH_65133373Entities();
        private const int PageSize = 10;

        // GET: Cashier/BanHang_65133373
        public ActionResult Index()
        {
            ViewBag.Title = "Bán hàng";
            return View(new TaoHoaDonViewModel());
        }

        // POST: Cashier/BanHang_65133373/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TaoHoaDonViewModel model)
        {
            if (model.ChiTietHoaDons == null || !model.ChiTietHoaDons.Any())
            {
                TempData["ErrorMessage"] = "Hóa đơn phải có ít nhất một sản phẩm!";
                return RedirectToAction("Index");
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
                    return RedirectToAction("ChiTietHoaDon", new { id = hoaDon.MaHD });
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    TempData["ErrorMessage"] = "Lỗi: " + ex.Message;
                    return RedirectToAction("Index");
                }
            }
        }

        // GET: Cashier/BanHang_65133373/LichSuHoaDon
        public ActionResult LichSuHoaDon(DateTime? ngay, bool? all, int page = 1)
        {
            ViewBag.Title = "Lịch sử hóa đơn";
            
            var maTK = Convert.ToInt32(Session["MaTK"]);
            var loaiTK = Convert.ToInt32(Session["LoaiTaiKhoan"]);
            
            var query = db.HoaDons.AsQueryable();
            
            // Thu ngân chỉ xem hóa đơn của mình
            if (loaiTK == 1)
            {
                query = query.Where(h => h.MaTK == maTK);
            }
            
            if (ngay.HasValue)
            {
                query = query.Where(h => DbFunctions.TruncateTime(h.NgayBan) == ngay.Value.Date);
                ViewBag.Ngay = ngay.Value.ToString("yyyy-MM-dd");
            }
            else
            {
                // Hiển thị tất cả hóa đơn, không lọc theo ngày
                ViewBag.Ngay = "";
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
            
            // Tổng doanh thu trong ngày
            ViewBag.TongDoanhThu = query.Sum(h => (decimal?)h.ThanhToan) ?? 0;
            ViewBag.SoHoaDon = totalCount;
            
            return View(new PagedList<HoaDonViewModel>(hoaDons, page, PageSize, totalCount));
        }

        // GET: Cashier/BanHang_65133373/ChiTietHoaDon/5
        public ActionResult ChiTietHoaDon(int id)
        {
            var hoaDon = db.HoaDons.Find(id);
            if (hoaDon == null)
            {
                return HttpNotFound();
            }
            
            // Thu ngân chỉ xem hóa đơn của mình
            var maTK = Convert.ToInt32(Session["MaTK"]);
            var loaiTK = Convert.ToInt32(Session["LoaiTaiKhoan"]);
            if (loaiTK == 1 && hoaDon.MaTK != maTK)
            {
                TempData["ErrorMessage"] = "Bạn không có quyền xem hóa đơn này!";
                return RedirectToAction("LichSuHoaDon");
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

        // GET: Cashier/BanHang_65133373/Print/5
        public ActionResult Print(int id)
        {
            var hoaDon = db.HoaDons.Find(id);
            if (hoaDon == null)
            {
                return HttpNotFound();
            }
            
            // Thu ngân chỉ in hóa đơn của mình
            var maTK = Convert.ToInt32(Session["MaTK"]);
            var loaiTK = Convert.ToInt32(Session["LoaiTaiKhoan"]);
            if (loaiTK == 1 && hoaDon.MaTK != maTK)
            {
                TempData["ErrorMessage"] = "Bạn không có quyền in hóa đơn này!";
                return RedirectToAction("LichSuHoaDon");
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

        // GET: Cashier/BanHang_65133373/TraCuuSanPham
        public ActionResult TraCuuSanPham(string search, int? danhMuc, int page = 1)
        {
            ViewBag.Title = "Tra cứu sản phẩm";
            
            var today = DateTime.Today;
            var query = db.SanPhams.AsQueryable();
            
            if (!string.IsNullOrWhiteSpace(search))
            {
                int searchId;
                if (int.TryParse(search, out searchId))
                {
                    query = query.Where(s => s.MaSP == searchId || s.TenSP.Contains(search));
                }
                else
                {
                    query = query.Where(s => s.TenSP.Contains(search));
                }
                ViewBag.Search = search;
            }
            
            if (danhMuc.HasValue)
            {
                query = query.Where(s => s.MaDanhMuc == danhMuc.Value);
                ViewBag.DanhMuc = danhMuc;
            }
            
            int totalCount = query.Count();
            
            var sanPhams = query
                .OrderBy(s => s.TenSP)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .Select(s => new SanPhamViewModel
                {
                    MaSP = s.MaSP,
                    TenSP = s.TenSP,
                    DonViTinh = s.DonViTinh,
                    DonGia = s.DonGia,
                    SoLuongTon = s.SoLuongTon,
                    TenDanhMuc = s.DanhMuc.TenDanhMuc
                })
                .ToList();
            
            ViewBag.DanhMucs = new SelectList(db.DanhMucs.OrderBy(d => d.TenDanhMuc), "MaDanhMuc", "TenDanhMuc", danhMuc);
            
            return View(new PagedList<SanPhamViewModel>(sanPhams, page, PageSize, totalCount));
        }

        // GET: Cashier/BanHang_65133373/KhuyenMaiHienTai
        public ActionResult KhuyenMaiHienTai()
        {
            ViewBag.Title = "Khuyến mãi hiện tại";
            
            var today = DateTime.Today;
            
            var khuyenMais = db.KhuyenMais
                .Where(k => k.NgayBatDau <= today && k.NgayKetThuc >= today)
                .OrderBy(k => k.NgayKetThuc)
                .Select(k => new KhuyenMaiViewModel
                {
                    MaKM = k.MaKM,
                    TenKM = k.TenKM,
                    NgayBatDau = k.NgayBatDau,
                    NgayKetThuc = k.NgayKetThuc,
                    MoTa = k.MoTa,
                    SoSanPham = k.ChiTietKhuyenMais.Count(),
                    ChiTietKhuyenMais = k.ChiTietKhuyenMais.Select(ct => new ChiTietKhuyenMaiViewModel
                    {
                        MaSP = ct.MaSP,
                        TenSP = ct.SanPham.TenSP,
                        DonGia = ct.SanPham.DonGia,
                        PhanTramGiam = ct.PhanTramGiam,
                        DonViTinh = ct.SanPham.DonViTinh
                    }).ToList()
                })
                .ToList();
            
            return View(khuyenMais);
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

        // AJAX: Lấy danh sách sản phẩm (cho BanHang Index)
        public JsonResult GetSanPham()
        {
            var today = DateTime.Today;
            
            var sanPhams = db.SanPhams
                .OrderBy(s => s.TenSP)
                .Select(s => new
                {
                    maSanPham = s.MaSP,
                    tenSanPham = s.TenSP,
                    donGia = s.DonGia,
                    donViTinh = s.DonViTinh,
                    soLuongTon = s.SoLuongTon,
                    maDanhMuc = s.MaDanhMuc,
                    tenDanhMuc = s.DanhMuc.TenDanhMuc
                })
                .ToList();
            
            return Json(sanPhams, JsonRequestBehavior.AllowGet);
        }

        // AJAX: Lấy thông tin sản phẩm theo ID
        public JsonResult GetSanPhamById(int id)
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

        // AJAX: Lấy danh sách khuyến mãi hiện tại
        public JsonResult GetKhuyenMaiHienTai()
        {
            var today = DateTime.Today;
            
            var khuyenMais = db.KhuyenMais
                .Where(k => k.NgayBatDau <= today && k.NgayKetThuc >= today)
                .OrderBy(k => k.NgayKetThuc)
                .Select(k => new
                {
                    maKhuyenMai = k.MaKM,
                    tenKhuyenMai = k.TenKM,
                    ngayBatDau = k.NgayBatDau.ToString(),
                    ngayKetThuc = k.NgayKetThuc.ToString(),
                    moTa = k.MoTa,
                    chiTiet = k.ChiTietKhuyenMais.Select(ct => new
                    {
                        maSanPham = ct.MaSP,
                        tenSanPham = ct.SanPham.TenSP,
                        phanTramGiam = ct.PhanTramGiam,
                        giaGoc = ct.SanPham.DonGia
                    }).ToList()
                })
                .ToList();
            
            return Json(khuyenMais, JsonRequestBehavior.AllowGet);
        }

        // AJAX: Thanh toán hóa đơn
        [HttpPost]
        public JsonResult ThanhToan(TaoHoaDonViewModel model)
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
                    
                    // Tạo hóa đơn
                    var hoaDon = new HoaDon
                    {
                        NgayBan = DateTime.Now,
                        MaTK = maTK,
                        TongTien = 0,
                        TongGiamGia = 0,
                        ThanhToan = 0,
                        GhiChu = ""
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
                        
                        decimal thanhTienChuaGiam = item.DonGia * item.SoLuong;
                        decimal tienGiam = thanhTienChuaGiam * phanTramGiam / 100;
                        decimal thanhTien = thanhTienChuaGiam - tienGiam;
                        
                        var chiTiet = new ChiTietHoaDon
                        {
                            MaHD = hoaDon.MaHD,
                            MaSP = item.MaSanPham,
                            SoLuong = item.SoLuong,
                            DonGia = item.DonGia,
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
                    
                    return Json(new { success = true, maHoaDon = hoaDon.MaHD, thanhTien = hoaDon.ThanhToan });
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
