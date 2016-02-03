namespace Solution.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ver : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.T_CheckItem", "WeightPoint", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.T_CheckItem", "WeightPoint");
        }
    }
}
