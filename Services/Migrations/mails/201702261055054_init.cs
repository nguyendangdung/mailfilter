namespace Services.Migrations.mails
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
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
                    })
                .PrimaryKey(t => t.EmailContentID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.EmailContents");
        }
    }
}
