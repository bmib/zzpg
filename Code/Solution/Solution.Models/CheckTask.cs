using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Solution.Models
{
    [Table("T_CheckTask")]
    public class CheckTask
    {
        [DisplayName("ID")]
        [Key]
        [StringLength(36)]
        public string CheckTaskID { get; set; }

        [DisplayName("检查ID")]
        [StringLength(36)]
        public string CheckID { get; set; }

        [ForeignKey("CheckID")]
        public virtual Check Check { get; set; }

        [DisplayName("被考核人员ID")]
        [StringLength(36)]
        public string UserID { get; set; }

        [DisplayName("检查员ID")]
        [StringLength(36)]
        public string Checker { get; set; }

        /// <summary>
        /// 0为未完成
        /// 1为已完成
        /// </summary>
        [DisplayName("状态")]
        [StringLength(1)]
        public string State { get; set; }

        [DisplayName("创建时间")]
        public DateTime CreatedTime { get; set; }

        [DisplayName("完成时间")]
        public DateTime FinishedTime { get; set; }
    }
}
