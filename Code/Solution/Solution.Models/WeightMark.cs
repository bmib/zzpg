using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Solution.Models
{
    /// <summary>
    /// 权重打分表，排序用
    /// </summary>
    [Table("T_WeightMark")]
    public class WeightMark
    {
        [DisplayName("ID")]
        [Key]
        [StringLength(36)]
        public string WeightMarkID { get; set; }

        [DisplayName("指标ID")]
        [StringLength(36)]
        public string CheckItemID { get; set; }

        [DisplayName("得分")]
        public int Score { get; set; }

        [DisplayName("专家ID")]
        [StringLength(36)]
        public string WeightUser { get; set; }
    }
}
