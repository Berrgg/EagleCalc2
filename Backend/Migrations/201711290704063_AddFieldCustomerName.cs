namespace Backend.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFieldCustomerName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "CustomerName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "CustomerName");
        }
    }
}
