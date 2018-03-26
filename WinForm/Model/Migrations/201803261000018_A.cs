namespace Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class A : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Areas", newName: "Area");
            RenameTable(name: "dbo.Images", newName: "Image");
            RenameTable(name: "dbo.ShopTransfers", newName: "ShopTransfer");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.ShopTransfer", newName: "ShopTransfers");
            RenameTable(name: "dbo.Image", newName: "Images");
            RenameTable(name: "dbo.Area", newName: "Areas");
        }
    }
}
