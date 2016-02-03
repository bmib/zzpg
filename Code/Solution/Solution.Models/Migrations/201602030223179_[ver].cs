namespace Solution.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ver : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.T_Check", "CreatedTime", c => c.DateTime());
            AlterColumn("dbo.T_CheckTask", "CreatedTime", c => c.DateTime());
            AlterColumn("dbo.T_CheckTask", "FinishedTime", c => c.DateTime());
            AlterColumn("dbo.T_Pay", "CreatedTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.T_Pay", "CreatedTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.T_CheckTask", "FinishedTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.T_CheckTask", "CreatedTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.T_Check", "CreatedTime", c => c.DateTime(nullable: false));
        }
    }
}
