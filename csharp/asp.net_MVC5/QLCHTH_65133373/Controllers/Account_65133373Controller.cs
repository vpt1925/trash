using System;
using System.Linq;
using System.Web.Mvc;
using QLCHTH_65133373.Helpers;
using QLCHTH_65133373.Models;
using QLCHTH_65133373.Models.ViewModels;

namespace QLCHTH_65133373.Controllers
{
    public class Account_65133373Controller : Controller
    {
        private QLCHTH_65133373Entities db = new QLCHTH_65133373Entities();

        // GET: Account_65133373/Login
        public ActionResult Login(string returnUrl)
        {
            // Nếu đã đăng nhập thì redirect
            if (Session["MaTK"] != null)
            {
                return RedirectToArea();
            }
            
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.Title = "Đăng nhập";
            return View(new LoginViewModel());
        }

        // POST: Account_65133373/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var hashedPassword = SecurityHelper.HashPassword(model.MatKhau);
                
                var taiKhoan = db.TaiKhoans
                    .FirstOrDefault(t => t.TenDangNhap == model.TenDangNhap && t.MatKhau == hashedPassword);
                
                if (taiKhoan != null)
                {
                    // Lưu session
                    Session["MaTK"] = taiKhoan.MaTK;
                    Session["TenDangNhap"] = taiKhoan.TenDangNhap;
                    Session["TenHienThi"] = taiKhoan.TenHienThi;
                    Session["LoaiTaiKhoan"] = taiKhoan.LoaiTaiKhoan;
                    
                    // Nếu có return URL và hợp lệ
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    
                    // Redirect theo loại tài khoản
                    return RedirectToArea();
                }
                else
                {
                    ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng");
                }
            }
            
            ViewBag.Title = "Đăng nhập";
            return View(model);
        }

        // GET: Account_65133373/Logout
        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Login");
        }

        // GET: Account_65133373/AccessDenied
        public ActionResult AccessDenied()
        {
            ViewBag.Title = "Không có quyền truy cập";
            return View();
        }

        private ActionResult RedirectToArea()
        {
            var loaiTaiKhoan = Convert.ToInt32(Session["LoaiTaiKhoan"]);
            
            if (loaiTaiKhoan == 0) // Admin
            {
                return RedirectToAction("Index", "Dashboard_65133373", new { area = "Admin" });
            }
            else // Thu ngân
            {
                return RedirectToAction("Index", "BanHang_65133373", new { area = "Cashier" });
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
