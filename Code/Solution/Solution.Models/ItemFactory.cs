using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Solution.Models
{
    /// <summary>
    /// 指标库
    /// </summary>
    [Table("T_ItemFactory")]
    public class ItemFactory
    {
        [DisplayName("指标库ID")]
        [Key]
        [StringLength(36)]
        public string ItemFactoryID { get; set; }

        [DisplayName("指标库名称")]
        [StringLength(128)]
        public string ItemFactoryName { get; set; }
    }
}
