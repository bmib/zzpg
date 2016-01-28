namespace Solution.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ver : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.T_Check",
                c => new
                    {
                        CheckID = c.String(nullable: false, maxLength: 36),
                        CheckName = c.String(maxLength: 512),
                        State = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => t.CheckID);
            
            CreateTable(
                "dbo.T_CheckItem",
                c => new
                    {
                        CheckItemID = c.String(nullable: false, maxLength: 36),
                        CheckItemNumber = c.String(maxLength: 128),
                        CheckItemName = c.String(maxLength: 128),
                        CheckStandard = c.String(maxLength: 4000),
                        CheckID = c.String(maxLength: 36),
                        CheckItemCode = c.String(maxLength: 36),
                        WeightOrder = c.Int(nullable: false),
                        Weight = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.CheckItemID)
                .ForeignKey("dbo.T_Check", t => t.CheckID)
                .Index(t => t.CheckID);
            
            CreateTable(
                "dbo.T_CheckItemPoint",
                c => new
                    {
                        CheckItemPointID = c.String(nullable: false, maxLength: 36),
                        CheckItemID = c.String(maxLength: 36),
                        CheckItemPointName = c.String(maxLength: 1024),
                    })
                .PrimaryKey(t => t.CheckItemPointID)
                .ForeignKey("dbo.T_CheckItem", t => t.CheckItemID)
                .Index(t => t.CheckItemID);
            
            CreateTable(
                "dbo.T_CheckItemScore",
                c => new
                    {
                        CheckItemScoreID = c.String(nullable: false, maxLength: 36),
                        CheckItemID = c.String(maxLength: 36),
                        CheckTaskUser = c.String(maxLength: 36),
                        Score = c.Int(nullable: false),
                        Checker = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => t.CheckItemScoreID);
            
            CreateTable(
                "dbo.T_CheckTask",
                c => new
                    {
                        CheckTaskID = c.String(nullable: false, maxLength: 36),
                        CheckID = c.String(maxLength: 36),
                        UserID = c.String(maxLength: 36),
                        Checker = c.String(maxLength: 36),
                        State = c.String(maxLength: 1),
                        CreatedTime = c.DateTime(nullable: false),
                        FinishedTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.CheckTaskID)
                .ForeignKey("dbo.T_Check", t => t.CheckID)
                .Index(t => t.CheckID);
            
            CreateTable(
                "dbo.T_CheckUserScore",
                c => new
                    {
                        CheckUserScoreID = c.String(nullable: false, maxLength: 36),
                        CheckID = c.String(maxLength: 36),
                        CheckTaskUser = c.String(maxLength: 36),
                        Score = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.CheckUserScoreID)
                .ForeignKey("dbo.T_Check", t => t.CheckID)
                .Index(t => t.CheckID);
            
            CreateTable(
                "dbo.T_Pay",
                c => new
                    {
                        PayID = c.String(nullable: false, maxLength: 36),
                        CompanyID = c.String(maxLength: 36),
                        Money = c.Double(nullable: false),
                        State = c.String(maxLength: 1),
                        CreatedTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.PayID)
                .ForeignKey("dbo.T_Company", t => t.CompanyID)
                .Index(t => t.CompanyID);
            
            CreateTable(
                "dbo.T_PayCheckTask",
                c => new
                    {
                        PayCheckTaskID = c.String(nullable: false, maxLength: 36),
                        PayID = c.String(maxLength: 36),
                        CheckTaskID = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => t.PayCheckTaskID);
            
            CreateTable(
                "dbo.T_WeightMark",
                c => new
                    {
                        WeightMarkID = c.String(nullable: false, maxLength: 36),
                        CheckItemID = c.String(maxLength: 36),
                        Score = c.Int(nullable: false),
                        WeightUser = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => t.WeightMarkID);
            
            CreateTable(
                "dbo.T_WeightPoint",
                c => new
                    {
                        WeightPointID = c.String(nullable: false, maxLength: 36),
                        CheckItemID = c.String(maxLength: 36),
                        Point = c.Double(nullable: false),
                        WeightUser = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => t.WeightPointID);
            
            CreateTable(
                "dbo.T_WeightTask",
                c => new
                    {
                        WeightTaskID = c.String(nullable: false, maxLength: 36),
                        CheckID = c.String(maxLength: 36),
                        WeightUser = c.String(maxLength: 36),
                        Type = c.String(maxLength: 36),
                        State = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => t.WeightTaskID)
                .ForeignKey("dbo.T_Check", t => t.CheckID)
                .Index(t => t.CheckID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.T_WeightTask", "CheckID", "dbo.T_Check");
            DropForeignKey("dbo.T_Pay", "CompanyID", "dbo.T_Company");
            DropForeignKey("dbo.T_CheckUserScore", "CheckID", "dbo.T_Check");
            DropForeignKey("dbo.T_CheckTask", "CheckID", "dbo.T_Check");
            DropForeignKey("dbo.T_CheckItemPoint", "CheckItemID", "dbo.T_CheckItem");
            DropForeignKey("dbo.T_CheckItem", "CheckID", "dbo.T_Check");
            DropIndex("dbo.T_WeightTask", new[] { "CheckID" });
            DropIndex("dbo.T_Pay", new[] { "CompanyID" });
            DropIndex("dbo.T_CheckUserScore", new[] { "CheckID" });
            DropIndex("dbo.T_CheckTask", new[] { "CheckID" });
            DropIndex("dbo.T_CheckItemPoint", new[] { "CheckItemID" });
            DropIndex("dbo.T_CheckItem", new[] { "CheckID" });
            DropTable("dbo.T_WeightTask");
            DropTable("dbo.T_WeightPoint");
            DropTable("dbo.T_WeightMark");
            DropTable("dbo.T_PayCheckTask");
            DropTable("dbo.T_Pay");
            DropTable("dbo.T_CheckUserScore");
            DropTable("dbo.T_CheckTask");
            DropTable("dbo.T_CheckItemScore");
            DropTable("dbo.T_CheckItemPoint");
            DropTable("dbo.T_CheckItem");
            DropTable("dbo.T_Check");
        }
    }
}
