using System.ComponentModel;

namespace Solution.Models
{
    public class ViewCheckItem
    {
        public string CheckItemID { get; set; }

        [DisplayName("指标编号")]
        public string CheckItemNumber { get; set; }

        [DisplayName("指标名称")]
        public string CheckItemName { get; set; }

        [DisplayName("评分标准")]
        public string CheckStandard { get; set; }

        [DisplayName("所属检查ID")]
        public string CheckID { get; set; }

        [DisplayName("Code")]
        public string CheckItemCode { get; set; }

        /// <summary>
        /// 由高到低排序
        /// </summary>
        [DisplayName("权重排序")]
        public int WeightOrder { get; set; }

        public double WeightPoint { get; set; }

        [DisplayName("权重")]
        public double Weight { get; set; }

        public int Score { get; set; }

        public double Point { get; set; }
    }
}
