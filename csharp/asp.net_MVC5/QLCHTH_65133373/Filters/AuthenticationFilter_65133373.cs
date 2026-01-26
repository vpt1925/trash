using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using QLCHTH_65133373.Models;

namespace QLCHTH_65133373.Filters
{
    /// <summary>
    /// Filter xác thực và phân quyền người dùng
    /// </summary>
    public class AuthenticationFilter_65133373 : ActionFilterAttribute
    {
        private readonly int[] _allowedRoles;

        /// <summary>
        /// Constructor cho phép tất cả user đã đăng nhập
        /// </summary>
        public AuthenticationFilter_65133373()
        {
            _allowedRoles = null;
        }

        /// <summary>
        /// Constructor cho phép các loại tài khoản cụ thể
        /// </summary>
        /// <param name="allowedRoles">0 = Admin, 1 = Thu ngân</param>
        public AuthenticationFilter_65133373(params int[] allowedRoles)
        {
            _allowedRoles = allowedRoles;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var session = filterContext.HttpContext.Session;

            // Kiểm tra đã đăng nhập chưa
            if (session["MaTK"] == null)
            {
                // Chưa đăng nhập -> chuyển về trang login
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary
                    {
                        { "controller", "Account_65133373" },
                        { "action", "Login" },
                        { "area", "" },
                        { "returnUrl", filterContext.HttpContext.Request.RawUrl }
                    });
                return;
            }

            // Nếu có giới hạn quyền thì kiểm tra
            if (_allowedRoles != null && _allowedRoles.Length > 0)
            {
                var loaiTaiKhoan = Convert.ToInt32(session["LoaiTaiKhoan"]);
                if (!_allowedRoles.Contains(loaiTaiKhoan))
                {
                    // Không có quyền -> chuyển về trang không có quyền
                    filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary
                        {
                            { "controller", "Account_65133373" },
                            { "action", "AccessDenied" },
                            { "area", "" }
                        });
                    return;
                }
            }

            base.OnActionExecuting(filterContext);
        }
    }

    /// <summary>
    /// Filter chỉ cho Admin
    /// </summary>
    public class AdminOnlyFilter_65133373 : AuthenticationFilter_65133373
    {
        public AdminOnlyFilter_65133373() : base(0) { }
    }

    /// <summary>
    /// Filter cho cả Admin và Thu ngân
    /// </summary>
    public class StaffFilter_65133373 : AuthenticationFilter_65133373
    {
        public StaffFilter_65133373() : base(0, 1) { }
    }
}
