namespace JokerKS.WLFU.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitDb_1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Products", "MainImageId", "dbo.Images");
            DropIndex("dbo.Products", new[] { "MainImageId" });
            AlterColumn("dbo.Products", "MainImageId", c => c.Int());
            CreateIndex("dbo.Products", "MainImageId");
            AddForeignKey("dbo.Products", "MainImageId", "dbo.Images", "ImageID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "MainImageId", "dbo.Images");
            DropIndex("dbo.Products", new[] { "MainImageId" });
            AlterColumn("dbo.Products", "MainImageId", c => c.Int(nullable: false));
            CreateIndex("dbo.Products", "MainImageId");
            AddForeignKey("dbo.Products", "MainImageId", "dbo.Images", "ImageID", cascadeDelete: true);
        }
    }
}
