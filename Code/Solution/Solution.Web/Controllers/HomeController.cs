using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Solution.Common;

namespace Solution.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult LoginSystem(FormCollection collection)
        {
            ViewBag.Message = string.Empty;
            if (!Session["VerifyImageCode"].ToString().Equals(collection["inputCheckCode"].Trim()))
            {
                ViewBag.Message = "验证码不正确。";
            }


            return View();
        }


        public ActionResult Register()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult VerifyImage()
        {
            var s1 = new ValidateCode_Style10();
            string code = string.Empty;
            byte[] bytes = s1.CreateImage(out code);

            Session["VerifyImageCode"] = code;

            return File(bytes, @"image/jpeg");
        }
    }
}