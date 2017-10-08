namespace WLFU.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMainImageToProd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "MainImageId", c => c.Int(nullable: false));
            CreateIndex("dbo.Products", "MainImageId");
            AddForeignKey("dbo.Products", "MainImageId", "dbo.Images", "ImageID", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "MainImageId", "dbo.Images");
            DropIndex("dbo.Products", new[] { "MainImageId" });
            DropColumn("dbo.Products", "MainImageId");
        }
    }
}
