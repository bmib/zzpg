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

        [DisplayName("检查ID")]
        [StringLength(36)]
        public string CheckID { get; set; }

        [ForeignKey("CheckID")]
        public virtual Check Check { get; set; }

        [DisplayName("被考核人员ID")]
        [StringLength(36)]
        public string CheckTaskUser { get; set; }

        [DisplayName("得分")]
        public double Score { get; set; }
    }
}
