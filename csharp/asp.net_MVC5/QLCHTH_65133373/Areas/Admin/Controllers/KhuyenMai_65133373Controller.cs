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
    public class KhuyenMai_65133373Controller : Controller
    {
        private QLCHTH_65133373Entities db = new QLCHTH_65133373Entities();
        private const int PageSize = 10;

        // GET: Admin/KhuyenMai_65133373
        public ActionResult Index(string trangThai, int page = 1)
        {
            ViewBag.Title = "Quản lý khuyến mãi";
            
            var today = DateTime.Today;
            var query = db.KhuyenMais.AsQueryable();
            
            if (!string.IsNullOrEmpty(trangThai))
            {
                switch (trangThai)
                {
                    case "chuabatdau":
                        query = query.Where(k => k.NgayBatDau > today);
                        break;
                    case "dangdienra":
                        query = query.Where(k => k.NgayBatDau <= today && k.NgayKetThuc >= today);
                        break;
                    case "daketthuc":
                        query = query.Where(k => k.NgayKetThuc < today);
                        break;
                }
                ViewBag.TrangThai = trangThai;
            }
            
            int totalCount = query.Count();
            
            var khuyenMais = query
                .OrderBy(k => k.MaKM)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .Select(k => new KhuyenMaiViewModel
                {
                    MaKM = k.MaKM,
                    TenKM = k.TenKM,
                    NgayBatDau = k.NgayBatDau,
                    NgayKetThuc = k.NgayKetThuc,
                    MoTa = k.MoTa,
                    SoSanPham = k.ChiTietKhuyenMais.Count()
                })
                .ToList();
            
            return View(new PagedList<KhuyenMaiViewModel>(khuyenMais, page, PageSize, totalCount));
        }

        // GET: Admin/KhuyenMai_65133373/Details/5
        public ActionResult Details(int id)
        {
            var khuyenMai = db.KhuyenMais.Find(id);
            if (khuyenMai == null)
            {
                return HttpNotFound();
            }
            
            var viewModel = new KhuyenMaiViewModel
            {
                MaKM = khuyenMai.MaKM,
                TenKM = khuyenMai.TenKM,
                NgayBatDau = khuyenMai.NgayBatDau,
                NgayKetThuc = khuyenMai.NgayKetThuc,
                MoTa = khuyenMai.MoTa,
                ChiTietKhuyenMais = khuyenMai.ChiTietKhuyenMais.Select(ct => new ChiTietKhuyenMaiViewModel
                {
                    MaKM = ct.MaKM,
                    MaSP = ct.MaSP,
                    PhanTramGiam = ct.PhanTramGiam,
                    TenSP = ct.SanPham.TenSP,
                    DonGia = ct.SanPham.DonGia,
                    DonViTinh = ct.SanPham.DonViTinh
                }).ToList()
            };
            
            ViewBag.Title = "Chi tiết khuyến mãi";
            return View(viewModel);
        }

        // GET: Admin/KhuyenMai_65133373/Create
        public ActionResult Create()
        {
            ViewBag.Title = "Tạo khuyến mãi mới";
            ViewBag.SanPhamList = db.SanPhams.OrderBy(s => s.TenSP).ToList();
            var model = new TaoKhuyenMaiViewModel
            {
                NgayBatDau = DateTime.Today,
                NgayKetThuc = DateTime.Today.AddDays(7)
            };
            return View(model);
        }

        // POST: Admin/KhuyenMai_65133373/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TaoKhuyenMaiViewModel model)
        {
            if (model.ChiTietKhuyenMais == null || !model.ChiTietKhuyenMais.Any())
            {
                TempData["ErrorMessage"] = "Khuyến mãi phải có ít nhất một sản phẩm!";
                ViewBag.SanPhamList = db.SanPhams.OrderBy(s => s.TenSP).ToList();
                return View(model);
            }
            
            if (model.NgayKetThuc < model.NgayBatDau)
            {
                ModelState.AddModelError("NgayKetThuc", "Ngày kết thúc phải sau hoặc bằng ngày bắt đầu");
                ViewBag.SanPhamList = db.SanPhams.OrderBy(s => s.TenSP).ToList();
                return View(model);
            }
            
            if (ModelState.IsValid)
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var khuyenMai = new KhuyenMai
                        {
                            TenKM = model.TenKM,
                            NgayBatDau = model.NgayBatDau,
                            NgayKetThuc = model.NgayKetThuc,
                            MoTa = model.MoTa
                        };
                        
                        db.KhuyenMais.Add(khuyenMai);
                        db.SaveChanges();
                        
                        foreach (var item in model.ChiTietKhuyenMais)
                        {
                            // Kiểm tra sản phẩm đã có trong khuyến mãi khác trong cùng thời gian
                            var existingPromo = db.ChiTietKhuyenMais
                                .Where(c => c.MaSP == item.MaSP &&
                                           c.KhuyenMai.MaKM != khuyenMai.MaKM &&
                                           ((model.NgayBatDau >= c.KhuyenMai.NgayBatDau && model.NgayBatDau <= c.KhuyenMai.NgayKetThuc) ||
                                            (model.NgayKetThuc >= c.KhuyenMai.NgayBatDau && model.NgayKetThuc <= c.KhuyenMai.NgayKetThuc) ||
                                            (model.NgayBatDau <= c.KhuyenMai.NgayBatDau && model.NgayKetThuc >= c.KhuyenMai.NgayKetThuc)))
                                .FirstOrDefault();
                            
                            if (existingPromo != null)
                            {
                                var sp = db.SanPhams.Find(item.MaSP);
                                throw new Exception($"Sản phẩm {sp?.TenSP} đã có trong khuyến mãi khác trong thời gian này");
                            }
                            
                            var chiTiet = new ChiTietKhuyenMai
                            {
                                MaKM = khuyenMai.MaKM,
                                MaSP = item.MaSP,
                                PhanTramGiam = item.PhanTramGiam
                            };
                            
                            db.ChiTietKhuyenMais.Add(chiTiet);
                        }
                        
                        db.SaveChanges();
                        transaction.Commit();
                        
                        TempData["SuccessMessage"] = "Tạo khuyến mãi thành công!";
                        return RedirectToAction("Details", new { id = khuyenMai.MaKM });
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        TempData["ErrorMessage"] = "Lỗi: " + ex.Message;
                        ViewBag.SanPhamList = db.SanPhams.OrderBy(s => s.TenSP).ToList();
                        return View(model);
                    }
                }
            }
            
            ViewBag.Title = "Tạo khuyến mãi mới";
            ViewBag.SanPhamList = db.SanPhams.OrderBy(s => s.TenSP).ToList();
            return View(model);
        }

        // GET: Admin/KhuyenMai_65133373/Edit/5
        public ActionResult Edit(int id)
        {
            var khuyenMai = db.KhuyenMais.Find(id);
            if (khuyenMai == null)
            {
                return HttpNotFound();
            }
            
            var today = DateTime.Today;
            
            // Không cho phép sửa khuyến mãi đã kết thúc
            if (khuyenMai.NgayKetThuc < today)
            {
                TempData["ErrorMessage"] = "Không thể sửa khuyến mãi đã kết thúc!";
                return RedirectToAction("Index");
            }
            
            var viewModel = new KhuyenMaiViewModel
            {
                MaKM = khuyenMai.MaKM,
                TenKM = khuyenMai.TenKM,
                NgayBatDau = khuyenMai.NgayBatDau,
                NgayKetThuc = khuyenMai.NgayKetThuc,
                MoTa = khuyenMai.MoTa,
                ChiTietKhuyenMais = khuyenMai.ChiTietKhuyenMais.Select(ct => new ChiTietKhuyenMaiViewModel
                {
                    MaKM = ct.MaKM,
                    MaSP = ct.MaSP,
                    PhanTramGiam = ct.PhanTramGiam,
                    TenSP = ct.SanPham.TenSP,
                    DonGia = ct.SanPham.DonGia,
                    DonViTinh = ct.SanPham.DonViTinh
                }).ToList()
            };
            
            // Xác định có thể sửa những gì
            ViewBag.ChuaBatDau = khuyenMai.NgayBatDau > today;
            ViewBag.DangDienRa = khuyenMai.NgayBatDau <= today && khuyenMai.NgayKetThuc >= today;
            
            ViewBag.Title = "Chỉnh sửa khuyến mãi";
            return View(viewModel);
        }

        // POST: Admin/KhuyenMai_65133373/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(KhuyenMaiViewModel model)
        {
            var khuyenMai = db.KhuyenMais.Find(model.MaKM);
            if (khuyenMai == null)
            {
                return HttpNotFound();
            }
            
            var today = DateTime.Today;
            
            // Không cho phép sửa khuyến mãi đã kết thúc
            if (khuyenMai.NgayKetThuc < today)
            {
                TempData["ErrorMessage"] = "Không thể sửa khuyến mãi đã kết thúc!";
                return RedirectToAction("Index");
            }
            
            bool chuaBatDau = khuyenMai.NgayBatDau > today;
            bool dangDienRa = khuyenMai.NgayBatDau <= today && khuyenMai.NgayKetThuc >= today;
            
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    if (chuaBatDau)
                    {
                        // Chưa bắt đầu -> sửa được tất cả
                        khuyenMai.TenKM = model.TenKM;
                        khuyenMai.NgayBatDau = model.NgayBatDau;
                        khuyenMai.NgayKetThuc = model.NgayKetThuc;
                        khuyenMai.MoTa = model.MoTa;
                        
                        if (model.NgayKetThuc < model.NgayBatDau)
                        {
                            throw new Exception("Ngày kết thúc phải sau hoặc bằng ngày bắt đầu");
                        }
                        
                        // Xóa chi tiết cũ và thêm mới
                        if (model.ChiTietKhuyenMais != null && model.ChiTietKhuyenMais.Any())
                        {
                            db.ChiTietKhuyenMais.RemoveRange(khuyenMai.ChiTietKhuyenMais);
                            
                            foreach (var item in model.ChiTietKhuyenMais)
                            {
                                var chiTiet = new ChiTietKhuyenMai
                                {
                                    MaKM = khuyenMai.MaKM,
                                    MaSP = item.MaSP,
                                    PhanTramGiam = item.PhanTramGiam
                                };
                                db.ChiTietKhuyenMais.Add(chiTiet);
                            }
                        }
                    }
                    else if (dangDienRa)
                    {
                        // Đang diễn ra -> chỉ sửa được ngày kết thúc
                        if (model.NgayKetThuc < today)
                        {
                            throw new Exception("Ngày kết thúc không thể trước ngày hôm nay");
                        }
                        khuyenMai.NgayKetThuc = model.NgayKetThuc;
                    }
                    
                    db.Entry(khuyenMai).State = EntityState.Modified;
                    db.SaveChanges();
                    transaction.Commit();
                    
                    TempData["SuccessMessage"] = "Cập nhật khuyến mãi thành công!";
                    return RedirectToAction("Details", new { id = khuyenMai.MaKM });
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    TempData["ErrorMessage"] = "Lỗi: " + ex.Message;
                    
                    ViewBag.ChuaBatDau = chuaBatDau;
                    ViewBag.DangDienRa = dangDienRa;
                    return View(model);
                }
            }
        }

        // POST: Admin/KhuyenMai_65133373/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var khuyenMai = db.KhuyenMais.Find(id);
            if (khuyenMai == null)
            {
                return HttpNotFound();
            }
            
            var today = DateTime.Today;
            
            // Chỉ cho phép xóa khuyến mãi chưa bắt đầu
            if (khuyenMai.NgayBatDau <= today)
            {
                TempData["ErrorMessage"] = "Chỉ có thể xóa khuyến mãi chưa bắt đầu!";
                return RedirectToAction("Index");
            }
            
            // Chi tiết sẽ bị xóa theo cascade
            db.KhuyenMais.Remove(khuyenMai);
            db.SaveChanges();
            
            TempData["SuccessMessage"] = "Xóa khuyến mãi thành công!";
            return RedirectToAction("Index");
        }

        // AJAX: Tìm kiếm sản phẩm cho khuyến mãi
        public JsonResult SearchSanPham(string term)
        {
            var results = db.SanPhams
                .Where(s => s.MaSP.ToString().Contains(term) || s.TenSP.Contains(term))
                .Take(10)
                .Select(s => new
                {
                    id = s.MaSP,
                    text = s.TenSP,
                    code = s.MaSP,
                    donGia = s.DonGia,
                    donViTinh = s.DonViTinh
                })
                .ToList();
            
            return Json(results, JsonRequestBehavior.AllowGet);
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
