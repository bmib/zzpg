using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Solution.Filters;

namespace Solution.Web.Controllers
{
    [UserFilter()]
    [LoggerFilter()]
    public class BaseController : Controller
    {
        protected override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);

            //记录错误日志
            //TODO

            // 当自友定义显示错误 mode = On，显示好错误页面
            if (filterContext.HttpContext.IsCustomErrorEnabled)
            {
                filterContext.ExceptionHandled = true;
                filterContext.Result = new RedirectResult("/Home/Error");
            }
        }
	}
}