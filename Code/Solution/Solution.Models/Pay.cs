using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Solution.Models
{
    [Table("T_Pay")]
    public class Pay
    {
        [DisplayName("ID")]
        [Key]
        [StringLength(36)]
        public string PayID { get; set; }

        [DisplayName("公司ID")]
        [StringLength(36)]
        public string CompanyID { get; set; }

        [ForeignKey("CompanyID")]
        public virtual Company Company { get; set; }

        [DisplayName("金额")]
        public double Money { get; set; }

        /// <summary>
        /// 0-未支付
        /// 1-支付完成待确认
        /// 2-支付成功
        /// </summary>
        [DisplayName("状态")]
        [StringLength(1)]
        public string State { get; set; }

        [DisplayName("创建时间")]
        public DateTime? CreatedTime { get; set; }
    }
}
