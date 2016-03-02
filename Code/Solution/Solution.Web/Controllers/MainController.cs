using Solution.Common;
using Solution.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Solution.Services;

namespace Solution.Web.Controllers
{
    public class MainController : Controller
    {
        public ActionResult Default()
        {
            return View();
        }
	}
}