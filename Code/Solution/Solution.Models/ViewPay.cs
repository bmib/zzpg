using System;

namespace Solution.Models
{
    public class ViewPay
    {
        public string PayID { get; set; }

        public string CompanyID { get; set; }

        public string CompanyName { get; set; }

        public double Money { get; set; }

        /// <summary>
        /// 0-未支付
        /// 1-支付完成待确认
        /// 2-支付成功
        /// </summary>
        public string State { get; set; }

        public DateTime? CreatedTime { get; set; }
    }
}
