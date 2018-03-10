namespace JokerKS.WLFU.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BasketTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BasketProducts",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        ProductId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.ProductId })
                .ForeignKey("dbo.Users", t => t.UserId)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.ProductId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BasketProducts", "ProductId", "dbo.Products");
            DropForeignKey("dbo.BasketProducts", "UserId", "dbo.Users");
            DropIndex("dbo.BasketProducts", new[] { "ProductId" });
            DropIndex("dbo.BasketProducts", new[] { "UserId" });
            DropTable("dbo.BasketProducts");
        }
    }
}
