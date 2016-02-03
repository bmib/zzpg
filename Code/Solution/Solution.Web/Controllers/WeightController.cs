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

namespace Solution.Web.Controllers
{
    public class WeightController : BaseController
    {
        /// <summary>
        /// 指标排序任务页面
        /// </summary>
        /// <returns></returns>
        public ActionResult ItemSortView()
        {
            using (DBContext db = new DBContext())
            {
                var list = db.WeightTask.Include("Check").Where(m => m.WeightUser == SessionService.UserID && m.Type == "0").OrderBy(m => m.State).ToList();
                return View(list);
            }
        }

        /// <summary>
        /// 指标排序详细页面
        /// </summary>
        /// <param name="WeightTaskID"></param>
        /// <returns></returns>
        public ActionResult ItemSortDetailView(string WeightTaskID)
        {
            List<ViewCheckItemMark> result = new List<ViewCheckItemMark>();
            using (DBContext db = new DBContext())
            {
                var task = db.WeightTask.Include("Check").Where(m => m.WeightTaskID == WeightTaskID).SingleOrDefault();
                var listAll = (from m in db.CheckItem
                               join n in db.WeightMark on m.CheckItemID equals n.CheckItemID into temp
                               from t in temp.DefaultIfEmpty()
                               where m.CheckID == task.CheckID
                               select new ViewCheckItem
                               {
                                   CheckID = m.CheckID,
                                   CheckItemID = m.CheckItemID,
                                   CheckItemCode = m.CheckItemCode,
                                   CheckItemName = m.CheckItemName,
                                   CheckItemNumber = m.CheckItemNumber,
                                   CheckStandard = m.CheckStandard,
                                   WeightOrder = m.WeightOrder,
                                   Score = t.Score == null ? 0 : t.Score
                               }).OrderBy(m => m.CheckItemCode).ToList();
                var list = listAll.Where(m => m.CheckItemCode.Length == 4).OrderBy(m => m.CheckItemCode).ToList();
                result.Add(new ViewCheckItemMark { ViewCheckItemCode = "0001", ViewCheckItemList = list }); //添加一级指标

                for (int i = 0; i < list.Count; i++)
                {
                    GetNextViewCheckItem(listAll, result, list[i].CheckItemCode);
                }

                return View(result.OrderBy(m => m.ViewCheckItemCode).ToList());
            }
        }

        /// <summary>
        /// 循环获取
        /// </summary>
        /// <param name="listAll"></param>
        /// <param name="result"></param>
        /// <param name="currentCode"></param>
        public void GetNextViewCheckItem(List<ViewCheckItem> listAll, List<ViewCheckItemMark> result, string currentCode)
        {
            var curItem = listAll.Where(m => m.CheckItemCode == currentCode).FirstOrDefault();
            var length = curItem.CheckItemCode.Length + 4;
            var list = listAll.Where(m => m.CheckItemCode.Length == length && m.CheckItemCode.StartsWith(curItem.CheckItemCode)).OrderBy(m => m.CheckItemCode).ToList();
            if (list.Count > 0)
            {
                result.Add(new ViewCheckItemMark { ViewCheckItemCode = list[0].CheckItemCode, ViewCheckItemList = list });
                for (int i = 0; i < list.Count; i++)
                {
                    GetNextViewCheckItem(listAll, result, list[i].CheckItemCode);
                }
            }
        }

        /// <summary>
        /// 保存指标权重排序
        /// </summary>
        /// <param name="collect"></param>
        /// <param name="WeightTaskID"></param>
        /// <returns></returns>
        public ActionResult SaveWeightMark(FormCollection collect, string WeightTaskID)
        {
            using (DBContext db = new DBContext())
            {
                var weightTask = db.WeightTask.Where(m => m.WeightTaskID == WeightTaskID).SingleOrDefault();
                string checkID = weightTask.CheckID;
                var list = db.CheckItem.Where(m => m.CheckID == checkID).ToList();
                foreach (var temp in list)
                {
                    if (!string.IsNullOrEmpty(collect["input_" + temp.CheckItemID]))
                    {
                        //temp.WeightOrder = temp.WeightOrder + int.Parse(collect["input_" + temp.CheckItemID].ToString());

                        var exist = db.WeightMark.Where(m => m.WeightUser == SessionService.UserID && m.CheckItemID == temp.CheckItemID).FirstOrDefault();
                        if (exist == null)
                        {
                            WeightMark mark = new WeightMark
                            {
                                WeightMarkID = Guid.NewGuid().ToString(),
                                CheckItemID = temp.CheckItemID,
                                WeightUser = SessionService.UserID,
                                Score = int.Parse(collect["input_" + temp.CheckItemID].ToString())
                            };
                            db.WeightMark.Add(mark);

                        }
                        else
                        {
                            exist.Score = int.Parse(collect["input_" + temp.CheckItemID].ToString());
                        }
                    }
                }

                db.SaveChanges();
            }

            return Json(new { result = true, message = "" });
        }

        /// <summary>
        /// 提交指标权重排序
        /// </summary>
        /// <param name="collect"></param>
        /// <param name="WeightTaskID"></param>
        /// <returns></returns>
        public ActionResult SubmitWeightMark(FormCollection collect, string WeightTaskID)
        {
            using (DBContext db = new DBContext())
            {
                var weightTask = db.WeightTask.Where(m => m.WeightTaskID == WeightTaskID).SingleOrDefault();
                string checkID = weightTask.CheckID;
                var list = db.CheckItem.Where(m => m.CheckID == checkID).ToList();
                foreach (var temp in list)
                {
                    if (!string.IsNullOrEmpty(collect["input_" + temp.CheckItemID]))
                    {
                        temp.WeightOrder = temp.WeightOrder + int.Parse(collect["input_" + temp.CheckItemID].ToString());

                        var exist = db.WeightMark.Where(m => m.WeightUser == SessionService.UserID && m.CheckItemID == temp.CheckItemID).FirstOrDefault();
                        if (exist == null)
                        {
                            WeightMark mark = new WeightMark
                            {
                                WeightMarkID = Guid.NewGuid().ToString(),
                                CheckItemID = temp.CheckItemID,
                                WeightUser = SessionService.UserID,
                                Score = int.Parse(collect["input_" + temp.CheckItemID].ToString())
                            };
                            db.WeightMark.Add(mark);

                        }
                        else
                        {
                            exist.Score = int.Parse(collect["input_" + temp.CheckItemID].ToString());
                        }
                    }
                }

                weightTask.State = "1"; //将任务状态改为已完成

                //判断是否所有的任务都已经完成，如果完成，生成指标重要程度任务
                var count = db.WeightTask.Where(m => m.CheckID == weightTask.CheckID && m.State == "0").Count();
                if (count == 0)
                {
                    var taskList = db.WeightTask.Where(m => m.CheckID == weightTask.CheckID && m.Type == "0").ToList();
                    foreach (var t in taskList)
                    {
                        WeightTask task = new WeightTask
                        {
                            WeightTaskID = Guid.NewGuid().ToString(),
                            Type = "1",
                            CheckID = t.CheckID,
                            State = "0",
                            WeightUser = t.WeightUser
                        };
                        db.WeightTask.Add(task);
                    }
                }

                db.SaveChanges();
            }

            return Json(new { result = true, message = "" });
        }

        /// <summary>
        /// 指标重要程度任务页面
        /// </summary>
        /// <returns></returns>
        public ActionResult ItemDegreeView()
        {
            using (DBContext db = new DBContext())
            {
                var list = db.WeightTask.Include("Check").Where(m => m.WeightUser == SessionService.UserID && m.Type == "1").OrderBy(m => m.State).ToList();
                return View(list);
            }
        }

        /// <summary>
        /// 重要程度评估页面
        /// </summary>
        /// <param name="WeightTaskID"></param>
        /// <returns></returns>
        public ActionResult ItemDegreeDetailView(string WeightTaskID)
        {
            List<ViewCheckItemMark> result = new List<ViewCheckItemMark>();
            using (DBContext db = new DBContext())
            {
                var task = db.WeightTask.Include("Check").Where(m => m.WeightTaskID == WeightTaskID).SingleOrDefault();
                var listAll = (from m in db.CheckItem
                               join n in db.WeightPoint on m.CheckItemID equals n.CheckItemID into temp
                               from t in temp.DefaultIfEmpty()
                               where m.CheckID == task.CheckID
                               select new ViewCheckItem
                               {
                                   CheckID = m.CheckID,
                                   CheckItemID = m.CheckItemID,
                                   CheckItemCode = m.CheckItemCode,
                                   CheckItemName = m.CheckItemName,
                                   CheckItemNumber = m.CheckItemNumber,
                                   CheckStandard = m.CheckStandard,
                                   WeightOrder = m.WeightOrder,
                                   Point = t.Point == null ? 0 : t.Point
                               }).OrderBy(m => m.CheckItemCode).ToList();
                var list = listAll.Where(m => m.CheckItemCode.Length == 4).OrderByDescending(m => m.WeightOrder).ToList();
                result.Add(new ViewCheckItemMark { ViewCheckItemCode = "0001", ViewCheckItemList = list }); //添加一级指标

                for (int i = 0; i < list.Count; i++)
                {
                    GetNextViewCheckItemPoint(listAll, result, list[i].CheckItemCode);
                }

                return View(result.OrderBy(m => m.ViewCheckItemCode).ToList());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listAll"></param>
        /// <param name="result"></param>
        /// <param name="currentCode"></param>
        public void GetNextViewCheckItemPoint(List<ViewCheckItem> listAll, List<ViewCheckItemMark> result, string currentCode)
        {
            var curItem = listAll.Where(m => m.CheckItemCode == currentCode).FirstOrDefault();
            var length = curItem.CheckItemCode.Length + 4;
            var list = listAll.Where(m => m.CheckItemCode.Length == length && m.CheckItemCode.StartsWith(curItem.CheckItemCode)).OrderBy(m => m.CheckItemCode).OrderByDescending(m => m.WeightOrder).ToList();
            if (list.Count > 0)
            {
                result.Add(new ViewCheckItemMark { ViewCheckItemCode = list[0].CheckItemCode, ViewCheckItemList = list });
                for (int i = 0; i < list.Count; i++)
                {
                    GetNextViewCheckItemPoint(listAll, result, list[i].CheckItemCode);
                }
            }
        }

        /// <summary>
        /// 保存指标重要程度
        /// </summary>
        /// <param name="collect"></param>
        /// <param name="WeightTaskID"></param>
        /// <returns></returns>
        public ActionResult SaveWeightPoint(FormCollection collect, string WeightTaskID)
        {
            using (DBContext db = new DBContext())
            {
                var weightTask = db.WeightTask.Where(m => m.WeightTaskID == WeightTaskID).SingleOrDefault();
                string checkID = weightTask.CheckID;
                var list = db.CheckItem.Where(m => m.CheckID == checkID).ToList();
                foreach (var temp in list)
                {
                    if (!string.IsNullOrEmpty(collect["input_" + temp.CheckItemID]))
                    {
                        var exist = db.WeightPoint.Where(m => m.WeightUser == SessionService.UserID && m.CheckItemID == temp.CheckItemID).FirstOrDefault();
                        if (exist == null)
                        {
                            WeightPoint point = new WeightPoint
                            {
                                WeightPointID = Guid.NewGuid().ToString(),
                                CheckItemID = temp.CheckItemID,
                                WeightUser = SessionService.UserID,
                                Point = double.Parse(collect["input_" + temp.CheckItemID].ToString())
                            };
                            db.WeightPoint.Add(point);

                        }
                        else
                        {
                            exist.Point = double.Parse(collect["input_" + temp.CheckItemID].ToString());
                        }
                    }
                }

                db.SaveChanges();
            }

            return Json(new { result = true, message = "" });
        }

        /// <summary>
        /// 提交指标重要程度
        /// </summary>
        /// <param name="collect"></param>
        /// <param name="WeightTaskID"></param>
        /// <returns></returns>
        public ActionResult SubmitWeightPoint(FormCollection collect, string WeightTaskID)
        {
            using (DBContext db = new DBContext())
            {
                var weightTask = db.WeightTask.Where(m => m.WeightTaskID == WeightTaskID).SingleOrDefault();
                string checkID = weightTask.CheckID;
                var list = db.CheckItem.Where(m => m.CheckID == checkID).ToList();
                foreach (var temp in list)
                {
                    if (!string.IsNullOrEmpty(collect["input_" + temp.CheckItemID]))
                    {
                        temp.WeightPoint = temp.WeightPoint + double.Parse(collect["input_" + temp.CheckItemID].ToString());
                        var exist = db.WeightPoint.Where(m => m.WeightUser == SessionService.UserID && m.CheckItemID == temp.CheckItemID).FirstOrDefault();
                        if (exist == null)
                        {
                            WeightPoint point = new WeightPoint
                            {
                                WeightPointID = Guid.NewGuid().ToString(),
                                CheckItemID = temp.CheckItemID,
                                WeightUser = SessionService.UserID,
                                Point = double.Parse(collect["input_" + temp.CheckItemID].ToString())
                            };
                            db.WeightPoint.Add(point);

                        }
                        else
                        {
                            exist.Point = double.Parse(collect["input_" + temp.CheckItemID].ToString());
                        }
                    }
                }

                weightTask.State = "1"; //将任务状态改为已完成

                //判断是否所有的任务都已经完成，如果完成，计算权重
                var count = db.WeightTask.Where(m => m.CheckID == weightTask.CheckID && m.State == "0" && m.Type == "1").Count();
                if (count == 0)
                {
                    //计算权重
                    var checkItemAll = db.CheckItem.Where(m => m.CheckID == weightTask.CheckID).ToList();
                    var proCount = db.WeightTask.Where(m => m.CheckID == weightTask.CheckID && m.Type == "1").Count();
                    var checkItemTempList = checkItemAll.Where(m => m.CheckItemCode.Length == 4).OrderByDescending(m => m.WeightOrder).ToList();
                    CountWeight(checkItemTempList, proCount);
                    for (int i = 0; i < checkItemTempList.Count; i++)
                    {
                        GetNextLevel(checkItemAll, list[i].CheckItemCode, proCount);
                    }
                }

                db.SaveChanges();
            }

            return Json(new { result = true, message = "" });
        }

        /// <summary>
        /// 获取下一层级的指标
        /// </summary>
        /// <param name="checkItemAll"></param>
        /// <param name="currentCode"></param>
        /// <param name="proCount"></param>
        public void GetNextLevel(List<CheckItem> checkItemAll, string currentCode, int proCount)
        {
            var curItem = checkItemAll.Where(m => m.CheckItemCode == currentCode).FirstOrDefault();
            var length = curItem.CheckItemCode.Length + 4;
            var list = checkItemAll.Where(m => m.CheckItemCode.Length == length && m.CheckItemCode.StartsWith(curItem.CheckItemCode)).OrderBy(m => m.CheckItemCode).OrderByDescending(m => m.WeightOrder).ToList();
            if (list.Count > 0)
            {
                CountWeight(list, proCount);
                for (int i = 0; i < list.Count; i++)
                {
                    GetNextLevel(checkItemAll, list[i].CheckItemCode, proCount);
                }
            }
        }

        /// <summary>
        /// 计算权重
        /// 1.2,1.1,1.0,1.3,1.4
        /// </summary>
        /// <param name="items"></param>
        /// <param name="proCount"></param>
        public void CountWeight(List<CheckItem> items, int proCount)
        {
            items = items.OrderByDescending(m => m.WeightOrder).ToList();
            double total = 0;
            double count = 1;
            for (int i = 0; i < items.Count; i++)
            {
                for (int j = i; j < items.Count; j++)
                {
                    count = count * Math.Round(items[j].WeightPoint / proCount, 1);
                }
                total = total + count;
            }

            items[items.Count - 1].Weight = Math.Round(1 / (1 + total), 3);
            for(int k = items.Count-2; k>0; k--)
            {
                items[k].Weight = Math.Round(Math.Round(items[k].WeightPoint / proCount, 1) * items[k + 1].Weight, 3);
            }
        }
    }
}