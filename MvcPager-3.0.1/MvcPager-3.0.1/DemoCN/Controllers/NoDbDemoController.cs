﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web.Mvc;
using Webdiyer.MvcPagerDemo.Models;
using Webdiyer.WebControls.Mvc;

namespace Webdiyer.MvcPagerDemo.Controllers
{
    public class NoDbDemoController : Controller
    {
        public ActionResult Index()
        {
            return View("NoDbIndex");
        }


        #region Html Pager

        public ViewResult Basic(int id = 1)
        {
            return View(DemoData.AllArticles.OrderByDescending(a => a.PubDate).ToPagedList(id, 5));
        }

        public ViewResult QueryString(int pageIndex = 1)
        {
            return View(DemoData.AllArticles.OrderByDescending(a => a.PubDate).ToPagedList(pageIndex, 5));
        }

        public ActionResult CustomRouting(int pageindex = 1)
        {
            return View(DemoData.AllArticles.OrderByDescending(a => a.PubDate).ToPagedList(pageindex, 5));
        }
        

        public ActionResult UrlParams(int id = 1)
        {
            return View(DemoData.AllArticles.OrderByDescending(a => a.PubDate).ToPagedList(id, 5));
        }

        public ActionResult PageIndexBox(int id = 1)
        {
            return View(DemoData.AllArticles.OrderByDescending(a => a.PubDate).ToPagedList(id, 5));
        }


        public ActionResult ApplyCSS(int id = 1)
        {
                return View(new PagedList<string>(new string[0], id, 1, 80));
        }


        public ActionResult CustomInfo(int id = 1)
        {
            return View(DemoData.AllArticles.OrderByDescending(a => a.PubDate).ToPagedList(id, 5));
        }


        public ActionResult MultipleMvcPagers(int id = 1,int pageIndex=1)
        {
            const int pageSize = 5;
                var model=new CompositeArticles();
                model.ArticleList1 = DemoData.AllArticles.OrderByDescending(a => a.PubDate).ToPagedList(pageIndex, pageSize);
                model.ArticleList2 = DemoData.AllArticles.OrderByDescending(a => a.PubDate).ToPagedList(id, pageSize);
                return View(model);
        }

        public ActionResult Search(int id = 1, string kword = null)
        {
            var query = DemoData.AllArticles.AsQueryable();
                if (!string.IsNullOrWhiteSpace(kword))
                    query = query.Where(a => a.Title.Contains(kword));
                var model = query.OrderByDescending(a => a.PubDate).ToPagedList(id, 5);
                return View(model);
        }

        public ActionResult ContentPaging(int id = 1)
        {
            return View(new PagedArticle(DemoData.ArticleWithContent, id));
        }

        public ActionResult FirstPageUrl(int id = 1)
        {
            return View(DemoData.AllArticles.OrderByDescending(a => a.PubDate).ToPagedList(id, 5));
        }

        public ActionResult PageSize(int pagesize = 10, int pageindex = 1)
        {
            return View(DemoData.AllArticles.OrderByDescending(a => a.PubDate).ToPagedList(pageindex, pagesize));
        }

        public ViewResult CombinedMode(int id = 1)
        {
            return View(DemoData.AllArticles.OrderByDescending(a => a.PubDate).ToPagedList(id, 5));
        }

        public ActionResult NavButtonsPosition(int id = 1)
        {
            return View(new PagedList<string>(new string[0], id, 1, 80));
        }

        public ActionResult NumericPagerItemsFormat(int id = 1)
        {
            return View(new PagedList<string>(new string[0], id, 1, 80));
        }

        public ActionResult PagerItemsTemplate(int id = 1)
        {
            return View(new PagedList<string>(new string[0], id, 1, 80));
        }

        public ActionResult Multilingual(int id = 1)
        {
            var lang = Request.QueryString["Languages"];
            var langs = new Dictionary<string, string>();
            langs.Add("zh-CN", "简体中文");
            langs.Add("zh-TW", "繁体中文");
            langs.Add("en-US", "英文");
            ViewBag.Languages = new SelectList(langs, "key", "value", lang);
            Thread.CurrentThread.CurrentCulture = new CultureInfo(lang ?? "zh-CN");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(lang ?? "zh-CN");
            return View(new PagedList<string>(new string[0], id, 1, 12));
        }

        public ActionResult PageIndexError(int id = 1)
        {
            return View(new PagedList<string>(new string[0], id, 1, 6));
        }
        #endregion
        
        #region Ajax Pager

        public ActionResult AjaxPaging(int id = 1)
        {
            var model = DemoData.AllArticles.OrderByDescending(a => a.PubDate).ToPagedList(id, 5);
                if (Request.IsAjaxRequest())
                    return PartialView("_ArticleList", model);
                return View(model);
        }

        public ActionResult LoadByAjax(int id = 1)
        {
            var model = DemoData.AllArticles.OrderByDescending(a => a.PubDate).ToPagedList(id, 5);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_LoadByAjax", model);
            }
            return View();
        }

        public ActionResult AjaxPageIndexBox(int id = 1)
        {
            var model = DemoData.AllArticles.OrderByDescending(a => a.PubDate).ToPagedList(id, 5);
            if (Request.IsAjaxRequest())
                return PartialView("_AjaxPageIndexBox", model);
            return View(model);
        }

        public ActionResult AjaxEvents(int id = 1)
        {
            if (id == 2)
            {
                Response.StatusCode = 500;
                return Content("测试用的服务器端错误信息");
            }
            //throw new ApplicationException("测试用的服务器端错误信息");
            var model = DemoData.AllArticles.OrderByDescending(a => a.PubDate).ToPagedList(id, 5);
            if (Request.IsAjaxRequest())
                return PartialView("_AjaxEvents", model);
            return View(model);
        }

        public ActionResult AjaxInitData(int id = 1)
        {
            var model = DemoData.AllArticles.OrderByDescending(a => a.PubDate).ToPagedList(id, 5);
                if (Request.IsAjaxRequest())
                    return PartialView("_ArticleList", model);
                return View(model);
        }


        public ActionResult AjaxSearchPartialGet(string title, string author, string source, int id = 1)
        {
            var qry = DemoData.AllArticles.AsQueryable();
            if (!string.IsNullOrWhiteSpace(title))
                qry = qry.Where(a => a.Title.Contains(title));
            if (!string.IsNullOrWhiteSpace(author))
                qry = qry.Where(a => a.Author.Contains(author));
            if (!string.IsNullOrWhiteSpace(source))
                qry = qry.Where(a => a.Source.Contains(source));
            var model = qry.OrderByDescending(a => a.PubDate).ToPagedList(id, 5);
            return View(model);
        }

        public ActionResult AjaxSearchPartialPost(string title, string author, string source, int id = 1)
        {
            var qry = DemoData.AllArticles.AsQueryable();
            if (!string.IsNullOrWhiteSpace(title))
                qry = qry.Where(a => a.Title.Contains(title));
            if (!string.IsNullOrWhiteSpace(author))
                qry = qry.Where(a => a.Author.Contains(author));
            if (!string.IsNullOrWhiteSpace(source))
                qry = qry.Where(a => a.Source.Contains(source));
            var model = qry.OrderByDescending(a => a.PubDate).ToPagedList(id, 5);
            return View(model);
        }

        public ActionResult AjaxLoading(int id = 1)
        {
            var model = DemoData.AllArticles.OrderByDescending(a => a.PubDate).ToPagedList(id, 5);
                if (Request.IsAjaxRequest())
                {
                    System.Threading.Thread.Sleep(2000);
                    return PartialView("_AjaxLoading", model);
                }
                return View(model);
        }

        public ActionResult AjaxNoHistory(int id = 1)
        {
            return View(DemoData.AllArticles.OrderByDescending(a => a.PubDate).ToPagedList(id, 5));
        }

        public ActionResult AjaxPartialLoading(int id = 1)
        {
            return View(DemoData.AllArticles.OrderByDescending(a => a.PubDate).ToPagedList(id, 5));
        }

        public ActionResult MultipleAjaxPagers(int id = 1,int pageIndex=1)
        {
            const int pageSize = 5;
            var model = new CompositeArticles();
            model.ArticleList1 = DemoData.AllArticles.OrderByDescending(a => a.PubDate).ToPagedList(pageIndex, pageSize);
            model.ArticleList2 = DemoData.AllArticles.OrderByDescending(a => a.PubDate).ToPagedList(id, pageSize);
            return View(model);
        }

        public ActionResult AjaxPagers(int id = 1, int pageIndex = 1,int page=1)
        {
            const int pageSize = 5;
            if (Request.IsAjaxRequest())
            {
                var target = Request.QueryString["target"];
                if (target == "articles")
                {
                    return PartialView("_AjaxArticles1",
                        DemoData.AllArticles.OrderByDescending(a => a.PubDate).ToPagedList(id, pageSize));
                }
                if (target == "articles2")
                {
                    return PartialView("_AjaxArticles2",
                        DemoData.AllArticles.OrderByDescending(a => a.PubDate).ToPagedList(pageIndex, pageSize));
                }
                return PartialView("_AjaxArticles3",
                    DemoData.AllArticles.OrderByDescending(a => a.PubDate).ToPagedList(page, pageSize));
            }
            var model = new CompositeArticles
            {
                ArticleList1 = DemoData.AllArticles.OrderByDescending(a => a.PubDate).ToPagedList(id, pageSize),
                ArticleList2 = DemoData.AllArticles.OrderByDescending(a => a.PubDate).ToPagedList(pageIndex, pageSize),
                ArticleList3 = DemoData.AllArticles.OrderByDescending(a => a.PubDate).ToPagedList(page, pageSize)
            };
            return View(model);
        }

        public ActionResult AjaxSearchGet(string title, string author, string source, int id = 1)
        {
            return ajaxSearchGetResult(title, author, source, id);
        }

        public ActionResult AjaxSearchHtmlGet(string title, string author, string source, int id = 1)
        {
            return ajaxSearchGetResult(title, author, source, id);
        }

        private ActionResult ajaxSearchGetResult(string title, string author, string source, int id = 1)
        {
            var qry = DemoData.AllArticles.AsQueryable();
                if (!string.IsNullOrWhiteSpace(title))
                    qry = qry.Where(a => a.Title.Contains(title));
                if (!string.IsNullOrWhiteSpace(author))
                    qry = qry.Where(a => a.Author.Contains(author));
                if (!string.IsNullOrWhiteSpace(source))
                    qry = qry.Where(a => a.Source.Contains(source));
                var model = qry.OrderByDescending(a => a.PubDate).ToPagedList(id, 5);
                if (Request.IsAjaxRequest())
                    return PartialView("_AjaxSearchGet", model);
                return View(model);
        }

        public ActionResult AjaxSearchPost(int id = 1)
        {
            var model = DemoData.AllArticles.OrderByDescending(a => a.PubDate).ToPagedList(id, 5);
                if (Request.IsAjaxRequest())
                    return PartialView("_AjaxSearchPost", model);
                return View(model);
        }

        [HttpPost]
        public ActionResult AjaxSearchPost(string title,string author,string source, int id = 1)
        {
            return ajaxSearchPostResult(title, author, source, id);
        }

        public ActionResult AjaxSearchHtmlPost(int id = 1)
        {
            var model = DemoData.AllArticles.OrderByDescending(a => a.PubDate).ToPagedList(id, 5);
                if (Request.IsAjaxRequest())
                    return PartialView("_AjaxSearchPost", model);
                return View(model);
        }
        [HttpPost]
        public ActionResult AjaxSearchHtmlPost(string title, string author, string source, int id = 1)
        {
            return ajaxSearchPostResult(title, author, source, id);
        }
        
        private ActionResult ajaxSearchPostResult(string title,string author,string source, int id = 1)
        {
            var qry = DemoData.AllArticles.AsQueryable();
                if (!string.IsNullOrWhiteSpace(title))
                    qry = qry.Where(a => a.Title.Contains(title));
                if (!string.IsNullOrWhiteSpace(author))
                    qry = qry.Where(a => a.Author.Contains(author));
                if (!string.IsNullOrWhiteSpace(source))
                    qry = qry.Where(a => a.Source.Contains(source));
                var model = qry.OrderByDescending(a => a.PubDate).ToPagedList(id, 5);
                if (Request.IsAjaxRequest())
                    return PartialView("_AjaxSearchPost", model);
                return View(model);
        }
        

        public ActionResult AjaxDegradation(int id = 1)
        {
            var model = DemoData.AllArticles.OrderByDescending(a => a.PubDate).ToPagedList(id, 5);
            if (Request.IsAjaxRequest())
                return PartialView("_Degradation", model);
            return View(model);
        }
        #endregion

        #region Javascript API
        public ActionResult JavascriptApiHtml(int id = 1)
        {
            return View(DemoData.AllArticles.OrderByDescending(a => a.PubDate).ToPagedList(id, 5));
        }

        public ActionResult JavascriptApiAjax(int id = 1)
        {
            var model = DemoData.AllArticles.OrderByDescending(a => a.PubDate).ToPagedList(id, 5);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_ArticleList",model);

            }
            return View(model);
        }



        public ActionResult CustomizeAjaxPager(int id = 1)
        {
            var model = DemoData.AllArticles.OrderByDescending(a => a.PubDate).ToPagedList(id, 5);
            if (Request.IsAjaxRequest())
                return PartialView("_AjaxPagerHidden", model);
            return View(model);
        }
        
        public ActionResult CustomizeHtmlPager(int id = 1)
        {
            return View(DemoData.AllArticles.OrderByDescending(a => a.PubDate).ToPagedList(id, 5));
        }

        #endregion

        #region PagedList and ToPagedList method samples

        public ActionResult Delete(int id = 1)
        {
            List<int> list = HttpContext.Cache["TestList"] as List<int>;
            if (list == null)
            {
                list = Enumerable.Range(1, 12).ToList();
                HttpContext.Cache["TestList"] = list;
            }
            return View(list.ToPagedList(id, 5));
        }

        [HttpPost]
        public ActionResult Delete(FormCollection values, int id=1)
        {
            if (values["resetList"] != null)
            {
                HttpContext.Cache.Remove("TestList");
                return RedirectToAction("Delete");
            }
            var list = HttpContext.Cache["TestList"] as List<int>;
            if (list == null)
                throw new ApplicationException("无法获取缓存的测试数据，请刷新重试");
            int[] delItems = Array.ConvertAll(values["val"].Split(','), i => int.Parse(i));
            list.RemoveAll(delItems.Contains);
            HttpContext.Cache["TestList"] = list;
            return View(list.ToPagedList(id, 5));
        }


        public ActionResult IPagedList(int id=1)
        {
            MyPagedList<int> list=new MyPagedList<int>(Enumerable.Range(1,88),id,5);
            return View(list);
        }

        #endregion
    }
}
