namespace Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class G : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.ShopTransfer", newName: "ShopRentOrTransfer");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.ShopRentOrTransfer", newName: "ShopTransfer");
        }
    }
}
