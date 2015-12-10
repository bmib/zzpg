using System;
using System.Web.Mvc;

namespace Solution.Filters
{
    public class LoggerFilter : FilterAttribute, IActionFilter
    {
        void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)
        {
            string controler = filterContext.Controller.ToString();
            string action = filterContext.ActionDescriptor.ActionName;
            filterContext.Controller.ViewData["ExecutingLogger"] = "已以写入日志！时间：" + DateTime.Now;
        }

        void IActionFilter.OnActionExecuted(ActionExecutedContext filterContext)
        {
            string controler = filterContext.Controller.ToString();
            string action = filterContext.ActionDescriptor.ActionName;

            filterContext.Controller.ViewData["ExecutedLogger"] = "已以写入日志！时间：" + DateTime.Now;
        }
    }
}