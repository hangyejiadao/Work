namespace Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class G4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Image", "FkId", c => c.Guid(nullable: false));
            AddColumn("dbo.ShopRentOrTransfer", "IndustryName", c => c.String());
            DropColumn("dbo.Image", "Guid");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Image", "Guid", c => c.Guid(nullable: false));
            DropColumn("dbo.ShopRentOrTransfer", "IndustryName");
            DropColumn("dbo.Image", "FkId");
        }
    }
}
