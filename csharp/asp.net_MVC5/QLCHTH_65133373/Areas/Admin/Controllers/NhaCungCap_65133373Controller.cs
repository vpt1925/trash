using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using QLCHTH_65133373.Filters;
using QLCHTH_65133373.Models;
using QLCHTH_65133373.Models.ViewModels;

namespace QLCHTH_65133373.Areas.Admin.Controllers
{
    [AdminOnlyFilter_65133373]
    public class NhaCungCap_65133373Controller : Controller
    {
        private QLCHTH_65133373Entities db = new QLCHTH_65133373Entities();
        private const int PageSize = 10;

        // GET: Admin/NhaCungCap_65133373
        public ActionResult Index(string search, int page = 1)
        {
            ViewBag.Title = "Quản lý nhà cung cấp";
            
            var query = db.NhaCungCaps.AsQueryable();
            
            if (!string.IsNullOrWhiteSpace(search))
            {
                int searchId;
                if (int.TryParse(search, out searchId))
                {
                    query = query.Where(n => n.MaNCC == searchId || n.TenNCC.Contains(search));
                }
                else
                {
                    query = query.Where(n => n.TenNCC.Contains(search) || n.DienThoai.Contains(search));
                }
                ViewBag.Search = search;
            }
            
            int totalCount = query.Count();
            
            var nhaCungCaps = query
                .OrderBy(n => n.MaNCC)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .Select(n => new NhaCungCapViewModel
                {
                    MaNCC = n.MaNCC,
                    TenNCC = n.TenNCC,
                    DiaChi = n.DiaChi,
                    DienThoai = n.DienThoai,
                    Email = n.Email,
                    SoSanPham = n.SanPhams.Count()
                })
                .ToList();
            
            return View(new PagedList<NhaCungCapViewModel>(nhaCungCaps, page, PageSize, totalCount));
        }

        // GET: Admin/NhaCungCap_65133373/Create
        public ActionResult Create()
        {
            ViewBag.Title = "Thêm nhà cung cấp mới";
            return View(new NhaCungCapViewModel());
        }

        // POST: Admin/NhaCungCap_65133373/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NhaCungCapViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (db.NhaCungCaps.Any(n => n.TenNCC == model.TenNCC))
                {
                    ModelState.AddModelError("TenNCC", "Tên nhà cung cấp đã tồn tại");
                    return View(model);
                }
                
                var nhaCungCap = new NhaCungCap
                {
                    TenNCC = model.TenNCC,
                    DiaChi = model.DiaChi,
                    DienThoai = model.DienThoai,
                    Email = model.Email
                };
                
                db.NhaCungCaps.Add(nhaCungCap);
                db.SaveChanges();
                
                TempData["SuccessMessage"] = "Thêm nhà cung cấp thành công!";
                return RedirectToAction("Index");
            }
            
            ViewBag.Title = "Thêm nhà cung cấp mới";
            return View(model);
        }

        // GET: Admin/NhaCungCap_65133373/Edit/5
        public ActionResult Edit(int id)
        {
            var nhaCungCap = db.NhaCungCaps.Find(id);
            if (nhaCungCap == null)
            {
                return HttpNotFound();
            }
            
            var viewModel = new NhaCungCapViewModel
            {
                MaNCC = nhaCungCap.MaNCC,
                TenNCC = nhaCungCap.TenNCC,
                DiaChi = nhaCungCap.DiaChi,
                DienThoai = nhaCungCap.DienThoai,
                Email = nhaCungCap.Email
            };
            
            ViewBag.Title = "Chỉnh sửa nhà cung cấp";
            return View(viewModel);
        }

        // POST: Admin/NhaCungCap_65133373/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(NhaCungCapViewModel model)
        {
            if (ModelState.IsValid)
            {
                var nhaCungCap = db.NhaCungCaps.Find(model.MaNCC);
                if (nhaCungCap == null)
                {
                    return HttpNotFound();
                }
                
                if (db.NhaCungCaps.Any(n => n.TenNCC == model.TenNCC && n.MaNCC != model.MaNCC))
                {
                    ModelState.AddModelError("TenNCC", "Tên nhà cung cấp đã tồn tại");
                    return View(model);
                }
                
                nhaCungCap.TenNCC = model.TenNCC;
                nhaCungCap.DiaChi = model.DiaChi;
                nhaCungCap.DienThoai = model.DienThoai;
                nhaCungCap.Email = model.Email;
                
                db.Entry(nhaCungCap).State = EntityState.Modified;
                db.SaveChanges();
                
                TempData["SuccessMessage"] = "Cập nhật nhà cung cấp thành công!";
                return RedirectToAction("Index");
            }
            
            ViewBag.Title = "Chỉnh sửa nhà cung cấp";
            return View(model);
        }

        // POST: Admin/NhaCungCap_65133373/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var nhaCungCap = db.NhaCungCaps.Find(id);
            if (nhaCungCap == null)
            {
                return HttpNotFound();
            }
            
            if (nhaCungCap.SanPhams.Any())
            {
                TempData["ErrorMessage"] = "Không thể xóa nhà cung cấp đang có sản phẩm!";
                return RedirectToAction("Index");
            }
            
            db.NhaCungCaps.Remove(nhaCungCap);
            db.SaveChanges();
            
            TempData["SuccessMessage"] = "Xóa nhà cung cấp thành công!";
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
