using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Solution.Models
{
    /// <summary>
    /// 指标
    /// </summary>
     [Table("T_Item")]
    public class Item
    {
         [DisplayName("指标ID")]
         [Key]
         [StringLength(36)]
         public string ItemID { get; set; }

         [DisplayName("指标编号")]
         [StringLength(128)]
         public string ItemNumber { get; set; }

         [DisplayName("指标名称")]
         [StringLength(128)]
         public string ItemName { get; set; }

         [DisplayName("评分标准")]
         [StringLength(4000)]
         public string CheckStandard { get; set; }

         [DisplayName("所属指标库ID")]
         [StringLength(36)]
         public string ItemFactoryID { get; set; }

         [ForeignKey("ItemFactoryID")]
         public virtual ItemFactory ItemFactory { get; set; }

         [DisplayName("Code")]
         [StringLength(36)]
         public string ItemCode { get; set; }

         public virtual List<ItemPoint> ItemPoints { get; set; }
    }
}
