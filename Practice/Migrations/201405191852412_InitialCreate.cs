namespace Practice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        Time = c.DateTime(nullable: false),
                        Contact = c.String(),
                        Topic = c.String(),
                        Comments = c.String(),
                        SourceReliability_Id = c.Int(),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SourceReliabilities", t => t.SourceReliability_Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.SourceReliability_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.SourceReliabilities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Events", "User_Id", "dbo.Users");
            DropForeignKey("dbo.Events", "SourceReliability_Id", "dbo.SourceReliabilities");
            DropIndex("dbo.Events", new[] { "User_Id" });
            DropIndex("dbo.Events", new[] { "SourceReliability_Id" });
            DropTable("dbo.Users");
            DropTable("dbo.SourceReliabilities");
            DropTable("dbo.Events");
        }
    }
}
