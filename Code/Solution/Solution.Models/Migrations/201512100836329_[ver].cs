namespace Solution.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ver : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.T_Company",
                c => new
                    {
                        CompanyID = c.String(nullable: false, maxLength: 36),
                        CompanyName = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.CompanyID);
            
            CreateTable(
                "dbo.T_Department",
                c => new
                    {
                        DepartmentID = c.String(nullable: false, maxLength: 36),
                        DepartmentName = c.String(maxLength: 128),
                        CompanyID = c.String(maxLength: 36),
                        ParentDepartmentID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => t.DepartmentID)
                .ForeignKey("dbo.T_Company", t => t.CompanyID)
                .Index(t => t.CompanyID);
            
            CreateTable(
                "dbo.T_User",
                c => new
                    {
                        UserID = c.String(nullable: false, maxLength: 36),
                        UserName = c.String(maxLength: 128),
                        CompanyID = c.String(maxLength: 36),
                        Email = c.String(maxLength: 128),
                        Password = c.String(maxLength: 128),
                        Salt = c.String(maxLength: 36),
                        Roles = c.String(maxLength: 512),
                    })
                .PrimaryKey(t => t.UserID)
                .ForeignKey("dbo.T_Company", t => t.CompanyID)
                .Index(t => t.CompanyID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.T_User", "CompanyID", "dbo.T_Company");
            DropForeignKey("dbo.T_Department", "CompanyID", "dbo.T_Company");
            DropIndex("dbo.T_User", new[] { "CompanyID" });
            DropIndex("dbo.T_Department", new[] { "CompanyID" });
            DropTable("dbo.T_User");
            DropTable("dbo.T_Department");
            DropTable("dbo.T_Company");
        }
    }
}
