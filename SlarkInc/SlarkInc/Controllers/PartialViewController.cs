using System;
using System.Web.Mvc;

namespace SlarkInc.Controllers
{
    public class PartialViewController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
public PartialViewResult ChildAction(DateTime time)
{
    string greetings = string.Empty;
    if(time.Hour > 18)
    {
        greetings = "Good evening. Now is " + time.ToString("HH:mm:ss");
    }
    else if (time.Hour > 12)
    {
        greetings = "Good afternoon. Now is " + time.ToString("HH:mm:ss");
    }
    else
    {
        greetings = "Good morning. Now is " + time.ToString("HH:mm:ss");
    }
    return PartialView("ChildAction",greetings);
}
	}
}