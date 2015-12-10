using System.Collections.Generic;
using System.Web.Mvc;
namespace SlarkInc.Controllers
{
    public class FirstController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Number = 8;
            ViewBag.Message = "This is index Page";
            ViewBag.Slarks = new List<string> { "Slark1", "Slark2", "Slark3" };
            return View();
        }
	}
}