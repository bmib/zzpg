using Solution.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Solution.Services;
using Solution.Framework.Contract;
using Solution.Common;
using System.IO;
using System.Data;
using System.Data.Entity.Validation;
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
                string companyID = SessionService.CompanyID;
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

        /// <summary>
        /// 下载用户导入模板
        /// </summary>
        /// <returns></returns>
        public ActionResult DownLoadUserTemplate()
        {
            var fileName = Server.MapPath("~/Content/template/UserTemplate.xls");
            return File(fileName, "application/ms-excel", "UserTemplate.xls");
        }

        /// <summary>
        /// 批量导入用户
        /// </summary>
        /// <returns></returns>
        public ActionResult UploadUser()
        {
            int successNum = 0;
            try
            {
                var files = Request.Files;
                if (files != null && files.Count > 0)
                {
                    int length = files[0].FileName.Split('.').Count();
                    string extName = files[0].FileName.Split('.')[length - 1];
                    DirectoryInfo dir = new DirectoryInfo(HttpContext.Server.MapPath("../Files"));
                    if (!dir.Exists)// 如果不存在，则创建目录  
                        dir.Create();
                    Random ran = new Random();
                    int RandKey = ran.Next(1000, 9999);
                    string fileName = Path.Combine(dir.FullName, Path.GetFileName(string.Concat(DateTime.Now.ToString("yyyyMMddHHmmss"), "_", RandKey.ToString(), ".", extName)));
                    files[0].SaveAs(fileName);

                    DataSet ds = ExcelHelper.ExcelToDS(fileName, "sheet1");

                    using (DBContext db = new DBContext())
                    {
                        if (ds != null && ds.Tables.Count > 0)
                        {
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                string email = ds.Tables[0].Rows[i][1].ToString().Trim();
                                if (!string.IsNullOrEmpty(email))
                                {
                                    //判断email是否已经存在
                                    var chkuser = db.User.Where(m => m.Email.Equals(email)).SingleOrDefault();
                                    if (chkuser == null)
                                    {
                                        string userName = ds.Tables[0].Rows[i][0].ToString().Trim();

                                        string pwd = ds.Tables[0].Rows[i][2].ToString().Trim();
                                        string department = ds.Tables[0].Rows[i][3].ToString().Trim();
                                        string roles = ds.Tables[0].Rows[i][4].ToString().Trim();
                                        if (string.IsNullOrEmpty(pwd))
                                        {
                                            pwd = email;
                                        }

                                        //获得密匙
                                        string salt = EncryptPassWord.CreateSalt();
                                        //得到经过加密后的"密码"
                                        string espassword = EncryptPassWord.EncryptPwd(pwd, salt);

                                        User user = new User
                                        {
                                            UserID = Guid.NewGuid().ToString(),
                                            UserName = userName,
                                            Email = email,
                                            Password = espassword,
                                            Department = department,
                                            Roles = roles,
                                            Salt = salt,
                                            CompanyID = SessionService.CompanyID
                                        };

                                        db.User.Add(user);

                                        successNum++;
                                    }
                                }
                            }
                        }
                        db.SaveChanges();
                    }
                }

                return Json(new { result = true, message = string.Concat("操作成功，导入了", successNum.ToString(), "个用户。") });
            }
            catch (Exception ex)
            {
                return Json(new { result = false, message = ex.Message });
            }
        }

        /// <summary>
        /// 指标库管理
        /// </summary>
        /// <returns></returns>
        public ActionResult ItemFactoryList(Request request)
        {
            using (DBContext db = new DBContext())
            {
                string companyID = SessionService.CompanyID;
                var itemFactoryList = db.ItemFactory.Where(m => m.CompanyID.Equals(companyID)).OrderBy(m => m.ItemFactoryName).ToPagedList(request.PageIndex, request.PageSize);
                return View(itemFactoryList);
            }
        }

        /// <summary>
        /// 删除指标库
        /// </summary>
        /// <param name="itemFatoryID"></param>
        /// <returns></returns>
        public ActionResult ItemFactoryDelete(string itemFatoryID)
        {
            using (DBContext db = new DBContext())
            {
                var itemFactory = db.ItemFactory.Where(m => m.ItemFactoryID.Equals(itemFatoryID)).FirstOrDefault();

                db.ItemFactory.Remove(itemFactory);

                db.SaveChanges();
            }

            return RedirectToAction("ItemFactoryList");
        }

        /// <summary>
        /// 保存新增的指标库
        /// </summary>
        /// <param name="txtItemFactoryName"></param>
        /// <returns></returns>
        public ActionResult SaveItemFactory(string txtItemFactoryName)
        {
            string companyID = SessionService.CompanyID;
            if (!string.IsNullOrEmpty(txtItemFactoryName) && !string.IsNullOrEmpty(companyID))
            {
                using (DBContext db = new DBContext())
                {
                    ItemFactory itemFactory = new ItemFactory
                    {
                        CompanyID = companyID,
                        ItemFactoryID = Guid.NewGuid().ToString(),
                        ItemFactoryName = txtItemFactoryName,
                        State = 0
                    };

                    db.ItemFactory.Add(itemFactory);
                    db.SaveChanges();
                }

                return Json(new { result = true, message = "" });
            }

            return Json(new { result = false, message = "" });
        }

        /// <summary>
        /// 保存修改的指标库
        /// </summary>
        /// <param name="txtCurrentItemFactoryID"></param>
        /// <param name="txtCurrentItemFactoryName"></param>
        /// <returns></returns>
        public ActionResult SaveModifyItemFactory(string txtCurrentItemFactoryID, string txtCurrentItemFactoryName)
        {
            string companyID = SessionService.CompanyID;
            if (!string.IsNullOrEmpty(txtCurrentItemFactoryName) && !string.IsNullOrEmpty(companyID) && !string.IsNullOrEmpty(txtCurrentItemFactoryID))
            {
                using (DBContext db = new DBContext())
                {
                    ItemFactory itemFactory = db.ItemFactory.Where(m => m.CompanyID == companyID && m.ItemFactoryID == txtCurrentItemFactoryID).SingleOrDefault();
                    itemFactory.ItemFactoryName = txtCurrentItemFactoryName;
                    db.SaveChanges();
                }

                return Json(new { result = true, message = "" });
            }

            return Json(new { result = false, message = "" });
        }

        /// <summary>
        /// 指标管理视图
        /// </summary>
        /// <param name="request"></param>
        /// <param name="itemFactoryID"></param>
        /// <returns></returns>
        public ActionResult ItemManageView(Request request, string itemFactoryID)
        {
            using (DBContext db = new DBContext())
            {
                var itemList = db.Item.Where(m => m.ItemFactoryID.Equals(itemFactoryID)).OrderBy(m => m.ItemCode).ToPagedList(request.PageIndex, request.PageSize);
                return View(itemList);
            }
        }

        /// <summary>
        /// 删除指标操作
        /// </summary>
        /// <param name="ItemCode"></param>
        /// <returns></returns>
        public ActionResult ItemDelete(string ItemCode, string ItemFactoryID)
        {
            using (DBContext db = new DBContext())
            {
                //重新计算指标的编号
                List<Item> numList = db.Item.Where(m => m.ItemFactoryID == ItemFactoryID && m.ItemCode.Length == ItemCode.Length).ToList();
                Item numItem = db.Item.Where(m => m.ItemFactoryID == ItemFactoryID && m.ItemCode == ItemCode).SingleOrDefault();
                foreach (var temp in numList)
                {
                    var nums = temp.ItemNumber.Split('.');
                    int last = int.Parse(nums[nums.Length - 1]);
                    var curNums = numItem.ItemNumber.Split('.');
                    if (last > int.Parse(curNums[curNums.Length - 1]))
                    {
                        string strnumber = string.Empty; 
                        for (int i = 0; i < nums.Length - 1; i++)
                        {
                            strnumber = string.Concat(strnumber, nums[i], ".");
                        }

                        strnumber = string.Concat(strnumber, (last-1).ToString());

                        temp.ItemNumber = strnumber;
                    }
                }

                //删除指标及下级指标
                List<Item> itemList = db.Item.Where(m => m.ItemCode.StartsWith(ItemCode) && m.ItemFactoryID == ItemFactoryID).ToList();
                foreach (Item item in itemList)
                {
                    List<ItemPoint> pointList = db.ItemPoint.Where(m => m.ItemID == item.ItemID).ToList();
                    foreach(ItemPoint point in pointList)
                    {
                        db.ItemPoint.Remove(point); //删除考核点
                    }

                    db.Item.Remove(item); //删除指标
                }

                db.SaveChanges();
            }

            return new RedirectResult("/Manage/ItemManageView?itemFactoryID=" + ItemFactoryID);
        }

        /// <summary>
        /// 添加一级指标
        /// </summary>
        /// <param name="ItemName"></param>
        /// <returns></returns>
        public ActionResult SaveFirstItem(string ItemName, string ItemFactoryID, string CheckStandard, string pointList)
        {
            if (!string.IsNullOrEmpty(ItemName) && !string.IsNullOrEmpty(ItemFactoryID))
            {
                using (DBContext db = new DBContext())
                {
                    int itemCode = 1;
                    int ItemNumber = 1;
                    //获取一级指标最大的指标
                    Item item = db.Item.Where(m => m.ItemFactoryID == ItemFactoryID && m.ItemCode.Length == 4).OrderByDescending(m => m.ItemCode).FirstOrDefault();
                    if (item != null)
                    {
                        itemCode = int.Parse(item.ItemCode) + 1;
                        ItemNumber = int.Parse(item.ItemNumber) + 1;
                    }

                    //新增指标
                    Item newItem = new Item
                    {
                        ItemID = Guid.NewGuid().ToString(),
                        ItemFactoryID = ItemFactoryID,
                        ItemCode = itemCode.ToString().PadLeft(4, '0'),
                        ItemNumber = ItemNumber.ToString(),
                        CheckStandard = CheckStandard,
                        ItemName = ItemName
                    };

                    //新增指标考核点
                    if (!string.IsNullOrEmpty(pointList))
                    {
                        foreach (string point in pointList.Split(','))
                        {
                            if (!string.IsNullOrEmpty(point))
                            {
                                ItemPoint itemPoint = new ItemPoint
                                {
                                    ItemID = newItem.ItemID,
                                    ItemPointID = Guid.NewGuid().ToString(),
                                    ItemPointName = point
                                };
                                db.ItemPoint.Add(itemPoint);
                            }
                        }
                    }

                    db.Item.Add(newItem);
                    db.SaveChanges();
                }

                return Json(new { result = true, message = "" });
            }

            return Json(new { result = false, message = "" });
        }

        /// <summary>
        /// 添加下级指标
        /// </summary>
        /// <param name="txtNextItemName"></param>
        /// <param name="txtBeforeItemID"></param>
        /// <param name="CheckStandard"></param>
        /// <param name="pointList"></param>
        /// <returns></returns>
        public ActionResult SaveNextItem(string txtNextItemName, string txtBeforeItemID, string CheckStandard, string pointList)
        {
            if (!string.IsNullOrEmpty(txtNextItemName) && !string.IsNullOrEmpty(txtBeforeItemID))
            {
                using (DBContext db = new DBContext())
                {
                    Item beforeItem = db.Item.Where(m => m.ItemID == txtBeforeItemID).SingleOrDefault();
                    int len = beforeItem.ItemCode.Length + 4;
                    string code = beforeItem.ItemCode;
                    int itemCode = 1;
                    int itemNumber = 1;
                    //获取下级部门编码最大的部门
                    Item item = db.Item.Where(m => m.ItemCode.Length == len && m.ItemCode.StartsWith(code)).OrderByDescending(m => m.ItemCode).FirstOrDefault();
                    if (item != null)
                    {
                        itemCode = int.Parse(item.ItemCode.Substring(beforeItem.ItemCode.Length, 4)) + 1;
                        var num = item.ItemNumber.Split('.');
                        itemNumber = int.Parse(num[num.Length - 1]) + 1;
                    }

                    Item newItem = new Item
                    {
                        ItemFactoryID = beforeItem.ItemFactoryID,
                        ItemCode = string.Concat(beforeItem.ItemCode, itemCode.ToString().PadLeft(4, '0')),
                        ItemID = Guid.NewGuid().ToString(),
                        ItemName = txtNextItemName,
                        CheckStandard = CheckStandard,
                        ItemNumber = string.Concat(beforeItem.ItemNumber, ".", itemNumber.ToString())
                    };

                    //新增指标考核点
                    if (!string.IsNullOrEmpty(pointList))
                    {
                        foreach (string point in pointList.Split(','))
                        {
                            if (!string.IsNullOrEmpty(point))
                            {
                                ItemPoint itemPoint = new ItemPoint
                                {
                                    ItemID = newItem.ItemID,
                                    ItemPointID = Guid.NewGuid().ToString(),
                                    ItemPointName = point
                                };
                                db.ItemPoint.Add(itemPoint);
                            }
                        }
                    }

                    db.Item.Add(newItem);
                    db.SaveChanges();
                }

                return Json(new { result = true, message = "" });
            }

            return Json(new { result = false, message = "" });
        }


        public ActionResult SaveModifyItem(string txtCurrentDepartmentID, string txtCurrentDepartmentName)
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

        public ActionResult CheckManageView()
        {
            return View();
        }
    }
}