namespace Labyrinth.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Sell : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Sells",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PlayerId = c.Int(nullable: false),
                        ItemId = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Price = c.Int(nullable: false),
                        IsSold = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Items", t => t.ItemId, cascadeDelete: true)
                .ForeignKey("dbo.Players", t => t.PlayerId, cascadeDelete: true)
                .Index(t => t.PlayerId)
                .Index(t => t.ItemId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Sells", "PlayerId", "dbo.Players");
            DropForeignKey("dbo.Sells", "ItemId", "dbo.Items");
            DropIndex("dbo.Sells", new[] { "ItemId" });
            DropIndex("dbo.Sells", new[] { "PlayerId" });
            DropTable("dbo.Sells");
        }
    }
}
