using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using QLCHTH_65133373.Filters;
using QLCHTH_65133373.Models;
using QLCHTH_65133373.Models.ViewModels;

namespace QLCHTH_65133373.Areas.Admin.Controllers
{
    [AdminOnlyFilter_65133373]
    public class DanhMuc_65133373Controller : Controller
    {
        private QLCHTH_65133373Entities db = new QLCHTH_65133373Entities();
        private const int PageSize = 10;

        // GET: Admin/DanhMuc_65133373
        public ActionResult Index(string search, int page = 1)
        {
            ViewBag.Title = "Quản lý danh mục";
            
            var query = db.DanhMucs.AsQueryable();
            
            if (!string.IsNullOrWhiteSpace(search))
            {
                int searchId;
                if (int.TryParse(search, out searchId))
                {
                    query = query.Where(d => d.MaDanhMuc == searchId || d.TenDanhMuc.Contains(search));
                }
                else
                {
                    query = query.Where(d => d.TenDanhMuc.Contains(search));
                }
                ViewBag.Search = search;
            }
            
            int totalCount = query.Count();
            
            var danhMucs = query
                .OrderBy(d => d.MaDanhMuc)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .Select(d => new DanhMucViewModel
                {
                    MaDanhMuc = d.MaDanhMuc,
                    TenDanhMuc = d.TenDanhMuc,
                    MoTa = d.MoTa,
                    SoSanPham = d.SanPhams.Count()
                })
                .ToList();
            
            return View(new PagedList<DanhMucViewModel>(danhMucs, page, PageSize, totalCount));
        }

        // GET: Admin/DanhMuc_65133373/Create
        public ActionResult Create()
        {
            ViewBag.Title = "Thêm danh mục mới";
            return View(new DanhMucViewModel());
        }

        // POST: Admin/DanhMuc_65133373/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DanhMucViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (db.DanhMucs.Any(d => d.TenDanhMuc == model.TenDanhMuc))
                {
                    ModelState.AddModelError("TenDanhMuc", "Tên danh mục đã tồn tại");
                    return View(model);
                }
                
                var danhMuc = new DanhMuc
                {
                    TenDanhMuc = model.TenDanhMuc,
                    MoTa = model.MoTa
                };
                
                db.DanhMucs.Add(danhMuc);
                db.SaveChanges();
                
                TempData["SuccessMessage"] = "Thêm danh mục thành công!";
                return RedirectToAction("Index");
            }
            
            ViewBag.Title = "Thêm danh mục mới";
            return View(model);
        }

        // GET: Admin/DanhMuc_65133373/Edit/5
        public ActionResult Edit(int id)
        {
            var danhMuc = db.DanhMucs.Find(id);
            if (danhMuc == null)
            {
                return HttpNotFound();
            }
            
            var viewModel = new DanhMucViewModel
            {
                MaDanhMuc = danhMuc.MaDanhMuc,
                TenDanhMuc = danhMuc.TenDanhMuc,
                MoTa = danhMuc.MoTa
            };
            
            ViewBag.Title = "Chỉnh sửa danh mục";
            return View(viewModel);
        }

        // POST: Admin/DanhMuc_65133373/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DanhMucViewModel model)
        {
            if (ModelState.IsValid)
            {
                var danhMuc = db.DanhMucs.Find(model.MaDanhMuc);
                if (danhMuc == null)
                {
                    return HttpNotFound();
                }
                
                if (db.DanhMucs.Any(d => d.TenDanhMuc == model.TenDanhMuc && d.MaDanhMuc != model.MaDanhMuc))
                {
                    ModelState.AddModelError("TenDanhMuc", "Tên danh mục đã tồn tại");
                    return View(model);
                }
                
                danhMuc.TenDanhMuc = model.TenDanhMuc;
                danhMuc.MoTa = model.MoTa;
                
                db.Entry(danhMuc).State = EntityState.Modified;
                db.SaveChanges();
                
                TempData["SuccessMessage"] = "Cập nhật danh mục thành công!";
                return RedirectToAction("Index");
            }
            
            ViewBag.Title = "Chỉnh sửa danh mục";
            return View(model);
        }

        // POST: Admin/DanhMuc_65133373/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var danhMuc = db.DanhMucs.Find(id);
            if (danhMuc == null)
            {
                return HttpNotFound();
            }
            
            if (danhMuc.SanPhams.Any())
            {
                TempData["ErrorMessage"] = "Không thể xóa danh mục đang có sản phẩm!";
                return RedirectToAction("Index");
            }
            
            db.DanhMucs.Remove(danhMuc);
            db.SaveChanges();
            
            TempData["SuccessMessage"] = "Xóa danh mục thành công!";
            return RedirectToAction("Index");
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
