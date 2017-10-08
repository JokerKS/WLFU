namespace WLFU.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRequestsTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ProductCreationRequests", "ProductId", "dbo.Products");
            DropIndex("dbo.ProductCreationRequests", new[] { "ProductId" });
            AlterColumn("dbo.ProductCreationRequests", "ProductId", c => c.Int());
            CreateIndex("dbo.ProductCreationRequests", "ProductId");
            AddForeignKey("dbo.ProductCreationRequests", "ProductId", "dbo.Products", "ProductId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductCreationRequests", "ProductId", "dbo.Products");
            DropIndex("dbo.ProductCreationRequests", new[] { "ProductId" });
            AlterColumn("dbo.ProductCreationRequests", "ProductId", c => c.Int(nullable: false));
            CreateIndex("dbo.ProductCreationRequests", "ProductId");
            AddForeignKey("dbo.ProductCreationRequests", "ProductId", "dbo.Products", "ProductId", cascadeDelete: true);
        }
    }
}
