using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using BaiTap2_65133373.Models;

namespace BaiTap2_65133373.Controllers
{
    public class SinhVien_65133373Controller : Controller
    {
        public ActionResult UseRequest()
        {
            return View("Register");
        }
        [HttpPost]
        public ActionResult UseRequest(string placeholder)
        {
            ViewBag.Id = Request["Id"];
            ViewBag.Name = Request["Name"];
            ViewBag.Marks = Convert.ToDouble(Request["Marks"]);
            return View("Registered");
        }
        public ActionResult UseArgument()
        {
            return View("Register");
        }
        [HttpPost]
        public ActionResult UseArgument(string Id, string Name, string Marks)
        {
            ViewBag.Id = Id;
            ViewBag.Name = Name;
            ViewBag.Marks = Convert.ToDouble(Marks);
            return View("Registered");
        }
        public ActionResult UseFormCollection()
        {
            return View("Register");
        }
        [HttpPost]
        public ActionResult UseFormCollection(FormCollection form)
        {
            ViewBag.Id = form["Id"];
            ViewBag.Name = form["Name"];
            ViewBag.Marks = Convert.ToDouble(form["Marks"]);
            return View("Registered");
        }
        public ActionResult UseModel()
        {
            return View("Register");
        }
        [HttpPost]
        public ActionResult UseModel(InfoModel i)
        {
            ViewBag.Id = i.Id;
            ViewBag.Name = i.Name;
            ViewBag.Marks = Convert.ToDouble(i.Marks);
            return View("Registered");
        }
    }
}