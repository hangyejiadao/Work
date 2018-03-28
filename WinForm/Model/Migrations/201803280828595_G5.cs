namespace Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class G5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ShopRentOrTransfer", "AreaId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ShopRentOrTransfer", "AreaId");
        }
    }
}
