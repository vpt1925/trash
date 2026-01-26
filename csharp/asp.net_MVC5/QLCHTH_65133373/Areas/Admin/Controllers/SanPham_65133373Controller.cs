using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLCHTH_65133373.Filters;
using QLCHTH_65133373.Models;
using QLCHTH_65133373.Models.ViewModels;

namespace QLCHTH_65133373.Areas.Admin.Controllers
{
    [AdminOnlyFilter_65133373]
    public class SanPham_65133373Controller : Controller
    {
        private QLCHTH_65133373Entities db = new QLCHTH_65133373Entities();
        private const int PageSize = 10;

        // GET: Admin/SanPham_65133373
        public ActionResult Index(string search, int? danhMuc, int? nhaCungCap, int page = 1)
        {
            ViewBag.Title = "Quản lý sản phẩm";
            
            var query = db.SanPhams.AsQueryable();
            
            // Tìm kiếm
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
            
            // Lọc theo danh mục
            if (danhMuc.HasValue)
            {
                query = query.Where(s => s.MaDanhMuc == danhMuc.Value);
                ViewBag.DanhMuc = danhMuc;
            }
            
            // Lọc theo nhà cung cấp
            if (nhaCungCap.HasValue)
            {
                query = query.Where(s => s.MaNCC == nhaCungCap.Value);
                ViewBag.NhaCungCap = nhaCungCap;
            }
            
            // Tổng số record
            int totalCount = query.Count();
            
            // Phân trang
            var sanPhams = query
                .OrderBy(s => s.MaSP)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .Select(s => new SanPhamViewModel
                {
                    MaSP = s.MaSP,
                    TenSP = s.TenSP,
                    MoTaSP = s.MoTaSP,
                    DonViTinh = s.DonViTinh,
                    DonGia = s.DonGia,
                    SoLuongTon = s.SoLuongTon,
                    AnhSanPham = s.AnhSanPham,
                    MaDanhMuc = s.MaDanhMuc,
                    MaNCC = s.MaNCC,
                    TenDanhMuc = s.DanhMuc.TenDanhMuc,
                    TenNCC = s.NhaCungCap.TenNCC
                })
                .ToList();
            
            var pagedList = new PagedList<SanPhamViewModel>(sanPhams, page, PageSize, totalCount);
            
            // Dropdown cho filter
            ViewBag.DanhMucs = new SelectList(db.DanhMucs.OrderBy(d => d.TenDanhMuc), "MaDanhMuc", "TenDanhMuc", danhMuc);
            ViewBag.NhaCungCaps = new SelectList(db.NhaCungCaps.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC", nhaCungCap);
            
            return View(pagedList);
        }

        // GET: Admin/SanPham_65133373/Details/5
        public ActionResult Details(int id)
        {
            var sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            
            var viewModel = new SanPhamViewModel
            {
                MaSP = sanPham.MaSP,
                TenSP = sanPham.TenSP,
                MoTaSP = sanPham.MoTaSP,
                DonViTinh = sanPham.DonViTinh,
                DonGia = sanPham.DonGia,
                SoLuongTon = sanPham.SoLuongTon,
                AnhSanPham = sanPham.AnhSanPham,
                MaDanhMuc = sanPham.MaDanhMuc,
                MaNCC = sanPham.MaNCC,
                TenDanhMuc = sanPham.DanhMuc.TenDanhMuc,
                TenNCC = sanPham.NhaCungCap.TenNCC
            };
            
            ViewBag.Title = "Chi tiết sản phẩm";
            return View(viewModel);
        }

        // GET: Admin/SanPham_65133373/Create
        public ActionResult Create()
        {
            ViewBag.Title = "Thêm sản phẩm mới";
            ViewBag.DanhMucs = new SelectList(db.DanhMucs.OrderBy(d => d.TenDanhMuc), "MaDanhMuc", "TenDanhMuc");
            ViewBag.NhaCungCaps = new SelectList(db.NhaCungCaps.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC");
            return View(new SanPhamViewModel());
        }

        // POST: Admin/SanPham_65133373/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SanPhamViewModel model, HttpPostedFileBase anhSanPham)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra tên sản phẩm trùng
                if (db.SanPhams.Any(s => s.TenSP == model.TenSP))
                {
                    ModelState.AddModelError("TenSP", "Tên sản phẩm đã tồn tại");
                    ViewBag.DanhMucs = new SelectList(db.DanhMucs.OrderBy(d => d.TenDanhMuc), "MaDanhMuc", "TenDanhMuc", model.MaDanhMuc);
                    ViewBag.NhaCungCaps = new SelectList(db.NhaCungCaps.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC", model.MaNCC);
                    return View(model);
                }
                
                var sanPham = new SanPham
                {
                    TenSP = model.TenSP,
                    MoTaSP = model.MoTaSP,
                    DonViTinh = model.DonViTinh,
                    DonGia = model.DonGia,
                    SoLuongTon = model.SoLuongTon,
                    MaDanhMuc = model.MaDanhMuc,
                    MaNCC = model.MaNCC
                };
                
                // Upload ảnh
                if (anhSanPham != null && anhSanPham.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(anhSanPham.FileName);
                    var extension = Path.GetExtension(fileName).ToLower();
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                    
                    if (allowedExtensions.Contains(extension))
                    {
                        var newFileName = Guid.NewGuid().ToString() + extension;
                        var path = Path.Combine(Server.MapPath("~/Content/images/products"), newFileName);
                        
                        // Tạo thư mục nếu chưa tồn tại
                        var directory = Path.GetDirectoryName(path);
                        if (!Directory.Exists(directory))
                        {
                            Directory.CreateDirectory(directory);
                        }
                        
                        anhSanPham.SaveAs(path);
                        sanPham.AnhSanPham = newFileName;
                    }
                }
                
                db.SanPhams.Add(sanPham);
                db.SaveChanges();
                
                TempData["SuccessMessage"] = "Thêm sản phẩm thành công!";
                return RedirectToAction("Index");
            }
            
            ViewBag.Title = "Thêm sản phẩm mới";
            ViewBag.DanhMucs = new SelectList(db.DanhMucs.OrderBy(d => d.TenDanhMuc), "MaDanhMuc", "TenDanhMuc", model.MaDanhMuc);
            ViewBag.NhaCungCaps = new SelectList(db.NhaCungCaps.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC", model.MaNCC);
            return View(model);
        }

        // GET: Admin/SanPham_65133373/Edit/5
        public ActionResult Edit(int id)
        {
            var sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            
            var viewModel = new SanPhamViewModel
            {
                MaSP = sanPham.MaSP,
                TenSP = sanPham.TenSP,
                MoTaSP = sanPham.MoTaSP,
                DonViTinh = sanPham.DonViTinh,
                DonGia = sanPham.DonGia,
                SoLuongTon = sanPham.SoLuongTon,
                AnhSanPham = sanPham.AnhSanPham,
                MaDanhMuc = sanPham.MaDanhMuc,
                MaNCC = sanPham.MaNCC
            };
            
            ViewBag.Title = "Chỉnh sửa sản phẩm";
            ViewBag.DanhMucs = new SelectList(db.DanhMucs.OrderBy(d => d.TenDanhMuc), "MaDanhMuc", "TenDanhMuc", sanPham.MaDanhMuc);
            ViewBag.NhaCungCaps = new SelectList(db.NhaCungCaps.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC", sanPham.MaNCC);
            return View(viewModel);
        }

        // POST: Admin/SanPham_65133373/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SanPhamViewModel model, HttpPostedFileBase anhSanPham)
        {
            if (ModelState.IsValid)
            {
                var sanPham = db.SanPhams.Find(model.MaSP);
                if (sanPham == null)
                {
                    return HttpNotFound();
                }
                
                // Kiểm tra tên sản phẩm trùng (ngoại trừ bản thân)
                if (db.SanPhams.Any(s => s.TenSP == model.TenSP && s.MaSP != model.MaSP))
                {
                    ModelState.AddModelError("TenSP", "Tên sản phẩm đã tồn tại");
                    ViewBag.DanhMucs = new SelectList(db.DanhMucs.OrderBy(d => d.TenDanhMuc), "MaDanhMuc", "TenDanhMuc", model.MaDanhMuc);
                    ViewBag.NhaCungCaps = new SelectList(db.NhaCungCaps.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC", model.MaNCC);
                    return View(model);
                }
                
                sanPham.TenSP = model.TenSP;
                sanPham.MoTaSP = model.MoTaSP;
                sanPham.DonViTinh = model.DonViTinh;
                sanPham.DonGia = model.DonGia;
                sanPham.SoLuongTon = model.SoLuongTon;
                sanPham.MaDanhMuc = model.MaDanhMuc;
                sanPham.MaNCC = model.MaNCC;
                
                // Upload ảnh mới
                if (anhSanPham != null && anhSanPham.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(anhSanPham.FileName);
                    var extension = Path.GetExtension(fileName).ToLower();
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                    
                    if (allowedExtensions.Contains(extension))
                    {
                        // Xóa ảnh cũ
                        if (!string.IsNullOrEmpty(sanPham.AnhSanPham))
                        {
                            var oldPath = Path.Combine(Server.MapPath("~/Content/images/products"), sanPham.AnhSanPham);
                            if (System.IO.File.Exists(oldPath))
                            {
                                System.IO.File.Delete(oldPath);
                            }
                        }
                        
                        var newFileName = Guid.NewGuid().ToString() + extension;
                        var path = Path.Combine(Server.MapPath("~/Content/images/products"), newFileName);
                        
                        var directory = Path.GetDirectoryName(path);
                        if (!Directory.Exists(directory))
                        {
                            Directory.CreateDirectory(directory);
                        }
                        
                        anhSanPham.SaveAs(path);
                        sanPham.AnhSanPham = newFileName;
                    }
                }
                
                db.Entry(sanPham).State = EntityState.Modified;
                db.SaveChanges();
                
                TempData["SuccessMessage"] = "Cập nhật sản phẩm thành công!";
                return RedirectToAction("Index");
            }
            
            ViewBag.Title = "Chỉnh sửa sản phẩm";
            ViewBag.DanhMucs = new SelectList(db.DanhMucs.OrderBy(d => d.TenDanhMuc), "MaDanhMuc", "TenDanhMuc", model.MaDanhMuc);
            ViewBag.NhaCungCaps = new SelectList(db.NhaCungCaps.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC", model.MaNCC);
            return View(model);
        }

        // POST: Admin/SanPham_65133373/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            
            // Kiểm tra có chi tiết hóa đơn không
            if (sanPham.ChiTietHoaDons.Any())
            {
                TempData["ErrorMessage"] = "Không thể xóa sản phẩm đã có trong hóa đơn!";
                return RedirectToAction("Index");
            }
            
            // Kiểm tra có chi tiết khuyến mãi không
            if (sanPham.ChiTietKhuyenMais.Any())
            {
                TempData["ErrorMessage"] = "Không thể xóa sản phẩm đã có trong chương trình khuyến mãi!";
                return RedirectToAction("Index");
            }
            
            // Xóa ảnh
            if (!string.IsNullOrEmpty(sanPham.AnhSanPham))
            {
                var path = Path.Combine(Server.MapPath("~/Content/images/products"), sanPham.AnhSanPham);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
            }
            
            db.SanPhams.Remove(sanPham);
            db.SaveChanges();
            
            TempData["SuccessMessage"] = "Xóa sản phẩm thành công!";
            return RedirectToAction("Index");
        }

        // AJAX: Tìm kiếm danh mục
        public JsonResult SearchDanhMuc(string term)
        {
            var results = db.DanhMucs
                .Where(d => d.MaDanhMuc.ToString().Contains(term) || d.TenDanhMuc.Contains(term))
                .Take(10)
                .Select(d => new { id = d.MaDanhMuc, text = d.TenDanhMuc, code = d.MaDanhMuc })
                .ToList();
            return Json(results, JsonRequestBehavior.AllowGet);
        }

        // AJAX: Tìm kiếm nhà cung cấp
        public JsonResult SearchNhaCungCap(string term)
        {
            var results = db.NhaCungCaps
                .Where(n => n.MaNCC.ToString().Contains(term) || n.TenNCC.Contains(term))
                .Take(10)
                .Select(n => new { id = n.MaNCC, text = n.TenNCC, code = n.MaNCC })
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
