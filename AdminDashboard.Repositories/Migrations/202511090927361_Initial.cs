namespace AdminDashboard.Repositories.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 255),
                        LastName = c.String(nullable: false, maxLength: 255),
                        FullName = c.String(),
                        FirstNameAr = c.String(),
                        LastNameAr = c.String(),
                        FullNameAr = c.String(),
                        Username = c.String(nullable: false, maxLength: 255),
                        Email = c.String(nullable: false),
                        Phone = c.String(),
                        Password = c.String(nullable: false),
                        Type = c.Int(nullable: false),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        IsLocked = c.Boolean(nullable: false),
                        Status = c.Int(nullable: false),
                        ApprovedBy = c.Int(),
                        ApprovedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.Users", t => t.ApprovedBy)
                .ForeignKey("dbo.Users", t => t.CreatedBy)
                .Index(t => t.Username, unique: true)
                .Index(t => t.CreatedBy)
                .Index(t => t.ApprovedBy);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "CreatedBy", "dbo.Users");
            DropForeignKey("dbo.Users", "ApprovedBy", "dbo.Users");
            DropIndex("dbo.Users", new[] { "ApprovedBy" });
            DropIndex("dbo.Users", new[] { "CreatedBy" });
            DropIndex("dbo.Users", new[] { "Username" });
            DropTable("dbo.Users");
        }
    }
}
