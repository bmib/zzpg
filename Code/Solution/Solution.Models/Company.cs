using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Solution.Models
{
    [Table("T_Company")]
    public class Company
    {
        [DisplayName("公司ID")]
        [Key]
        [StringLength(36)]
        public string CompanyID { get; set; }

        [DisplayName("公司名称")]
        [StringLength(128)]
        public string CompanyName { get; set; }
    }
}
