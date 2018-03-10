namespace JokerKS.WLFU.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateBasketProducts : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BasketProducts", "Amount", c => c.Int(nullable: false));
            AddColumn("dbo.BasketProducts", "DateCreated", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BasketProducts", "DateCreated");
            DropColumn("dbo.BasketProducts", "Amount");
        }
    }
}
