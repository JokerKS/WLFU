namespace JokerKS.WLFU.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Auctions : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AuctionComments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateCreated = c.DateTime(nullable: false),
                        Text = c.String(),
                        UserId = c.String(maxLength: 128),
                        AuctionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Auctions", t => t.AuctionId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.AuctionId);
            
            CreateTable(
                "dbo.Auctions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        IsPublished = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        StartPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PriceIncrease = c.Decimal(nullable: false, precision: 18, scale: 2),
                        InstantSellingPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DateStart = c.DateTime(nullable: false),
                        DateFinish = c.DateTime(nullable: false),
                        DesignerId = c.String(nullable: false, maxLength: 128),
                        MainImageId = c.Int(),
                        CategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProductCategories", t => t.CategoryId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.DesignerId, cascadeDelete: true)
                .ForeignKey("dbo.Images", t => t.MainImageId)
                .Index(t => t.DesignerId)
                .Index(t => t.MainImageId)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.AuctionImages",
                c => new
                    {
                        AuctionId = c.Int(nullable: false),
                        ImageId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.AuctionId, t.ImageId })
                .ForeignKey("dbo.Images", t => t.ImageId, cascadeDelete: true)
                .ForeignKey("dbo.Auctions", t => t.AuctionId)
                .Index(t => t.AuctionId)
                .Index(t => t.ImageId);
            
            CreateTable(
                "dbo.AuctionTags",
                c => new
                    {
                        AuctionId = c.Int(nullable: false),
                        TagId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.AuctionId, t.TagId })
                .ForeignKey("dbo.Auctions", t => t.AuctionId, cascadeDelete: true)
                .ForeignKey("dbo.Tags", t => t.TagId, cascadeDelete: true)
                .Index(t => t.AuctionId)
                .Index(t => t.TagId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AuctionComments", "UserId", "dbo.Users");
            DropForeignKey("dbo.AuctionTags", "TagId", "dbo.Tags");
            DropForeignKey("dbo.AuctionTags", "AuctionId", "dbo.Auctions");
            DropForeignKey("dbo.Auctions", "MainImageId", "dbo.Images");
            DropForeignKey("dbo.AuctionImages", "AuctionId", "dbo.Auctions");
            DropForeignKey("dbo.AuctionImages", "ImageId", "dbo.Images");
            DropForeignKey("dbo.Auctions", "DesignerId", "dbo.Users");
            DropForeignKey("dbo.AuctionComments", "AuctionId", "dbo.Auctions");
            DropForeignKey("dbo.Auctions", "CategoryId", "dbo.ProductCategories");
            DropIndex("dbo.AuctionTags", new[] { "TagId" });
            DropIndex("dbo.AuctionTags", new[] { "AuctionId" });
            DropIndex("dbo.AuctionImages", new[] { "ImageId" });
            DropIndex("dbo.AuctionImages", new[] { "AuctionId" });
            DropIndex("dbo.Auctions", new[] { "CategoryId" });
            DropIndex("dbo.Auctions", new[] { "MainImageId" });
            DropIndex("dbo.Auctions", new[] { "DesignerId" });
            DropIndex("dbo.AuctionComments", new[] { "AuctionId" });
            DropIndex("dbo.AuctionComments", new[] { "UserId" });
            DropTable("dbo.AuctionTags");
            DropTable("dbo.AuctionImages");
            DropTable("dbo.Auctions");
            DropTable("dbo.AuctionComments");
        }
    }
}
