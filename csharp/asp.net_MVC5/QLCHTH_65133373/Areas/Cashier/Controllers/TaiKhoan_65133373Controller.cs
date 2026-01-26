using System;
using System.Data.Entity;
using System.Web.Mvc;
using QLCHTH_65133373.Filters;
using QLCHTH_65133373.Helpers;
using QLCHTH_65133373.Models;
using QLCHTH_65133373.Models.ViewModels;

namespace QLCHTH_65133373.Areas.Cashier.Controllers
{
    [StaffFilter_65133373]
    public class TaiKhoan_65133373Controller : Controller
    {
        private QLCHTH_65133373Entities db = new QLCHTH_65133373Entities();

        // GET: Cashier/TaiKhoan_65133373/DoiMatKhau
        public ActionResult DoiMatKhau()
        {
            ViewBag.Title = "Đổi mật khẩu";
            var maTK = Convert.ToInt32(Session["MaTK"]);
            return View(new DoiMatKhauViewModel { MaTK = maTK });
        }

        // POST: Cashier/TaiKhoan_65133373/DoiMatKhau
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
                return RedirectToAction("Index", "BanHang_65133373");
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
