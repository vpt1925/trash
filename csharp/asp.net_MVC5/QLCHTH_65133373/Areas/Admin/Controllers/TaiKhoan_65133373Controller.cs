using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using QLCHTH_65133373.Filters;
using QLCHTH_65133373.Helpers;
using QLCHTH_65133373.Models;
using QLCHTH_65133373.Models.ViewModels;

namespace QLCHTH_65133373.Areas.Admin.Controllers
{
    [AdminOnlyFilter_65133373]
    public class TaiKhoan_65133373Controller : Controller
    {
        private QLCHTH_65133373Entities db = new QLCHTH_65133373Entities();
        private const int PageSize = 10;

        // GET: Admin/TaiKhoan_65133373
        public ActionResult Index(string search, int? loaiTaiKhoan, int page = 1)
        {
            ViewBag.Title = "Quản lý tài khoản";
            
            var query = db.TaiKhoans.AsQueryable();
            
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(t => t.TenDangNhap.Contains(search) || t.TenHienThi.Contains(search));
                ViewBag.Search = search;
            }
            
            if (loaiTaiKhoan.HasValue)
            {
                query = query.Where(t => t.LoaiTaiKhoan == loaiTaiKhoan.Value);
                ViewBag.LoaiTaiKhoan = loaiTaiKhoan;
            }
            
            int totalCount = query.Count();
            
            var taiKhoans = query
                .OrderBy(t => t.MaTK)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .Select(t => new TaiKhoanViewModel
                {
                    MaTK = t.MaTK,
                    TenDangNhap = t.TenDangNhap,
                    LoaiTaiKhoan = t.LoaiTaiKhoan,
                    TenHienThi = t.TenHienThi,
                    NgayTao = t.NgayTao,
                    GhiChu = t.GhiChu
                })
                .ToList();
            
            return View(new PagedList<TaiKhoanViewModel>(taiKhoans, page, PageSize, totalCount));
        }

        // GET: Admin/TaiKhoan_65133373/Create
        public ActionResult Create()
        {
            ViewBag.Title = "Thêm tài khoản mới";
            return View(new TaiKhoanViewModel());
        }

        // POST: Admin/TaiKhoan_65133373/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TaiKhoanViewModel model)
        {
            if (string.IsNullOrEmpty(model.MatKhau))
            {
                ModelState.AddModelError("MatKhau", "Vui lòng nhập mật khẩu");
            }
            
            if (ModelState.IsValid)
            {
                if (db.TaiKhoans.Any(t => t.TenDangNhap == model.TenDangNhap))
                {
                    ModelState.AddModelError("TenDangNhap", "Tên đăng nhập đã tồn tại");
                    return View(model);
                }
                
                var taiKhoan = new TaiKhoan
                {
                    TenDangNhap = model.TenDangNhap,
                    MatKhau = SecurityHelper.HashPassword(model.MatKhau),
                    LoaiTaiKhoan = model.LoaiTaiKhoan,
                    TenHienThi = model.TenHienThi,
                    NgayTao = DateTime.Now,
                    GhiChu = model.GhiChu
                };
                
                db.TaiKhoans.Add(taiKhoan);
                db.SaveChanges();
                
                TempData["SuccessMessage"] = "Thêm tài khoản thành công!";
                return RedirectToAction("Index");
            }
            
            ViewBag.Title = "Thêm tài khoản mới";
            return View(model);
        }

        // GET: Admin/TaiKhoan_65133373/Edit/5
        public ActionResult Edit(int id)
        {
            var taiKhoan = db.TaiKhoans.Find(id);
            if (taiKhoan == null)
            {
                return HttpNotFound();
            }
            
            var viewModel = new TaiKhoanViewModel
            {
                MaTK = taiKhoan.MaTK,
                TenDangNhap = taiKhoan.TenDangNhap,
                LoaiTaiKhoan = taiKhoan.LoaiTaiKhoan,
                TenHienThi = taiKhoan.TenHienThi,
                NgayTao = taiKhoan.NgayTao,
                GhiChu = taiKhoan.GhiChu
            };
            
            ViewBag.Title = "Chỉnh sửa tài khoản";
            return View(viewModel);
        }

        // POST: Admin/TaiKhoan_65133373/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TaiKhoanViewModel model)
        {
            // Bỏ qua validation mật khẩu khi edit (không bắt buộc thay đổi)
            ModelState.Remove("MatKhau");
            ModelState.Remove("XacNhanMatKhau");
            
            if (ModelState.IsValid)
            {
                var taiKhoan = db.TaiKhoans.Find(model.MaTK);
                if (taiKhoan == null)
                {
                    return HttpNotFound();
                }
                
                if (db.TaiKhoans.Any(t => t.TenDangNhap == model.TenDangNhap && t.MaTK != model.MaTK))
                {
                    ModelState.AddModelError("TenDangNhap", "Tên đăng nhập đã tồn tại");
                    return View(model);
                }
                
                taiKhoan.TenDangNhap = model.TenDangNhap;
                taiKhoan.LoaiTaiKhoan = model.LoaiTaiKhoan;
                taiKhoan.TenHienThi = model.TenHienThi;
                taiKhoan.GhiChu = model.GhiChu;
                
                // Cập nhật mật khẩu nếu có nhập
                if (!string.IsNullOrEmpty(model.MatKhau))
                {
                    if (model.MatKhau.Length < 3)
                    {
                        ModelState.AddModelError("MatKhau", "Mật khẩu phải từ 3 ký tự trở lên");
                        return View(model);
                    }
                    if (model.MatKhau != model.XacNhanMatKhau)
                    {
                        ModelState.AddModelError("XacNhanMatKhau", "Mật khẩu xác nhận không khớp");
                        return View(model);
                    }
                    taiKhoan.MatKhau = SecurityHelper.HashPassword(model.MatKhau);
                }
                
                db.Entry(taiKhoan).State = EntityState.Modified;
                db.SaveChanges();
                
                string successMsg = "Cập nhật tài khoản thành công!";
                if (!string.IsNullOrEmpty(model.MatKhau))
                {
                    successMsg += " Mật khẩu đã được thay đổi.";
                }
                TempData["SuccessMessage"] = successMsg;
                return RedirectToAction("Index");
            }
            
            ViewBag.Title = "Chỉnh sửa tài khoản";
            return View(model);
        }

        // POST: Admin/TaiKhoan_65133373/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var taiKhoan = db.TaiKhoans.Find(id);
            if (taiKhoan == null)
            {
                return HttpNotFound();
            }
            
            // Không cho phép xóa Admin
            if (taiKhoan.LoaiTaiKhoan == 0)
            {
                TempData["ErrorMessage"] = "Không thể xóa tài khoản Quản trị viên!";
                return RedirectToAction("Index");
            }
            
            // Kiểm tra có hóa đơn không
            if (taiKhoan.HoaDons.Any())
            {
                TempData["ErrorMessage"] = "Không thể xóa tài khoản đã tạo hóa đơn!";
                return RedirectToAction("Index");
            }
            
            db.TaiKhoans.Remove(taiKhoan);
            db.SaveChanges();
            
            TempData["SuccessMessage"] = "Xóa tài khoản thành công!";
            return RedirectToAction("Index");
        }

        // GET: Admin/TaiKhoan_65133373/ResetPassword/5
        public ActionResult ResetPassword(int id)
        {
            var taiKhoan = db.TaiKhoans.Find(id);
            if (taiKhoan == null)
            {
                return HttpNotFound();
            }
            
            var viewModel = new TaiKhoanViewModel
            {
                MaTK = taiKhoan.MaTK,
                TenDangNhap = taiKhoan.TenDangNhap,
                TenHienThi = taiKhoan.TenHienThi,
                LoaiTaiKhoan = taiKhoan.LoaiTaiKhoan
            };
            
            return View(viewModel);
        }
        
        // POST: Admin/TaiKhoan_65133373/ResetPassword/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(int id, FormCollection form)
        {
            var taiKhoan = db.TaiKhoans.Find(id);
            if (taiKhoan == null)
            {
                return HttpNotFound();
            }
            
            // Reset mật khẩu theo loại tài khoản: Admin = "admin", Thu ngân = "123"
            string defaultPassword = taiKhoan.LoaiTaiKhoan == 0 ? "admin" : "123";
            taiKhoan.MatKhau = SecurityHelper.HashPassword(defaultPassword);
            db.Entry(taiKhoan).State = EntityState.Modified;
            db.SaveChanges();
            
            TempData["SuccessMessage"] = $"Đã reset mật khẩu tài khoản '{taiKhoan.TenDangNhap}' về mặc định ({defaultPassword})!";
            return RedirectToAction("Index");
        }

        // GET: Admin/TaiKhoan_65133373/DoiMatKhau
        public ActionResult DoiMatKhau()
        {
            ViewBag.Title = "Đổi mật khẩu";
            var maTK = Convert.ToInt32(Session["MaTK"]);
            return View(new DoiMatKhauViewModel { MaTK = maTK });
        }

        // POST: Admin/TaiKhoan_65133373/DoiMatKhau
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DoiMatKhau(DoiMatKhauViewModel model)
        {
            if (ModelState.IsValid)
            {
                var maTK = Convert.ToInt32(Session["MaTK"]);
                var taiKhoan = db.TaiKhoans.Find(maTK);
                
                if (taiKhoan == null)
                {
                    return HttpNotFound();
                }
                
                // Kiểm tra mật khẩu cũ
                if (!SecurityHelper.VerifyPassword(model.MatKhauCu, taiKhoan.MatKhau))
                {
                    ModelState.AddModelError("MatKhauCu", "Mật khẩu cũ không đúng");
                    return View(model);
                }
                
                taiKhoan.MatKhau = SecurityHelper.HashPassword(model.MatKhauMoi);
                db.Entry(taiKhoan).State = EntityState.Modified;
                db.SaveChanges();
                
                TempData["SuccessMessage"] = "Đổi mật khẩu thành công!";
                return RedirectToAction("Index", "Dashboard_65133373");
            }
            
            ViewBag.Title = "Đổi mật khẩu";
            return View(model);
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
