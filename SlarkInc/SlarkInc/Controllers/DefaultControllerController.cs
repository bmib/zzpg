using System.Web.Mvc;
using SlarkInc.Models;

namespace SlarkInc.Controllers
{
    public class DefaultControllerController : Controller
    {
        // GET: /DefaultController/
        public ActionResult DefaultAction()
        {
            Simple s = new Simple();
            s.Name = "Slark";
            s.Email = "xxx@163.com";
            return View(s);
        }
	}
}