namespace AdminDashboard.Repositories.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_failedAttempts_Field : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "FailedAttempts", c => c.Int(nullable: false, defaultValue: 0));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "FailedAttempts");
        }
    }
}
