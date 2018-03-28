namespace Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class G2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ShopRentOrTransfer", "Address", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ShopRentOrTransfer", "Address");
        }
    }
}
