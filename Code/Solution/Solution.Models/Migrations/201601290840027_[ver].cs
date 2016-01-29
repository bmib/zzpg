namespace Solution.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ver : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.T_WeightTask", "Type", c => c.String(maxLength: 1));
            AlterColumn("dbo.T_WeightTask", "State", c => c.String(maxLength: 1));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.T_WeightTask", "State", c => c.String(maxLength: 36));
            AlterColumn("dbo.T_WeightTask", "Type", c => c.String(maxLength: 36));
        }
    }
}
