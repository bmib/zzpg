using Solution.Common;
using Solution.Framework.Contract;
using Solution.Models;
using Solution.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.Runtime.InteropServices;
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
                //List<Item> numList = db.Item.Where(m => m.ItemFactoryID == ItemFactoryID && m.ItemCode.Length == ItemCode.Length).ToList();
                //Item numItem = db.Item.Where(m => m.ItemFactoryID == ItemFactoryID && m.ItemCode == ItemCode).SingleOrDefault();
                //foreach (var temp in numList)
                //{
                //    var nums = temp.ItemNumber.Split('.');
                //    int last = int.Parse(nums[nums.Length - 1]);
                //    var curNums = numItem.ItemNumber.Split('.');
                //    if (last > int.Parse(curNums[curNums.Length - 1]))
                //    {
                //        string strnumber = string.Empty; 
                //        for (int i = 0; i < nums.Length - 1; i++)
                //        {
                //            strnumber = string.Concat(strnumber, nums[i], ".");
                //        }

                //        strnumber = string.Concat(strnumber, (last-1).ToString());

                //        temp.ItemNumber = strnumber;
                //    }
                //}

                //删除指标及下级指标
                List<Item> itemList = db.Item.Where(m => m.ItemCode.StartsWith(ItemCode) && m.ItemFactoryID == ItemFactoryID).ToList();
                foreach (Item item in itemList)
                {
                    List<ItemPoint> pointList = db.ItemPoint.Where(m => m.ItemID == item.ItemID).ToList();
                    foreach (ItemPoint point in pointList)
                    {
                        db.ItemPoint.Remove(point); //删除考核点
                    }

                    db.Item.Remove(item); //删除指标
                }

                db.SaveChanges();

                //重新计算编号。全部重新更新
                int iFirst = 0;
                int iSecond = 0;
                int iThird = 0;
                List<Item> items = db.Item.OrderBy(m => m.ItemCode).ToList();
                for (int i = 0; i < items.Count; i++)
                {
                    if (items[i].ItemCode.Length == 4)
                    {
                        //一级指标
                        iFirst++;
                        items[i].ItemNumber = iFirst.ToString();
                        items[i].ItemCode = iFirst.ToString().PadLeft(4, '0');
                        iSecond = 0;
                    }
                    else if (items[i].ItemCode.Length == 8)
                    {
                        //二级指标
                        iSecond++;
                        items[i].ItemNumber = string.Concat(iFirst.ToString(), ".", iSecond.ToString());
                        items[i].ItemCode = string.Concat(iFirst.ToString().PadLeft(4, '0'), iSecond.ToString().PadLeft(4, '0'));
                        iThird = 0;
                    }
                    else if (items[i].ItemCode.Length == 12)
                    {
                        //三级指标
                        iThird++;
                        items[i].ItemNumber = string.Concat(iFirst.ToString(), ".", iSecond.ToString(), ".", iThird.ToString());
                        items[i].ItemCode = string.Concat(iFirst.ToString().PadLeft(4, '0'), iSecond.ToString().PadLeft(4, '0'), iThird.ToString().PadLeft(4, '0'));
                    }
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

        /// <summary>
        /// 获取指标信息
        /// </summary>
        /// <param name="ItemID"></param>
        /// <returns></returns>
        public ActionResult GetModifyItem(string ItemID)
        {
            using (DBContext db = new DBContext())
            {
                Item item = db.Item.Include("ItemPoints").Where(m => m.ItemID == ItemID).FirstOrDefault();
                string points = string.Empty;
                foreach (ItemPoint p in item.ItemPoints)
                {
                    points += p.ItemPointName + ",";
                }

                return Json(new { result = true, message = "", CheckStandard = item.CheckStandard, ItemPoints = points.TrimEnd(',') });
            }
        }

        /// <summary>
        /// 保存修改的指标信息
        /// </summary>
        /// <param name="txtModifyItemID"></param>
        /// <param name="txtModifyItemName"></param>
        /// <param name="CheckStandard"></param>
        /// <param name="pointList"></param>
        /// <returns></returns>
        public ActionResult SaveModifyItem(string txtModifyItemID, string txtModifyItemName, string CheckStandard, string pointList)
        {
            if (!string.IsNullOrEmpty(txtModifyItemID) && !string.IsNullOrEmpty(txtModifyItemName))
            {
                using (DBContext db = new DBContext())
                {
                    Item curItem = db.Item.Include("ItemPoints").Where(m => m.ItemID == txtModifyItemID).SingleOrDefault();
                    curItem.ItemName = txtModifyItemName;
                    curItem.CheckStandard = CheckStandard;
                    curItem.ItemPoints.Clear();

                    db.SaveChanges();

                    //新增指标考核点
                    if (!string.IsNullOrEmpty(pointList))
                    {
                        foreach (string point in pointList.Split(','))
                        {
                            if (!string.IsNullOrEmpty(point))
                            {
                                ItemPoint itemPoint = new ItemPoint
                                {
                                    ItemID = curItem.ItemID,
                                    ItemPointID = Guid.NewGuid().ToString(),
                                    ItemPointName = point
                                };
                                db.ItemPoint.Add(itemPoint);
                            }
                        }
                    }

                    db.SaveChanges();
                }

                return Json(new { result = true, message = "" });
            }

            return Json(new { result = false, message = "" });
        }

        /// <summary>
        /// 检查列表视图
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult CheckManageView(Request request)
        {
            using (DBContext db = new DBContext())
            {
                string companyID = SessionService.CompanyID;
                var checkList = db.Check.Where(m => m.CompanyID.Equals(companyID)).OrderByDescending(m => m.CreatedTime).ToPagedList(request.PageIndex, request.PageSize);
                return View(checkList);
            }
        }

        /// <summary>
        /// 保存检查
        /// </summary>
        /// <param name="txtCheckName"></param>
        /// <returns></returns>
        public ActionResult SaveCheck(string txtCheckName)
        {
            if (!string.IsNullOrEmpty(txtCheckName))
            {
                using (DBContext db = new DBContext())
                {
                    Check check = new Check
                    {
                        CheckID = Guid.NewGuid().ToString(),
                        CheckName = txtCheckName,
                        CompanyID = SessionService.CompanyID,
                        CreatedTime = DateTime.Now,
                        CreatedUser = SessionService.UserID,
                        State = "0"
                    };

                    db.Check.Add(check);
                    db.SaveChanges();
                }

                return Json(new { result = true, message = "" });
            }

            return Json(new { result = false, message = "检查名称不能为空" });
        }

        /// <summary>
        /// 保存修改的检查名称
        /// </summary>
        /// <param name="txtCheckID"></param>
        /// <param name="txtCurrentCheckName"></param>
        /// <returns></returns>
        public ActionResult SaveModifyCheck(string txtCheckID, string txtCurrentCheckName)
        {
            if (!string.IsNullOrEmpty(txtCheckID))
            {
                using (DBContext db = new DBContext())
                {
                    Check check = db.Check.Where(m => m.CheckID == txtCheckID).FirstOrDefault();
                    check.CheckName = txtCurrentCheckName;
                    db.SaveChanges();
                }

                return Json(new { result = true, message = "" });
            }

            return Json(new { result = false, message = "" });
        }

        /// <summary>
        /// 评估详情页面
        /// </summary>
        /// <param name="CheckID"></param>
        /// <returns></returns>
        public ActionResult CheckDetailView(string CheckID)
        {
            using (DBContext db = new DBContext())
            {
                string companyID = SessionService.CompanyID;
                var check = db.Check.Where(m => m.CompanyID.Equals(companyID) && m.CheckID.Equals(CheckID)).SingleOrDefault();
                ViewBag.CheckName = check.CheckName;
                ViewBag.CheckState = check.State;
                ViewBag.ItemFactoryList = db.ItemFactory.Where(m => m.CompanyID == companyID).ToList();

                var checkItemList = db.CheckItem.Where(m => m.CheckID == CheckID).OrderBy(m => m.CheckItemCode).ToList();
                ViewBag.checkItemList = checkItemList; //指标列表

                List<User> proList = db.User.Where(m => m.CompanyID == companyID && m.Roles.Contains("pro")).ToList();
                ViewBag.proList = proList; //专家列表

                var weightTaskList = (from m in db.WeightTask
                                      join n in db.User on m.WeightUser equals n.UserID
                                      select new ViewWeightTask
                                                   {
                                                       CheckID = m.CheckID,
                                                       WeightUser = n.UserName,
                                                       State = m.State,
                                                       Type = m.Type,
                                                       WeightTaskID = m.WeightTaskID
                                                   }).OrderBy(m => m.WeightUser).OrderBy(m => m.Type).ToList();
                ViewBag.weightTaskList = weightTaskList; //权重任务情况

                return View(check);
            }
        }

        /// <summary>
        /// 从指标库导入指标
        /// </summary>
        /// <param name="CheckID"></param>
        /// <param name="factoryID"></param>
        /// <returns></returns>
        public ActionResult ImportCheckItems(string CheckID, string factoryID)
        {
            if (!string.IsNullOrEmpty(CheckID) && !string.IsNullOrEmpty(factoryID))
            {
                using (DBContext db = new DBContext())
                {
                    var itemList = db.Item.Include("ItemPoints").Where(m => m.ItemFactoryID == factoryID).ToList();
                    foreach (var item in itemList)
                    {
                        CheckItem checkItem = new CheckItem
                        {
                            CheckItemID = Guid.NewGuid().ToString(),
                            CheckID = CheckID,
                            CheckItemName = item.ItemName,
                            CheckItemCode = item.ItemCode,
                            CheckItemNumber = item.ItemNumber,
                            CheckStandard = item.CheckStandard,
                            CheckItemPoints = new List<CheckItemPoint>()
                        };

                        foreach (var point in item.ItemPoints)
                        {
                            CheckItemPoint checkItemPoint = new CheckItemPoint
                            {
                                CheckItemPointID = Guid.NewGuid().ToString(),
                                CheckItemID = checkItem.CheckItemID,
                                CheckItemPointName = point.ItemPointName
                            };
                            checkItem.CheckItemPoints.Add(checkItemPoint);
                        }

                        db.CheckItem.Add(checkItem);
                    }
                    var check = db.Check.Where(m => m.CheckID == CheckID).FirstOrDefault();
                    check.State = "1";
                    db.SaveChanges();
                }
                return Json(new { result = true, message = "" });
            }

            return Json(new { result = false, message = "" });
        }

        /// <summary>
        /// 分配专家计算权重
        /// </summary>
        /// <param name="CheckID"></param>
        /// <param name="proUser"></param>
        /// <returns></returns>
        public ActionResult AssignPro(string CheckID, string proUser)
        {
            if (!string.IsNullOrEmpty(CheckID) && !string.IsNullOrEmpty(proUser))
            {
                using (DBContext db = new DBContext())
                {
                    foreach (string userid in proUser.Split(','))
                    {
                        WeightTask wTask = new WeightTask
                        {
                            WeightTaskID = Guid.NewGuid().ToString(),
                            CheckID = CheckID,
                            WeightUser = userid,
                            Type = "0",
                            State = "0"
                        };
                        db.WeightTask.Add(wTask);
                    }

                    var check = db.Check.Where(m => m.CheckID == CheckID).FirstOrDefault();
                    check.State = "2";
                    db.SaveChanges();
                }
                return Json(new { result = true, message = "" });
            }

            return Json(new { result = false, message = "" });
        }

        /// <summary>
        /// 获取修改权重的指标及权重值
        /// </summary>
        /// <param name="CheckID"></param>
        /// <param name="CheckItemID"></param>
        /// <returns></returns>
        public ActionResult GetModifyCheckItemWeight(string CheckID, string CheckItemID)
        {
            if (!string.IsNullOrEmpty(CheckID) && !string.IsNullOrEmpty(CheckItemID))
            {
                using (DBContext db = new DBContext())
                {
                    CheckItem checkItem = db.CheckItem.Where(m => m.CheckID == CheckID && m.CheckItemID == CheckItemID).SingleOrDefault();
                    string code = checkItem.CheckItemCode;
                    string beginCode = code.Substring(0, code.Length - 4);
                    List<CheckItem> checkItemList = db.CheckItem.Where(m => m.CheckID == CheckID && m.CheckItemCode.Length == code.Length && m.CheckItemCode.StartsWith(beginCode)).OrderBy(m => m.CheckItemCode).ToList();

                    return Json(new { result = true, message = "", data = checkItemList });
                }
            }

            return Json(new { result = false, message = "" });
        }

        /// <summary>
        /// 保存修改的权重
        /// </summary>
        /// <param name="txtCheckID"></param>
        /// <param name="txtCheckItemID"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        public ActionResult SaveCheckItemWeight(string txtCheckID, string txtCheckItemID, FormCollection collection)
        {
            if (!string.IsNullOrEmpty(txtCheckItemID))
            {
                using (DBContext db = new DBContext())
                {
                    CheckItem checkItem = db.CheckItem.Where(m => m.CheckID == txtCheckID && m.CheckItemID == txtCheckItemID).SingleOrDefault();
                    string code = checkItem.CheckItemCode;
                    string beginCode = code.Substring(0, code.Length - 4);
                    List<CheckItem> checkItemList = db.CheckItem.Where(m => m.CheckID == txtCheckID && m.CheckItemCode.Length == code.Length && m.CheckItemCode.StartsWith(beginCode)).OrderBy(m => m.CheckItemCode).ToList();

                    foreach (CheckItem item in checkItemList)
                    {
                        if (!string.IsNullOrEmpty(collection["checkItemweight_" + item.CheckItemID]))
                        {
                            item.Weight = double.Parse(collection["checkItemweight_" + item.CheckItemID]);
                        }
                    }

                    db.SaveChanges();

                    return Json(new { result = true, message = "", data = checkItemList });
                }
            }

            return Json(new { result = false, message = "" });
        }

        /// <summary>
        /// 评估用户页面
        /// </summary>
        /// <param name="CheckID"></param>
        /// <returns></returns>
        public ActionResult CheckUserView(string CheckID)
        {
            using (DBContext db = new DBContext())
            {
                string companyID = SessionService.CompanyID;
                var check = db.Check.Where(m => m.CompanyID.Equals(companyID) && m.CheckID.Equals(CheckID)).SingleOrDefault();
                ViewBag.CheckName = check.CheckName;
                ViewBag.CheckState = check.State;

                var checkUserList = (from m in db.CheckTask
                                     join u in db.User on m.UserID equals u.UserID
                                     join u2 in db.User on m.Checker equals u2.UserID into temp
                                     from tt in temp.DefaultIfEmpty()
                                     select new ViewCheckUser
                                     {
                                         CheckID = m.CheckID,
                                         UserID = m.UserID,
                                         State = m.State,
                                         Checker = m.Checker,
                                         CheckerName = tt == null ? "" : tt.UserName,
                                         UserName = u.UserName,
                                         UserDepartmentName = u.Department,
                                         CheckTaskID = m.CheckTaskID,
                                         CreatedTime = m.CreatedTime,
                                         FinishedTime = m.FinishedTime
                                     }).OrderBy(m => m.UserName).OrderBy(m => m.UserDepartmentName).ToList();

                ViewBag.checkUserList = checkUserList;

                var checker = db.User.Where(m => m.CompanyID == companyID && m.Roles.Contains("chk")).ToList();
                ViewBag.checkerList = checker;

                return View(check);
            }
        }

        /// <summary>
        /// 添加评估用户视图
        /// </summary>
        /// <param name="CheckID"></param>
        /// <returns></returns>
        public ActionResult CheckUserAddView(string CheckID, Request request)
        {
            ViewBag.CheckID = CheckID;
            using (DBContext db = new DBContext())
            {
                string companyID = SessionService.CompanyID;

                var UserList = db.User.Where(m => m.CompanyID == companyID).OrderBy(m => m.UserName).OrderBy(m => m.Department).ToPagedList(request.PageIndex, request.PageSize);

                return View(UserList);
            }
        }

        /// <summary>
        /// 添加部分评估人员
        /// </summary>
        /// <param name="CheckID"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public ActionResult CheckUserAddPart(string CheckID, string UserID)
        {
            int number = 0;
            if (!string.IsNullOrEmpty(CheckID) && !string.IsNullOrEmpty(UserID))
            {
                using (DBContext db = new DBContext())
                {
                    if (UserID.Split(',').Count() > 0)
                    {
                        //创建支付
                        Pay pay = new Pay
                        {
                            PayID = Guid.NewGuid().ToString(),
                            CompanyID = SessionService.CompanyID,
                            CreatedTime = DateTime.Now,
                            State = "0"
                        };
                        db.Pay.Add(pay);

                        foreach (string temp in UserID.Split(','))
                        {
                            var exist = db.CheckTask.Where(m => m.UserID == temp && m.CheckID == CheckID).Count();
                            if (exist == 0)
                            {
                                CheckTask task = new CheckTask
                                {
                                    CheckTaskID = Guid.NewGuid().ToString(),
                                    CheckID = CheckID,
                                    UserID = temp,
                                    CreatedTime = DateTime.Now,
                                    State = "0"
                                };
                                db.CheckTask.Add(task);

                                PayCheckTask payCheckTask = new PayCheckTask
                                {
                                    PayCheckTaskID = Guid.NewGuid().ToString(),
                                    CheckTaskID = task.CheckTaskID,
                                    PayID = pay.PayID
                                };
                                db.PayCheckTask.Add(payCheckTask);
                                number++;
                            }
                        }

                        pay.Money = number * Solution.Common.Constants.PerUserPrice;
                    }

                    db.SaveChanges();
                }
            }

            return Json(new { result = true, message = "添加人员" + number + "个" });
        }

        /// <summary>
        /// 添加公司所有人员
        /// </summary>
        /// <param name="CheckID"></param>
        /// <returns></returns>
        public ActionResult CheckUserAddAll(string CheckID)
        {
            int number = 0;
            if (!string.IsNullOrEmpty(CheckID))
            {
                using (DBContext db = new DBContext())
                {
                    var userList = db.User.Where(m => m.CompanyID == SessionService.CompanyID).ToList();
                    if (userList.Count() > 0)
                    {
                        //创建支付
                        Pay pay = new Pay
                        {
                            PayID = Guid.NewGuid().ToString(),
                            CompanyID = SessionService.CompanyID,
                            CreatedTime = DateTime.Now,
                            State = "0"
                        };
                        db.Pay.Add(pay);

                        foreach (var user in userList)
                        {
                            string temp = user.UserID;
                            var exist = db.CheckTask.Where(m => m.UserID == temp && m.CheckID == CheckID).Count();
                            if (exist == 0)
                            {
                                CheckTask task = new CheckTask
                                {
                                    CheckTaskID = Guid.NewGuid().ToString(),
                                    CheckID = CheckID,
                                    UserID = temp,
                                    CreatedTime = DateTime.Now,
                                    State = "0"
                                };
                                db.CheckTask.Add(task);

                                PayCheckTask payCheckTask = new PayCheckTask
                                {
                                    PayCheckTaskID = Guid.NewGuid().ToString(),
                                    CheckTaskID = task.CheckTaskID,
                                    PayID = pay.PayID
                                };
                                db.PayCheckTask.Add(payCheckTask);
                                number++;
                            }
                        }

                        pay.Money = number * Solution.Common.Constants.PerUserPrice;
                    }

                    db.SaveChanges();
                }
            }

            return Json(new { result = true, message = "添加人员" + number + "个" });
        }

        /// <summary>
        /// 评估结果页面
        /// </summary>
        /// <param name="CheckID"></param>
        /// <returns></returns>
        public ActionResult CheckResultView(string CheckID)
        {
            ViewBag.CheckID = CheckID;
            using (DBContext db = new DBContext())
            {
                var list = (from m in db.User
                            join n in db.CheckTask on m.UserID equals n.UserID
                            join x in db.CheckUserScore on n.CheckTaskID equals x.CheckTaskID
                            where n.CheckID == CheckID && n.State == "3"
                            select new ViewCheckResult
                            {
                                UserID = m.UserID,
                                UserName = m.UserName,
                                Department = m.Department,
                                Score = x.Score,
                                Remark = x.Remark
                            }).OrderBy(m => m.UserName).OrderBy(m => m.Department).OrderByDescending(m => m.Score).ToList();

                return View(list);
            }
        }

        /// <summary>
        /// 查看用户详细得分情况
        /// </summary>
        /// <param name="CheckID"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public ActionResult CheckResultDetailView(string CheckID, string UserID)
        {
            ViewBag.CheckID = CheckID;

            using (DBContext db = new DBContext())
            {
                var user = db.User.Where(m => m.UserID == UserID).SingleOrDefault();
                ViewBag.Department = user.Department;
                ViewBag.UserName = user.UserName;
                var list = (from n in db.CheckTask
                            join x in db.CheckItemScore on n.CheckTaskID equals x.CheckTaskID
                            join h in db.CheckItem on x.CheckItemID equals h.CheckItemID
                            where n.CheckID == CheckID && n.UserID == UserID && n.State == "3"
                            select new ViewCheckResultDetail
                            {
                                CheckItemID = x.CheckItemID,
                                CheckItemName = h.CheckItemName,
                                CheckItemNumber = h.CheckItemNumber,
                                CheckItemCode = h.CheckItemCode,
                                Score = x.Score,
                                CheckMark = x.CheckMark
                            }).OrderBy(m => m.CheckItemCode).ToList();

                return View(list);
            }
        }

        /// <summary>
        /// 导出结果
        /// </summary>
        /// <param name="CheckID">检查ID</param>
        /// <returns></returns>
        public ActionResult ExportCheckResult(string CheckID)
        {
            using (DBContext db = new DBContext())
            {
                var model = db.Check.Where(m => m.CheckID == CheckID).SingleOrDefault();
                if (model != null && !string.IsNullOrEmpty(model.ExcelFileName))
                {
                    var fileName = Server.MapPath(string.Concat("~/Content/file/", model.ExcelFileName));
                    return File(fileName, "application/ms-excel", string.Concat(DateTime.Now.ToString("yyyyMMddHHmmssffff"), ".xlsx"));
                }
                else
                {
                    //新生成Excel文件
                    System.Data.DataTable dt = new System.Data.DataTable("ExportData");
                    dt.Columns.Add("ItemName");
                    dt.Rows.Add("");
                    var checkItemList = db.CheckItem.Where(m => m.CheckID == CheckID).OrderBy(m => m.CheckItemCode).ToList();
                    for (int i = 0; i < checkItemList.Count; i++)
                    {
                        dt.Rows.Add(checkItemList[i].CheckItemCode);
                        dt.Rows[i + 1]["ItemName"] = string.Concat(checkItemList[i].CheckItemNumber, " ", checkItemList[i].CheckItemName);
                    }

                    dt.Rows.Add("Total");
                    dt.Rows[checkItemList.Count + 1]["ItemName"] = "总分";

                    var checkTask = (from m in db.CheckTask
                                     join n in db.User on m.UserID equals n.UserID
                                     where m.CheckID == CheckID && m.State == "3"
                                     select new
                                     {
                                         UserID = m.UserID,
                                         Department = n.Department,
                                         UserName = n.UserName,
                                         CheckTaskID = m.CheckTaskID
                                     }).ToList();

                    for (int j = 0; j < checkTask.Count; j++)
                    {
                        dt.Columns.Add(checkTask[j].UserID);
                        dt.Rows[0][j + 1] = checkTask[j].UserName;

                        string uID = checkTask[j].UserID;
                        var list = (from n in db.CheckTask
                                    join x in db.CheckItemScore on n.CheckTaskID equals x.CheckTaskID
                                    join h in db.CheckItem on x.CheckItemID equals h.CheckItemID
                                    where n.CheckID == CheckID && n.UserID == uID && n.State == "3"
                                    select new ViewCheckResultDetail
                                    {
                                        CheckItemID = x.CheckItemID,
                                        CheckItemName = h.CheckItemName,
                                        CheckItemNumber = h.CheckItemNumber,
                                        CheckItemCode = h.CheckItemCode,
                                        Score = x.Score,
                                        CheckMark = x.CheckMark
                                    }).OrderBy(m => m.CheckItemCode).ToList();

                        for (int k = 0; k < list.Count; k++)
                        {
                            dt.Rows[k + 1][j + 1] = list[k].Score;
                        }
                        string cTaskID = checkTask[j].CheckTaskID;
                        var userScore = db.CheckUserScore.Where(m => m.CheckTaskID == cTaskID).SingleOrDefault();
                        if (userScore != null)
                        {
                            dt.Rows[list.Count + 1][j + 1] = userScore.Score;
                        }
                    }

                    //将DataTable保存为excel
                    string FileName = string.Concat(DateTime.Now.ToString("yyyyMMddHHmmssffff"), ".xlsx");
                    string FilePath = Server.MapPath(string.Concat("~/Content/file/", FileName));
                    if (ExportExcel(dt, FilePath))
                    {
                        model.ExcelFileName = FileName;
                    }

                    db.SaveChanges();

                    return File(FilePath, "application/ms-excel", string.Concat(DateTime.Now.ToString("yyyyMMddHHmmssffff"), ".xlsx"));
                }
            }
        }

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool ExportExcel(System.Data.DataTable dt, string path)
        {
            bool succeed = false;
            if (dt != null)
            {
                Microsoft.Office.Interop.Excel.Application xlApp = null;
                try
                {
                    xlApp = new Microsoft.Office.Interop.Excel.Application();
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                if (xlApp != null)
                {
                    try
                    {
                        Microsoft.Office.Interop.Excel.Workbook xlBook = xlApp.Workbooks.Add(true);
                        object oMissing = System.Reflection.Missing.Value;
                        Microsoft.Office.Interop.Excel.Worksheet xlSheet = null;

                        xlSheet = (Worksheet)xlBook.Worksheets[1];
                        xlSheet.Name = dt.TableName;

                        int rowIndex = 1;
                        int colIndex = 1;
                        int colCount = dt.Columns.Count;
                        int rowCount = dt.Rows.Count;

                        //列名的处理
                        //for (int i = 0; i < colCount; i++)
                        //{
                        //    xlSheet.Cells[rowIndex, colIndex] = dt.Columns[i].ColumnName;
                        //    colIndex++;
                        //}
                        //列名加粗显示
                        //xlSheet.get_Range(xlSheet.Cells[rowIndex, 1], xlSheet.Cells[rowIndex, colCount]).Font.Bold = true;
                        //xlSheet.get_Range(xlSheet.Cells[rowIndex, 1], xlSheet.Cells[rowCount + 1, colCount]).Font.Name = "Arial";
                        //xlSheet.get_Range(xlSheet.Cells[rowIndex, 1], xlSheet.Cells[rowCount + 1, colCount]).Font.Size = "10";
                        //rowIndex++;

                        for (int i = 0; i < rowCount; i++)
                        {
                            colIndex = 1;
                            for (int j = 0; j < colCount; j++)
                            {
                                xlSheet.Cells[rowIndex, colIndex] = dt.Rows[i][j].ToString();
                                colIndex++;
                            }
                            rowIndex++;
                        }
                        xlSheet.Cells.EntireColumn.AutoFit();

                        xlApp.DisplayAlerts = false;
                        path = Path.GetFullPath(path);
                        xlBook.SaveCopyAs(path);
                        xlBook.Close(false, null, null);
                        xlApp.Workbooks.Close();
                        Marshal.ReleaseComObject(xlSheet);
                        Marshal.ReleaseComObject(xlBook);
                        xlBook = null;
                        succeed = true;
                    }
                    catch (Exception ex)
                    {
                        succeed = false;
                    }
                    finally
                    {
                        xlApp.Quit();
                        Marshal.ReleaseComObject(xlApp);
                        int generation = System.GC.GetGeneration(xlApp);
                        xlApp = null;
                        System.GC.Collect(generation);
                    }
                }
            }
            return succeed;
        }

        /// <summary>
        /// 分配检查员
        /// </summary>
        /// <param name="CheckID"></param>
        /// <param name="checkerID"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public ActionResult AssignChecker(string CheckID, string checkerID, string UserID)
        {
            if (!string.IsNullOrEmpty(CheckID) && !string.IsNullOrEmpty(checkerID) && !string.IsNullOrEmpty(UserID))
            {
                using (DBContext db = new DBContext())
                {
                    var task = db.CheckTask.Where(m => m.CheckID == CheckID && UserID.Contains(m.UserID)).ToList();
                    foreach (var temp in task)
                    {
                        temp.Checker = checkerID;
                        temp.State = "2";
                    }
                    db.SaveChanges();
                }
            }
            return Json(new { result = true, message = "" });
        }

        /// <summary>
        /// 支付页面
        /// </summary>
        /// <returns></returns>
        public ActionResult PayListView()
        {
            using (DBContext db = new DBContext())
            {
                var list = db.Pay.Where(m => m.CompanyID == SessionService.CompanyID).ToList();
                return View(list);
            }
        }

        /// <summary>
        /// 支付完成
        /// </summary>
        /// <param name="PayID"></param>
        /// <returns></returns>
        public ActionResult PayFinished(string PayID)
        {
            using (DBContext db = new DBContext())
            {
                var pay = db.Pay.Where(m => m.PayID == PayID).SingleOrDefault();
                pay.State = "1";
                db.SaveChanges();
            }
            return Json(new { result = true, message = "" });
        }
    }
}