using Solution.Common;
using Solution.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Solution.Services;

namespace Solution.Web.Controllers
{
    public class HomeController : BaseController
    {
        /// <summary>
        /// 系统首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 登录页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// 登录操作
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public ActionResult LoginAction(FormCollection collection)
        {
            if (!SessionService.GetValue("VerifyImageCode").Equals(collection["inputCheckCode"].Trim()))
            {
                return Json(new { result = false, message = "验证码不正确" });
            }
            string Email = collection["inputEmail"].Trim();
            string Password = collection["inputPassword"].Trim();
            using (DBContext db = new DBContext())
            {
                //检测email
                var user = db.User.Where(m => m.Email.Equals(Email)).SingleOrDefault();
                if (user == null)
                {
                    return Json(new { result = false, message = "Email不存在。" });
                }

                //获得密匙
                string salt = user.Salt;
                //得到经过加密后的"密码"
                string espassword = EncryptPassWord.EncryptPwd(Password, salt);

                //对比密码
                if(!espassword.Equals(user.Password))
                {
                    return Json(new { result = false, message = "密码不正确。" });
                }

                SessionService.SetValue("UserID", user.UserID);
                SessionService.SetValue("User", user);
                SessionService.SetValue("CompanyID", user.CompanyID);
            }
            return Json(new { result = true, message = "" });
        }

        /// <summary>
        /// 注册页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// 注册提交
        /// </summary>
        /// <returns></returns>
        public ActionResult RegisterSubmit(FormCollection collection)
        {
            //检测验证码
            if (!SessionService.GetValue("VerifyImageCode").Equals(collection["inputCheckCode"].Trim()))
            {
                return Json(new { result = false, message = "验证码不正确。" });
            }

            string CompanyName = collection["inputCompany"].Trim();
            string Email = collection["inputEmail"].Trim();
            string Password = collection["inputPassword"].Trim();
            using (DBContext db = new DBContext())
            {
                //检测公司名字
                if (db.Company.Where(m => m.CompanyName.Equals(CompanyName)).Count() > 0)
                {
                    return Json(new { result = false, message = "公司已经存在，请联系公司管理员开通权限。" });
                }
                //检测email
                if (db.User.Where(m => m.Email.Equals(Email)).Count() > 0)
                {
                    return Json(new { result = false, message = "Email已经存在。" });
                }

                //获得密匙
                string salt = EncryptPassWord.CreateSalt();
                //得到经过加密后的"密码"
                string espassword = EncryptPassWord.EncryptPwd(Password, salt);

                string companyID = Guid.NewGuid().ToString();
                string userID = Guid.NewGuid().ToString();
                Company com = new Company
                {
                    CompanyID = companyID,
                    CompanyName = CompanyName
                };
                db.Company.Add(com);

                User user = new User
                {
                    UserID = userID,
                    Email = Email,
                    Password = espassword,
                    Salt = salt,
                    CompanyID = companyID,
                    Company = com,
                    Roles = "admin"
                };
                db.User.Add(user);

                db.SaveChanges();
            }
            return Json(new { result = true, message = "" });
        }

        /// <summary>
        /// 验证码图片
        /// </summary>
        /// <returns></returns>
        public ActionResult VerifyImage()
        {
            var s1 = new ValidateCode_Style10();
            string code = string.Empty;
            byte[] bytes = s1.CreateImage(out code);
            SessionService.SetValue("VerifyImageCode",code);

            return File(bytes, @"image/jpeg");
        }

        /// <summary>
        /// 错误页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Error()
        {
            return View();
        }
    }
}