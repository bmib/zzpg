namespace Solution.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ver : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.T_CheckItem", "CheckItemType", c => c.String(maxLength: 1));
            AddColumn("dbo.T_Item", "ItemType", c => c.String(maxLength: 1));
        }
        
        public override void Down()
        {
            DropColumn("dbo.T_Item", "ItemType");
            DropColumn("dbo.T_CheckItem", "CheckItemType");
        }
    }
}
