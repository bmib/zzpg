using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Solution.Models
{
    public class ViewWeightTask
    {
        [DisplayName("ID")]
        [Key]
        [StringLength(36)]
        public string WeightTaskID { get; set; }

        [DisplayName("检查ID")]
        [StringLength(36)]
        public string CheckID { get; set; }

        [DisplayName("专家ID")]
        [StringLength(128)]
        public string WeightUser { get; set; }

        /// <summary>
        /// 0为评分任务
        /// 1为重要性任务
        /// </summary>
        [DisplayName("任务类型")]
        [StringLength(1)]
        public string Type { get; set; }

        /// <summary>
        /// 0为未完成
        /// 1为已完成
        /// </summary>
        [DisplayName("状态")]
        [StringLength(1)]
        public string State { get; set; }
    }
}
