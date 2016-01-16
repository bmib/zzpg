using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Solution.Models
{
    [Table("T_Department")]
    public class Department
    {
        [DisplayName("部门ID")]
        [Key]
        [StringLength(36)]
        public string DepartmentID { get; set; }

        [DisplayName("部门名称")]
        [StringLength(128)]
        public string DepartmentName { get; set; }

        [DisplayName("部门Code")]
        [StringLength(36)]
        public string DepartmentCode { get; set; }

        [DisplayName("所属公司ID")]
        [StringLength(36)]
        public string CompanyID { get; set; }
    }
}
