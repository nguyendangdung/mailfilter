namespace Services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class next : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EmailContents",
                c => new
                    {
                        EmailContentID = c.Guid(nullable: false),
                        Content = c.String(),
                        Status = c.Int(nullable: false),
                        IsChecking = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.EmailContentID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.EmailContents");
        }
    }
}
