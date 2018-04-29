namespace JokerKS.WLFU.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Notifications_Changes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notifications", "Action", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Notifications", "MessageType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Notifications", "MessageType", c => c.String(maxLength: 100));
            DropColumn("dbo.Notifications", "Action");
        }
    }
}
