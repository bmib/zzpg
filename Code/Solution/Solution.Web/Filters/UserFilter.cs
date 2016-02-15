using Solution.Services;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Solution.Filters
{
    public class UserFilter : FilterAttribute, IActionFilter
    {
        void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)
        {
            Console.WriteLine(filterContext.Controller.ToString());
            Console.WriteLine(filterContext.ActionDescriptor.ActionName);
            string actionName = filterContext.ActionDescriptor.ActionName;
            List<string> notAuthAction = new List<string> { "Login", "Register", "VerifyImage", "Error", "LoginAction", "RegisterSubmit", "GetPassword", "SendGetPasswordEmail", "SetPassword", "SetPasswordSubmit" };
            if (SessionService.User == null && !notAuthAction.Contains(actionName))
            {
                filterContext.Result = new RedirectResult("/Home/Login");
            }
        }

        void IActionFilter.OnActionExecuted(ActionExecutedContext filterContext)
        {

        }
    }
}