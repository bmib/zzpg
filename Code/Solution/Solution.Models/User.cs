using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Solution.Models
{
    [Table("T_User")]
    public class User
    {
        [DisplayName("用户ID")]
        [Key]
        [StringLength(36)]
        public string UserID { get; set; }

        [DisplayName("用户姓名")]
        [StringLength(128)]
        public string UserName { get; set; }

        [DisplayName("所属公司ID")]
        [StringLength(36)]
        public string CompanyID { get; set; }

        [ForeignKey("CompanyID")]
        public virtual Company Company { get; set; }

        [DisplayName("EMAIL地址")]
        [StringLength(128)]
        public string Email { get; set; }

        [DisplayName("登录密码")]
        [StringLength(128)]
        public string Password { get; set; }

        [DisplayName("秘钥")]
        [StringLength(36)]
        public string Salt { get; set; }

        /// <summary>
        /// 用户角色
        /// 公司管理员：admin
        /// 专家：pro
        /// 检查员：chk
        /// 多个角色以逗号隔开
        /// </summary>
        [DisplayName("角色")]
        [StringLength(512)]
        public string Roles { get; set; }
    }
}
