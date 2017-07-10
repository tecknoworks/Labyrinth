namespace Labyrinth.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PlayerItem : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PlayerItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PlayerId = c.Int(nullable: false),
                        ItemId = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Items", t => t.ItemId, cascadeDelete: true)
                .ForeignKey("dbo.Players", t => t.PlayerId, cascadeDelete: true)
                .Index(t => t.PlayerId)
                .Index(t => t.ItemId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PlayerItems", "PlayerId", "dbo.Players");
            DropForeignKey("dbo.PlayerItems", "ItemId", "dbo.Items");
            DropIndex("dbo.PlayerItems", new[] { "ItemId" });
            DropIndex("dbo.PlayerItems", new[] { "PlayerId" });
            DropTable("dbo.PlayerItems");
        }
    }
}
