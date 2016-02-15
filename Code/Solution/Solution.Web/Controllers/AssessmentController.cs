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
    public class AssessmentController : BaseController
    {
        /// <summary>
        /// 评估检查页面
        /// </summary>
        /// <returns></returns>
        public ActionResult CheckListView()
        {
            using (DBContext db = new DBContext())
            {
                var list = (from m in db.CheckTask
                            join n in db.User on m.UserID equals n.UserID
                            where m.State == "2" || m.State == "3"
                            select new ViewCheckTask
                            {
                                CheckTaskID = m.CheckTaskID,
                                UserID = m.UserID,
                                UserName = n.UserName,
                                Department = n.Department,
                                CheckID = m.CheckID,
                                Check = m.Check,
                                Checker = m.Checker,
                                State = m.State
                            }).OrderBy(m => m.UserName).OrderBy(m => m.Department).OrderBy(m => m.State).ToList();

                return View(list);
            }
        }

        /// <summary>
        /// 检查
        /// </summary>
        /// <param name="CheckTaskID"></param>
        /// <returns></returns>
        public ActionResult CheckDetailView(string CheckTaskID)
        {
            using (DBContext db = new DBContext())
            {
                var list = (from m in db.CheckTask
                            join x in db.CheckItem on m.CheckID equals x.CheckID
                            join n in db.CheckItemScore on new { CheckItemID = x.CheckItemID, CheckTaskID = m.CheckTaskID } equals new { CheckItemID = n.CheckItemID, CheckTaskID = n.CheckTaskID } into temp
                            from tt in temp.DefaultIfEmpty()
                            where m.CheckTaskID == CheckTaskID
                            select new ViewCheckTaskItem
                            {
                                CheckTaskID = m.CheckTaskID,
                                CheckItemID = x.CheckItemID,
                                CheckItemCode = x.CheckItemCode,
                                CheckItemName = x.CheckItemName,
                                CheckItemNumber = x.CheckItemNumber,
                                CheckStandard = x.CheckStandard,
                                CheckItemType = x.CheckItemType,
                                CheckMark = tt.CheckMark == null ? "" : tt.CheckMark,
                                Score = tt.Score == null ? 0 : tt.Score,
                                CheckItemScoreID = tt.CheckItemScoreID == null ? "" : tt.CheckItemScoreID
                            }).OrderBy(m => m.CheckItemCode).ToList();

                foreach (var item in list)
                {
                    //获取考核点
                    var points = db.CheckItemPoint.Where(m => m.CheckItemID == item.CheckItemID).ToList();
                    string point = string.Empty;
                    foreach (var p in points)
                    {
                        point += p.CheckItemPointName + ",";
                    }
                    item.CheckPoint = point.TrimEnd(',');

                    //判断是否有下级节点
                    if (list.Where(m => m.CheckItemCode.StartsWith(item.CheckItemCode) && m.CheckItemCode.Length > item.CheckItemCode.Length).Count() > 0)
                        item.HaveChild = true;
                    else
                        item.HaveChild = false;
                }

                var checkResult = db.CheckUserScore.Where(m => m.CheckTaskID == CheckTaskID).SingleOrDefault();
                if (checkResult == null)
                    ViewBag.Remark = string.Empty;
                else
                    ViewBag.Remark = checkResult.Remark;

                return View(list);
            }
        }

        /// <summary>
        /// 保存评估结果
        /// </summary>
        /// <param name="collect"></param>
        /// <param name="CheckTaskID"></param>
        /// <param name="Remark"></param>
        /// <returns></returns>
        public ActionResult SaveCheckResult(FormCollection collect, string CheckTaskID, string Remark)
        {
            using (DBContext db = new DBContext())
            {
                var checkTask = db.CheckTask.Where(m => m.CheckTaskID == CheckTaskID).SingleOrDefault();
                string checkID = checkTask.CheckID;
                var list = db.CheckItem.Where(m => m.CheckID == checkID).ToList();
                foreach (var temp in list)
                {
                    if (!string.IsNullOrEmpty(collect["radio_" + temp.CheckItemID]))
                    {
                        var existItemScore = db.CheckItemScore.Where(m => m.CheckTaskID == CheckTaskID && m.CheckItemID == temp.CheckItemID).SingleOrDefault();
                        if (existItemScore == null)
                        {
                            CheckItemScore score = new CheckItemScore
                            {
                                CheckItemScoreID = Guid.NewGuid().ToString(),
                                CheckItemID = temp.CheckItemID,
                                CheckTaskID = CheckTaskID,
                                Score = int.Parse(collect["radio_" + temp.CheckItemID]),
                                CheckMark = collect["mark_" + temp.CheckItemID]
                            };
                            db.CheckItemScore.Add(score);
                        }
                        else
                        {
                            existItemScore.Score = int.Parse(collect["radio_" + temp.CheckItemID]);
                            existItemScore.CheckMark = collect["mark_" + temp.CheckItemID];
                        }
                    }
                }

                var existUserScore = db.CheckUserScore.Where(m => m.CheckTaskID == CheckTaskID).SingleOrDefault();
                if (existUserScore == null)
                {
                    CheckUserScore uScore = new CheckUserScore
                    {
                        CheckUserScoreID = Guid.NewGuid().ToString(),
                        CheckTaskID = CheckTaskID,
                        Remark = collect["RemarkTextarea"]
                    };
                    db.CheckUserScore.Add(uScore);
                }
                else
                {
                    existUserScore.Remark = collect["RemarkTextarea"];
                }

                db.SaveChanges();
                return Json(new { result = true, message = "" });
            }
        }

        /// <summary>
        /// 提交评估结果
        /// </summary>
        /// <param name="collect"></param>
        /// <param name="CheckTaskID"></param>
        /// <param name="Remark"></param>
        /// <returns></returns>
        public ActionResult SubmitCheckResult(FormCollection collect, string CheckTaskID, string Remark)
        {
            using (DBContext db = new DBContext())
            {
                #region 保存
                var checkTask = db.CheckTask.Where(m => m.CheckTaskID == CheckTaskID).SingleOrDefault();
                string checkID = checkTask.CheckID;
                var list = db.CheckItem.Where(m => m.CheckID == checkID).ToList();
                foreach (var temp in list)
                {
                    if (!string.IsNullOrEmpty(collect["radio_" + temp.CheckItemID]))
                    {
                        var existItemScore = db.CheckItemScore.Where(m => m.CheckTaskID == CheckTaskID && m.CheckItemID == temp.CheckItemID).SingleOrDefault();
                        if (existItemScore == null)
                        {
                            CheckItemScore score = new CheckItemScore
                            {
                                CheckItemScoreID = Guid.NewGuid().ToString(),
                                CheckItemID = temp.CheckItemID,
                                CheckTaskID = CheckTaskID,
                                Score = int.Parse(collect["radio_" + temp.CheckItemID]),
                                CheckMark = collect["mark_" + temp.CheckItemID]
                            };
                            db.CheckItemScore.Add(score);
                        }
                        else
                        {
                            existItemScore.Score = int.Parse(collect["radio_" + temp.CheckItemID]);
                            existItemScore.CheckMark = collect["mark_" + temp.CheckItemID];
                        }
                    }
                }

                var existUserScore = db.CheckUserScore.Where(m => m.CheckTaskID == CheckTaskID).SingleOrDefault();
                if (existUserScore == null)
                {
                    CheckUserScore uScore = new CheckUserScore
                    {
                        CheckUserScoreID = Guid.NewGuid().ToString(),
                        CheckTaskID = CheckTaskID,
                        Remark = collect["RemarkTextarea"]
                    };
                    db.CheckUserScore.Add(uScore);
                }
                else
                {
                    existUserScore.Remark = collect["RemarkTextarea"];
                }

                db.SaveChanges();

                #endregion

                #region 计算用户的总分
                var listAll = (from m in db.CheckTask
                               join x in db.CheckItem on m.CheckID equals x.CheckID
                               join n in db.CheckItemScore on new { CheckItemID = x.CheckItemID, CheckTaskID = m.CheckTaskID } equals new { CheckItemID = n.CheckItemID, CheckTaskID = n.CheckTaskID } into temp
                               from tt in temp.DefaultIfEmpty()
                               where m.CheckTaskID == CheckTaskID
                               select new ViewCheckTaskItem
                               {
                                   CheckTaskID = m.CheckTaskID,
                                   CheckItemID = x.CheckItemID,
                                   CheckItemCode = x.CheckItemCode,
                                   CheckItemName = x.CheckItemName,
                                   CheckItemNumber = x.CheckItemNumber,
                                   CheckItemType = x.CheckItemType,
                                   CheckStandard = x.CheckStandard,
                                   CheckMark = tt.CheckMark == null ? "" : tt.CheckMark,
                                   Score = tt.Score == null ? 0 : tt.Score,
                                   CheckItemScoreID = tt.CheckItemScoreID == null ? "" : tt.CheckItemScoreID,
                                   Weight = x.Weight
                               }).OrderBy(m => m.CheckItemCode).ToList();

                var normalList = listAll.Where(m => m.CheckItemType == "0").ToList();
                var specialList = listAll.Where(m => m.CheckItemType == "1").ToList();

                var listItemScore = db.CheckItemScore.Where(m => m.CheckTaskID == CheckTaskID).ToList();
                var FirstLevelList = listAll.Where(m => m.CheckItemCode.Length == 4).OrderBy(m => m.CheckItemCode).ToList();
                double total = 0;
                foreach (var firstTemp in FirstLevelList)
                {
                    if (firstTemp.Score == 0)
                    {
                        CountUserScore(firstTemp.CheckItemCode, normalList, listItemScore, db);
                    }
                    total = total + firstTemp.Weight * firstTemp.Score;
                }

                total = Math.Round(total, 2);

                var updateModel = db.CheckUserScore.Where(m => m.CheckTaskID == CheckTaskID).SingleOrDefault();
                updateModel.Score = total;
                #endregion

                #region 修改任务状态
                checkTask.State = "3";
                #endregion

                db.SaveChanges();

                return Json(new { result = true, message = "" });
            }
        }

        /// <summary>
        /// 循环计算指标的得分
        /// </summary>
        /// <param name="currentCode"></param>
        /// <param name="listAll"></param>
        /// <param name="listItemScore"></param>
        public void CountUserScore(string currentCode, List<ViewCheckTaskItem> listAll, List<CheckItemScore> listItemScore, DBContext db)
        {

            //获取下级节点
            double total = 0;
            var NextItems = listAll.Where(m => m.CheckItemCode.StartsWith(currentCode) && m.CheckItemCode.Length == (currentCode.Length + 4)).OrderBy(m => m.CheckItemCode).ToList();
            foreach (var temp in NextItems)
            {
                if (temp.Score == 0)
                {
                    CountUserScore(temp.CheckItemCode, listAll, listItemScore, db);
                }

                total = total + temp.Weight * temp.Score;
            }

            //更新分数
            var model = listAll.Where(m => m.CheckItemCode == currentCode).SingleOrDefault();
            model.Score = total;

            var dataModel = listItemScore.Where(m => m.CheckTaskID == model.CheckTaskID && m.CheckItemID == model.CheckItemID).SingleOrDefault();
            if (dataModel == null)
            {
                CheckItemScore itemScore = new CheckItemScore
                {
                    CheckItemScoreID = Guid.NewGuid().ToString(),
                    CheckTaskID = model.CheckTaskID,
                    CheckItemID = model.CheckItemID,
                    Score = total
                };
                db.CheckItemScore.Add(itemScore);
            }
            else
            {
                dataModel.Score = total;
            }
        }
    }
}