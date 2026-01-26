using System.Web;
using System.Web.Mvc;

namespace BaiTap2_65133373
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
