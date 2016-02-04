
namespace Solution.Models
{
    public class ViewCheckTask
    {
        public string CheckTaskID { get; set; }

        public string CheckID { get; set; }

        public  Check Check { get; set; }

        public string UserID { get; set; }

        public string Checker { get; set; }

        /// <summary>
        /// 0：未支付
        /// 1：未分配检查员
        /// 2：待评估
        /// 3：评估完成
        /// </summary>
        public string State { get; set; }

        public string Department { get; set; }

        public string UserName { get; set; }
    }
}
