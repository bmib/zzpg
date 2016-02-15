using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Solution.Models
{
    [Table("T_CheckItem")]
    public class CheckItem
    {
        [DisplayName("ID")]
        [Key]
        [StringLength(36)]
        public string CheckItemID { get; set; }

        [DisplayName("指标编号")]
        [StringLength(128)]
        public string CheckItemNumber { get; set; }

        [DisplayName("指标名称")]
        [StringLength(128)]
        public string CheckItemName { get; set; }

        [DisplayName("评分标准")]
        [StringLength(4000)]
        public string CheckStandard { get; set; }

        [DisplayName("所属检查ID")]
        [StringLength(36)]
        public string CheckID { get; set; }

        [ForeignKey("CheckID")]
        public virtual Check Check { get; set; }

        [DisplayName("Code")]
        [StringLength(36)]
        public string CheckItemCode { get; set; }

        /// <summary>
        /// 由高到低排序
        /// </summary>
        [DisplayName("权重排序分值")]
        public int WeightOrder { get; set; }

        [DisplayName("权重重要程度分值")]
        public double WeightPoint { get; set; }

        [DisplayName("权重")]
        public double Weight { get; set; }

        public virtual List<CheckItemPoint> CheckItemPoints { get; set; }

        /// <summary>
        /// 0：普通指标，得分为1-4分的指标
        /// 1：特殊指标，得分为0和1的指标
        /// </summary>
        [DisplayName("ItemType")]
        [StringLength(1)]
        public string CheckItemType { get; set; }
    }
}
