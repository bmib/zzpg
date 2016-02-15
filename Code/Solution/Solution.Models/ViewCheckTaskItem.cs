namespace Solution.Models
{
    public class ViewCheckTaskItem
    {
        public string CheckTaskID { get; set; }

        public string CheckItemID { get; set; }

        public string CheckItemNumber { get; set; }

        public string CheckItemName { get; set; }

        public string CheckStandard { get; set; }

        public string CheckItemCode { get; set; }

        public string CheckItemType { get; set; }

        public string CheckItemScoreID { get; set; }

        public double Score { get; set; }

        public string CheckMark { get; set; }

        public string CheckPoint{get; set;}

        public bool HaveChild { get; set; }

        public double Weight { get; set; }
    }
}
