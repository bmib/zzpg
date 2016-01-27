using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Solution.Models
{
    [Table("T_WeightTask")]
    public class WeightTask
    {
        [DisplayName("ID")]
        [Key]
        [StringLength(36)]
        public string WeightTaskID { get; set; }

        [DisplayName("检查ID")]
        [StringLength(36)]
        public string CheckID { get; set; }

        [ForeignKey("CheckID")]
        public virtual Check Check { get; set; }

        [DisplayName("专家ID")]
        [StringLength(36)]
        public string WeightUser { get; set; }

        /// <summary>
        /// 0为评分任务
        /// 1为重要性任务
        /// </summary>
        [DisplayName("任务类型")]
        [StringLength(36)]
        public string Type { get; set; }

        /// <summary>
        /// 0为未完成
        /// 1为已完成
        /// </summary>
        [DisplayName("状态")]
        [StringLength(36)]
        public string State { get; set; }
    }
}
