namespace Services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class next1 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.EmailContents", "IsChecking");
        }
        
        public override void Down()
        {
            AddColumn("dbo.EmailContents", "IsChecking", c => c.Boolean(nullable: false));
        }
    }
}
