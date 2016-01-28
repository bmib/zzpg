using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Solution.Models
{
    [Table("T_PayCheckTask")]
    public class PayCheckTask
    {
        [DisplayName("ID")]
        [Key]
        [StringLength(36)]
        public string PayCheckTaskID { get; set; }

        [DisplayName("支付ID")]
        [StringLength(36)]
        public string PayID { get; set; }

        [DisplayName("检查任务ID")]
        [StringLength(36)]
        public string CheckTaskID { get; set; }
    }
}
