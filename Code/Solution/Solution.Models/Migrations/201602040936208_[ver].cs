namespace Solution.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ver : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.T_CheckItemScore", "Score", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.T_CheckItemScore", "Score", c => c.Int(nullable: false));
        }
    }
}
