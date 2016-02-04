using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Solution.Models
{
    [Table("T_CheckItemScore")]
    public class CheckItemScore
    {
        [DisplayName("ID")]
        [Key]
        [StringLength(36)]
        public string CheckItemScoreID { get; set; }

        [DisplayName("指标ID")]
        [StringLength(36)]
        public string CheckItemID { get; set; }

        [DisplayName("检查任务ID")]
        [StringLength(36)]
        public string CheckTaskID { get; set; }

        [DisplayName("检查员评分")]
        public double Score { get; set; }

        [DisplayName("评分说明")]
        [StringLength(4000)]
        public string CheckMark { get; set; }
    }
}
