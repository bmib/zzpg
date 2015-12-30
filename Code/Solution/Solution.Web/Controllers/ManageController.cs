using Solution.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Solution.Services;
using Solution.Framework.Contract;
namespace Solution.Web.Controllers
{
    public class ManageController : BaseController
    {
        /// <summary>
        /// 部门列表视图
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult DepartManageView(Request request)
        {
            using (DBContext db = new DBContext())
            {
                string companyID = ((User)SessionService.GetValue("User")).CompanyID.ToString();
                var departList = db.Department.Where(m => m.CompanyID.Equals(companyID)).OrderBy(m => m.DepartmentCode).ToPagedList(request.PageIndex,request.PageSize);
                return View(departList);
            }
        }

        /// <summary>
        /// 删除部门
        /// </summary>
        /// <param name="departmentCode"></param>
        /// <returns></returns>
        public ActionResult DepartDelete(string departmentCode)
        {
            using (DBContext db = new DBContext())
            {
                List<Department> departList = db.Department.Where(m => m.DepartmentCode.StartsWith(departmentCode)).ToList();
                foreach (Department depart in departList)
                {
                    db.Department.Remove(depart);
                }

                db.SaveChanges();
            }

            return RedirectToAction("DepartManageView");
        }

        public ActionResult DepartAddView(string parentCode)
        {
            return View();
        }

        public ActionResult DepartEditView(string departmentID)
        {
            return View();
        }

        public ActionResult UserManageView()
        {
            return View();
        }

        public ActionResult ItemManageView()
        {
            return View();
        }

        public ActionResult CheckManageView()
        {
            return View();
        }
	}
}