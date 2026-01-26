using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BaiTap3_65133373.Models;

namespace BaiTap3_65133373.Controllers
{
    public class NhanVien_65133373Controller : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(HttpPostedFileBase Avatar, InfoModel emp)
        {
            string avatarFilePath = Server.MapPath("/Images/" + Avatar.FileName);
            string empInfoFilePath = Server.MapPath("/DB/" + "empInfo.txt");
            Avatar.SaveAs(avatarFilePath);
            string[] EmpInfo =
            {
                emp.EmpID,
                emp.Name,
                Convert.ToString(emp.BirthOfDate),
                emp.Email,  
                emp.Password,
                emp.Department,
                Avatar.FileName
            };
            System.IO.File.WriteAllLines(empInfoFilePath, EmpInfo);
            ViewBag.EmpID = EmpInfo[0];
            ViewBag.Name = EmpInfo[1];
            ViewBag.BirthOfDate = EmpInfo[2];
            ViewBag.Email = EmpInfo[3];
            ViewBag.Password = EmpInfo[4];
            ViewBag.Department = EmpInfo[5];
            ViewBag.Avatar = "/Images/" + EmpInfo[6];
            return View("Confirm");
        }
    }
}