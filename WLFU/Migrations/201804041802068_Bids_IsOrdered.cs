namespace JokerKS.WLFU.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Bids_IsOrdered : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BidAtAuctions", "IsOrdered", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BidAtAuctions", "IsOrdered");
        }
    }
}
