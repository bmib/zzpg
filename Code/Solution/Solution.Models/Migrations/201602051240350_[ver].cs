namespace Solution.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ver : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.T_Check", "ExcelFileName", c => c.String(maxLength: 4000));
            DropColumn("dbo.T_CheckTask", "ExcelFileName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.T_CheckTask", "ExcelFileName", c => c.String(maxLength: 4000));
            DropColumn("dbo.T_Check", "ExcelFileName");
        }
    }
}
