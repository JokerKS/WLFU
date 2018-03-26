namespace JokerKS.WLFU.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BidAtAuctions_isWinner : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Auctions", "IsClosed", c => c.Boolean(nullable: false));
            AddColumn("dbo.BidAtAuctions", "IsWinner", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Auctions", "InstantSellingPrice", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Auctions", "InstantSellingPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.BidAtAuctions", "IsWinner");
            DropColumn("dbo.Auctions", "IsClosed");
        }
    }
}
