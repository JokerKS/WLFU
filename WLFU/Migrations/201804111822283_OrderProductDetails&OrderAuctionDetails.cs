namespace JokerKS.WLFU.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrderProductDetailsOrderAuctionDetails : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.OrderDetails", newName: "ProductOrderDetails");
            CreateTable(
                "dbo.AuctionOrderDetails",
                c => new
                    {
                        OrderId = c.Int(nullable: false),
                        AuctionId = c.Int(nullable: false),
                        Amount = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => new { t.OrderId, t.AuctionId })
                .ForeignKey("dbo.Auctions", t => t.AuctionId, cascadeDelete: true)
                .ForeignKey("dbo.Orders", t => t.OrderId, cascadeDelete: true)
                .Index(t => t.OrderId)
                .Index(t => t.AuctionId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AuctionOrderDetails", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.AuctionOrderDetails", "AuctionId", "dbo.Auctions");
            DropIndex("dbo.AuctionOrderDetails", new[] { "AuctionId" });
            DropIndex("dbo.AuctionOrderDetails", new[] { "OrderId" });
            DropTable("dbo.AuctionOrderDetails");
            RenameTable(name: "dbo.ProductOrderDetails", newName: "OrderDetails");
        }
    }
}
