namespace JokerKS.WLFU.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tags_Name_isUnique : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Tags", "Name", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Tags", new[] { "Name" });
        }
    }
}
