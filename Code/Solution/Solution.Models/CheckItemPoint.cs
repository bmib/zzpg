using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Solution.Models
{
    [Table("T_CheckItemPoint")]
    public class CheckItemPoint
    {
        [DisplayName("ID")]
        [Key]
        [StringLength(36)]
        public string CheckItemPointID { get; set; }

        [DisplayName("所属指标ID")]
        [StringLength(36)]
        public string CheckItemID { get; set; }

        [ForeignKey("CheckItemID")]
        public virtual CheckItem CheckItem { get; set; }

        [DisplayName("指标考核点名称")]
        [StringLength(1024)]
        public string CheckItemPointName { get; set; }

        [DisplayName("排序号")]
        public int ItemPointOrder { get; set; }
    }
}
