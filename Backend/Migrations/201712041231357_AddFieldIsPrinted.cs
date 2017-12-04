namespace Backend.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFieldIsPrinted : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EagleBatches", "IsPrinted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.EagleBatches", "IsPrinted");
        }
    }
}
