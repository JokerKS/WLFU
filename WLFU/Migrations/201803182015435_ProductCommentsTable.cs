namespace JokerKS.WLFU.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductCommentsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProductComments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateCreated = c.DateTime(nullable: false),
                        Text = c.String(),
                        UserId = c.String(maxLength: 128),
                        ProductId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.ProductId);
            
            AddColumn("dbo.Products", "IsPublished", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductComments", "UserId", "dbo.Users");
            DropForeignKey("dbo.ProductComments", "ProductId", "dbo.Products");
            DropIndex("dbo.ProductComments", new[] { "ProductId" });
            DropIndex("dbo.ProductComments", new[] { "UserId" });
            DropColumn("dbo.Products", "IsPublished");
            DropTable("dbo.ProductComments");
        }
    }
}
