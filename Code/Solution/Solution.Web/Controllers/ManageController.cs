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
                var departList = db.Department.Where(m => m.CompanyID.Equals(companyID)).OrderBy(m => m.DepartmentCode).ToPagedList(request.PageIndex, request.PageSize);
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

        /// <summary>
        /// 添加一级部门
        /// </summary>
        /// <param name="departmentName"></param>
        /// <returns></returns>
        public ActionResult SaveFirstDepartment(string departmentName)
        {
            string companyID = SessionService.CompanyID;
            if (!string.IsNullOrEmpty(departmentName) && !string.IsNullOrEmpty(companyID))
            {
                using (DBContext db = new DBContext())
                {
                    int departCode = 1;
                    //获取一级部门编码最大的部门
                    Department depart = db.Department.Where(m => m.CompanyID == companyID && m.DepartmentCode.Length == 4).OrderByDescending(m => m.DepartmentCode).FirstOrDefault();
                    if (depart != null)
                    {
                        departCode = int.Parse(depart.DepartmentCode) + 1;
                    }

                    Department newDepart = new Department
                    {
                        CompanyID = companyID,
                        DepartmentCode = departCode.ToString().PadLeft(4, '0'),
                        DepartmentID = Guid.NewGuid().ToString(),
                        DepartmentName = departmentName
                    };

                    db.Department.Add(newDepart);
                    db.SaveChanges();
                }

                return Json(new { result = true, message = "" });
            }

            return Json(new { result = false, message = "" });
        }

        /// <summary>
        /// 添加下级部门
        /// </summary>
        /// <param name="departmentName">部门名称</param>
        /// <param name="beforeDepartmentID">上级部门ID</param>
        /// <returns></returns>
        public ActionResult SaveNextDepartment(string departmentName, string beforeDepartmentID)
        {
            string companyID = SessionService.CompanyID;
            if (!string.IsNullOrEmpty(departmentName) && !string.IsNullOrEmpty(companyID) && !string.IsNullOrEmpty(beforeDepartmentID))
            {
                using (DBContext db = new DBContext())
                {
                    Department beforeDepart = db.Department.Where(m => m.CompanyID == companyID && m.DepartmentID == beforeDepartmentID).SingleOrDefault();
                    int len = beforeDepart.DepartmentCode.Length + 4;
                    string code = beforeDepart.DepartmentCode;
                    int departCode = 1;
                    //获取下级部门编码最大的部门
                    Department depart = db.Department.Where(m => m.CompanyID == companyID && m.DepartmentCode.Length == len && m.DepartmentCode.StartsWith(code)).OrderByDescending(m => m.DepartmentCode).FirstOrDefault();
                    if (depart != null)
                    {
                        departCode = int.Parse(depart.DepartmentCode.Substring(beforeDepart.DepartmentCode.Length, 4)) + 1;
                    }

                    Department newDepart = new Department
                    {
                        CompanyID = companyID,
                        DepartmentCode = string.Concat(beforeDepart.DepartmentCode, departCode.ToString().PadLeft(4, '0')),
                        DepartmentID = Guid.NewGuid().ToString(),
                        DepartmentName = departmentName
                    };

                    db.Department.Add(newDepart);
                    db.SaveChanges();
                }

                return Json(new { result = true, message = "" });
            }

            return Json(new { result = false, message = "" });
        }

        /// <summary>
        /// 修改部门
        /// </summary>
        /// <param name="txtCurrentDepartmentID">部门ID</param>
        /// <param name="txtCurrentDepartmentName">部门名称</param>
        /// <returns></returns>
        public ActionResult SaveModifyDepartment(string txtCurrentDepartmentID, string txtCurrentDepartmentName)
        {
            string companyID = SessionService.CompanyID;
            if (!string.IsNullOrEmpty(txtCurrentDepartmentID) && !string.IsNullOrEmpty(companyID) && !string.IsNullOrEmpty(txtCurrentDepartmentName))
            {
                using (DBContext db = new DBContext())
                {
                    Department depart = db.Department.Where(m => m.CompanyID == companyID && m.DepartmentID == txtCurrentDepartmentID).SingleOrDefault();
                    depart.DepartmentName = txtCurrentDepartmentName;
                    db.SaveChanges();
                }

                return Json(new { result = true, message = "" });
            }

            return Json(new { result = false, message = "" });
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