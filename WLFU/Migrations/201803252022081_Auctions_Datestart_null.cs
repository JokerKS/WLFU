namespace JokerKS.WLFU.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Auctions_Datestart_null : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Auctions", "DateStart", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Auctions", "DateStart", c => c.DateTime(nullable: false));
        }
    }
}
