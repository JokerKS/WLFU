namespace WLFU.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        ProductId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 16, scale: 2),
                        DesignerId = c.String(nullable: false, maxLength: 128),
                        Description = c.String(nullable: false),
                        Amount = c.Short(nullable: false),
                    })
                .PrimaryKey(t => t.ProductId)
                .ForeignKey("dbo.AspNetUsers", t => t.DesignerId, cascadeDelete: true)
                .Index(t => t.DesignerId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "DesignerId", "dbo.AspNetUsers");
            DropIndex("dbo.Products", new[] { "DesignerId" });
            DropTable("dbo.Products");
        }
    }
}
