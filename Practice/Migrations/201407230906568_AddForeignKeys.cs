namespace Practice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddForeignKeys : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Events", "SourceReliability_Id", "dbo.SourceReliabilities");
            DropForeignKey("dbo.Events", "User_Id", "dbo.Users");
            DropIndex("dbo.Events", new[] { "SourceReliability_Id" });
            DropIndex("dbo.Events", new[] { "User_Id" });
            AlterColumn("dbo.Events", "SourceReliability_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.Events", "User_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Events", "User_Id");
            CreateIndex("dbo.Events", "SourceReliability_Id");
            AddForeignKey("dbo.Events", "SourceReliability_Id", "dbo.SourceReliabilities", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Events", "User_Id", "dbo.Users", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Events", "User_Id", "dbo.Users");
            DropForeignKey("dbo.Events", "SourceReliability_Id", "dbo.SourceReliabilities");
            DropIndex("dbo.Events", new[] { "SourceReliability_Id" });
            DropIndex("dbo.Events", new[] { "User_Id" });
            AlterColumn("dbo.Events", "User_Id", c => c.Int());
            AlterColumn("dbo.Events", "SourceReliability_Id", c => c.Int());
            CreateIndex("dbo.Events", "User_Id");
            CreateIndex("dbo.Events", "SourceReliability_Id");
            AddForeignKey("dbo.Events", "User_Id", "dbo.Users", "Id");
            AddForeignKey("dbo.Events", "SourceReliability_Id", "dbo.SourceReliabilities", "Id");
        }
    }
}
