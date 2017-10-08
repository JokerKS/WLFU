namespace WLFU.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RequestTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProductCreationRequests",
                c => new
                    {
                        RequestId = c.Guid(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.RequestId)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductCreationRequests", "ProductId", "dbo.Products");
            DropIndex("dbo.ProductCreationRequests", new[] { "ProductId" });
            DropTable("dbo.ProductCreationRequests");
        }
    }
}
