using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Solution.Models
{
    public class ViewCheckUser
    {
        [DisplayName("ID")]
        [StringLength(36)]
        public string CheckTaskID { get; set; }

        [DisplayName("检查ID")]
        [StringLength(36)]
        public string CheckID { get; set; }

        [DisplayName("被考核人员ID")]
        [StringLength(36)]
        public string UserID { get; set; }

        [DisplayName("检查员ID")]
        [StringLength(36)]
        public string Checker { get; set; }

        [DisplayName("被考核人员姓名")]
        [StringLength(128)]
        public string UserName { get; set; }

        [DisplayName("检查员姓名")]
        [StringLength(128)]
        public string CheckerName { get; set; }

        [DisplayName("被考核人员部门")]
        [StringLength(256)]
        public string UserDepartmentName { get; set; }

        /// <summary>
        /// 0为未完成
        /// 1为已完成
        /// </summary>
        [DisplayName("状态")]
        [StringLength(1)]
        public string State { get; set; }

        [DisplayName("创建时间")]
        public DateTime? CreatedTime { get; set; }

        [DisplayName("完成时间")]
        public DateTime? FinishedTime { get; set; }
    }
}
