namespace Services.Migrations.validations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class next : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ValidationHistories", "AsciiContent", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ValidationHistories", "AsciiContent");
        }
    }
}
