namespace JokerKS.WLFU.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Product_IsActive : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "IsActive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "IsActive");
        }
    }
}
