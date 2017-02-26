namespace Services.Migrations.validations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ValidationHistories",
                c => new
                    {
                        ValidationHistoryID = c.Guid(nullable: false),
                        Content = c.String(),
                        Status = c.Int(nullable: false),
                        Description = c.String(),
                        FileName = c.String(),
                        EmailContentId = c.Guid(),
                        ValidationDTG = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ValidationHistoryID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ValidationHistories");
        }
    }
}
