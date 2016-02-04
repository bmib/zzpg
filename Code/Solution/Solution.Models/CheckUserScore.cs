using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Solution.Models
{
    [Table("T_CheckUserScore")]
    public class CheckUserScore
    {
        [DisplayName("ID")]
        [Key]
        [StringLength(36)]
        public string CheckUserScoreID { get; set; }

        [DisplayName("检查任务ID")]
        [StringLength(36)]
        public string CheckTaskID { get; set; }

        [DisplayName("得分")]
        public double Score { get; set; }

        [DisplayName("总体评价")]
        [StringLength(4000)]
        public string Remark { get; set; }
    }
}
