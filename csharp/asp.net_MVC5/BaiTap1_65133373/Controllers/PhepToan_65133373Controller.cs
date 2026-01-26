using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BaiTap1_65133373.Models;

namespace BaiTap1_65133373.Controllers
{
    public class PhepToan_65133373Controller : Controller
    {
        // GET: PhepToan_65133373
        public ActionResult UseRequest()
        {
            return View("Calculator");
        }
        [HttpPost]
        public ActionResult UseRequest(string pt) 
        {
            double a = Convert.ToDouble(Request["a"]);
            double b = Convert.ToDouble(Request["b"]);
            pt = Request["pt"];
            switch (pt)
            {
                case "+": ViewBag.kq = a + b; break;
                case "-": ViewBag.kq = a - b; break;
                case "*": ViewBag.kq = a * b; break;
                case "/":
                    if (b == 0) ViewBag.kq = "không thể chia cho 0";
                    else ViewBag.kq = a / b; break;
            }
            return View("Calculator");
        }
        public ActionResult UseArgument()
        {
            return View("Calculator");
        }
        [HttpPost]
        public ActionResult UseArgument(double a, double b, string pt)
        {
            switch (pt)
            {
                case "+": ViewBag.kq = a + b; break;
                case "-": ViewBag.kq = a - b; break;
                case "*": ViewBag.kq = a * b; break;
                case "/":
                    if (b == 0) ViewBag.kq = "không thể chia cho 0";
                    else ViewBag.kq = a / b; break;
            }
            return View("Calculator");
        }
        public ActionResult UseFormCollection()
        {
            return View("Calculator");
        }
        [HttpPost]
        public ActionResult UseFormCollection(FormCollection form)
        {
            double a = Convert.ToDouble(form["a"]);
            double b = Convert.ToDouble(form["b"]);
            string pt = form["pt"];
            switch (pt)
            {
                case "+": ViewBag.kq = a + b; break;
                case "-": ViewBag.kq = a - b; break;
                case "*": ViewBag.kq = a * b; break;
                case "/":
                    if (b == 0) ViewBag.kq = "không thể chia cho 0";
                    else ViewBag.kq = a / b; break;
            }
            return View("Calculator");
        }
        public ActionResult UseModel()
        {
            return View("Calculator");
        }
        [HttpPost]
        public ActionResult UseModel(CalModel cal)
        {
            double a = cal.a;
            double b = cal.b;
            string pt = cal.pt;
            switch (pt)
            {
                case "+": ViewBag.kq = a + b; break;
                case "-": ViewBag.kq = a - b; break;
                case "*": ViewBag.kq = a * b; break;
                case "/":
                    if (b == 0) ViewBag.kq = "không thể chia cho 0";
                    else ViewBag.kq = a / b; break;
            }
            return View("Calculator");
        }
    }
}