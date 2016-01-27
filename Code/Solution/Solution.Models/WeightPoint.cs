using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Solution.Models
{
    [Table("T_WeightPoint")]
    public class WeightPoint
    {
        [DisplayName("ID")]
        [Key]
        [StringLength(36)]
        public string WeightPointID { get; set; }

        [DisplayName("指标ID")]
        [StringLength(36)]
        public string CheckItemID { get; set; }

        [DisplayName("得分")]
        public double Point { get; set; }

        [DisplayName("专家ID")]
        [StringLength(36)]
        public string WeightUser { get; set; }
    }
}
