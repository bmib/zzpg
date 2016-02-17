using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Solution.Models
{
    /// <summary>
    /// 指标考核点
    /// </summary>
     [Table("T_ItemPoint")]
    public class ItemPoint
    {
         [DisplayName("指标考核点ID")]
         [Key]
         [StringLength(36)]
         public string ItemPointID { get; set; }

         [DisplayName("所属指标ID")]
         [StringLength(36)]
         public string ItemID { get; set; }

         [ForeignKey("ItemID")]
         public virtual Item Item { get; set; }

         [DisplayName("指标考核点名称")]
         [StringLength(1024)]
         public string ItemPointName { get; set; }

         [DisplayName("排序号")]
         public int ItemPointOrder { get; set; }
    }
}
