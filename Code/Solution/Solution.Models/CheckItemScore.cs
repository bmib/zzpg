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

        [DisplayName("被考核人员ID")]
        [StringLength(36)]
        public string CheckTaskUser { get; set; }

        [DisplayName("检查员评分")]
        public int Score { get; set; }

        [DisplayName("检查员ID")]
        [StringLength(36)]
        public string Checker { get; set; }
    }
}
