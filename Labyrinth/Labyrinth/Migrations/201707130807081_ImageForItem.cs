namespace Labyrinth.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ImageForItem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Items", "Image", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Items", "Image");
        }
    }
}
