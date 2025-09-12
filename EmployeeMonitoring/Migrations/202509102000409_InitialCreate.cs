namespace EmployeeMonitoring.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.deps",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.persons",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        SecondName = c.String(),
                        LastName = c.String(),
                        DateEmploy = c.DateTime(),
                        DateUneploy = c.DateTime(),
                        StatusId = c.Int(nullable: false),
                        DepId = c.Int(nullable: false),
                        PostId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.deps", t => t.DepId)
                .ForeignKey("dbo.posts", t => t.PostId)
                .ForeignKey("dbo.status", t => t.StatusId)
                .Index(t => t.StatusId)
                .Index(t => t.DepId)
                .Index(t => t.PostId);
            
            CreateTable(
                "dbo.posts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.status",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.persons", "StatusId", "dbo.status");
            DropForeignKey("dbo.persons", "PostId", "dbo.posts");
            DropForeignKey("dbo.persons", "DepId", "dbo.deps");
            DropIndex("dbo.persons", new[] { "PostId" });
            DropIndex("dbo.persons", new[] { "DepId" });
            DropIndex("dbo.persons", new[] { "StatusId" });
            DropTable("dbo.status");
            DropTable("dbo.posts");
            DropTable("dbo.persons");
            DropTable("dbo.deps");
        }
    }
}
