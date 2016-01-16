namespace Solution.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ver : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.T_ItemFactory", "CompanyID", c => c.String(maxLength: 36));
            AddColumn("dbo.T_ItemFactory", "State", c => c.Int(nullable: false));
            CreateIndex("dbo.T_ItemFactory", "CompanyID");
            AddForeignKey("dbo.T_ItemFactory", "CompanyID", "dbo.T_Company", "CompanyID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.T_ItemFactory", "CompanyID", "dbo.T_Company");
            DropIndex("dbo.T_ItemFactory", new[] { "CompanyID" });
            DropColumn("dbo.T_ItemFactory", "State");
            DropColumn("dbo.T_ItemFactory", "CompanyID");
        }
    }
}
