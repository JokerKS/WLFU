namespace JokerKS.WLFU.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductRatings : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProductRatings",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        ProductId = c.Int(nullable: false),
                        Rating = c.Single(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.ProductId })
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductId)
                .Index(t => t.UserId)
                .Index(t => t.ProductId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductRatings", "ProductId", "dbo.Products");
            DropForeignKey("dbo.ProductRatings", "UserId", "dbo.Users");
            DropIndex("dbo.ProductRatings", new[] { "ProductId" });
            DropIndex("dbo.ProductRatings", new[] { "UserId" });
            DropTable("dbo.ProductRatings");
        }
    }
}
