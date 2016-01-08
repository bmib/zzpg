using Solution.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Solution.Services;
using Solution.Framework.Contract;
using Solution.Common;
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

        /// <summary>
        /// 人员管理页面
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult UserManageView(Request request)
        {
            using (DBContext db = new DBContext())
            {
                string companyID = ((User)SessionService.GetValue("User")).CompanyID.ToString();
                var userList = db.User.Where(m => m.CompanyID.Equals(companyID)).OrderBy(m => m.UserName).ToPagedList(request.PageIndex, request.PageSize);
                return View(userList);
            }
        }

        /// <summary>
        /// 保存新添加用户
        /// </summary>
        /// <param name="collect"></param>
        /// <returns></returns>
        public ActionResult SaveAddUser(FormCollection collect)
        {
            using (DBContext db = new DBContext())
            {
                //获得密匙
                string salt = EncryptPassWord.CreateSalt();
                Solution.Models.User user = new Models.User
                {
                    UserID = Guid.NewGuid().ToString(),
                    CompanyID = SessionService.CompanyID,
                    UserName = collect["txtaddUsername"].ToString().Trim(),
                    Email = collect["txtaddEmail"].ToString().Trim(),
                    Password = EncryptPassWord.EncryptPwd(collect["txtaddPassword"].ToString().Trim(), salt),
                    Salt = salt,
                    Department = collect["txtaddDepartment"].ToString().Trim(),
                    Roles = collect["txtaddRoles"].ToString().Trim()
                };

                db.User.Add(user);
                db.SaveChanges();

                return Json(new { result = true, message = "" });
            }
        }

        /// <summary>
        /// 保存修改的用户信息
        /// </summary>
        /// <param name="collect"></param>
        /// <returns></returns>
        public ActionResult SaveModifyUser(FormCollection collect)
        {
            string userID = collect["txtmUserID"].ToString().Trim();
            if (!string.IsNullOrEmpty(userID))
            {
                using (DBContext db = new DBContext())
                {
                    var user = db.User.Where(m => m.UserID == userID).SingleOrDefault();
                    if (user != null)
                    {
                        user.UserName = collect["txtmUsername"].ToString().Trim();
                        user.Email = collect["txtmEmail"].ToString().Trim();
                        if (!string.IsNullOrEmpty(collect["txtmPassword"]))
                        {
                            string pwd = EncryptPassWord.EncryptPwd(collect["txtmPassword"].ToString().Trim(), user.Salt);
                            user.Password = pwd;
                        }
                        user.Department = collect["txtmDepartment"].ToString().Trim();
                        user.Roles = collect["txtmRoles"].ToString().Trim();
                    }

                    db.SaveChanges();

                    return Json(new { result = true, message = "" });
                }
            }
            return Json(new { result = false, message = "" });
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="UserID">用户ID</param>
        /// <returns></returns>
        public ActionResult UserDelete(string UserID)
        {
            using (DBContext db = new DBContext())
            {
                var user = db.User.Where(m => m.UserID == UserID).SingleOrDefault();
                db.User.Remove(user);
                db.SaveChanges();
                return RedirectToAction("UserManageView");
            }
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