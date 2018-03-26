namespace JokerKS.WLFU.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BidAtAuctions : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BidAtAuctions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateCreated = c.DateTime(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UserId = c.String(maxLength: 128),
                        AuctionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Auctions", t => t.AuctionId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.AuctionId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BidAtAuctions", "UserId", "dbo.Users");
            DropForeignKey("dbo.BidAtAuctions", "AuctionId", "dbo.Auctions");
            DropIndex("dbo.BidAtAuctions", new[] { "AuctionId" });
            DropIndex("dbo.BidAtAuctions", new[] { "UserId" });
            DropTable("dbo.BidAtAuctions");
        }
    }
}
