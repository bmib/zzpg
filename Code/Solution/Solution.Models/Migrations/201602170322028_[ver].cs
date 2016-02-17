namespace Solution.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ver : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.T_CheckItemPoint", "ItemPointOrder", c => c.Int(nullable: false));
            AddColumn("dbo.T_ItemPoint", "ItemPointOrder", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.T_ItemPoint", "ItemPointOrder");
            DropColumn("dbo.T_CheckItemPoint", "ItemPointOrder");
        }
    }
}
