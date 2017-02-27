namespace Services.Migrations.mails
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class next : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EmailContents", "AsciiContent", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.EmailContents", "AsciiContent");
        }
    }
}
