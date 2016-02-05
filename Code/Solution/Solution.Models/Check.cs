using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Solution.Models
{
    /// <summary>
    /// 检查表
    /// </summary>
    [Table("T_Check")]
    public class Check
    {
        [DisplayName("ID")]
        [Key]
        [StringLength(36)]
        public string CheckID { get; set; }

        [DisplayName("检查名称")]
        [StringLength(512)]
        public string CheckName { get; set; }

        /// <summary>
        /// 0-创建完检查，还未导入指标
        /// 1-未分配专家
        /// 2-已分配专家，计算权重
        /// 3-专家计算权重已完成
        /// </summary>
        [DisplayName("状态")]
        [StringLength(2)]
        public string State { get; set; }

        [DisplayName("所属公司ID")]
        [StringLength(36)]
        public string CompanyID { get; set; }

        [ForeignKey("CompanyID")]
        public virtual Company Company { get; set; }

        [DisplayName("创建时间")]
        public DateTime? CreatedTime { get; set; }

        [DisplayName("创建人")]
        [StringLength(36)]
        public string CreatedUser { get; set; }

        [DisplayName("导出的文件地址")]
        [StringLength(4000)]
        public string ExcelFileName { get; set; }
    }
}
