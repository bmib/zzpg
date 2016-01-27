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

        [DisplayName("状态")]
        [StringLength(36)]
        public string State { get; set; }
    }
}
