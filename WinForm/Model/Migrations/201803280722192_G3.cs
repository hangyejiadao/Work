namespace Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class G3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ShopRentOrTransfer", "TransFerMoney", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ShopRentOrTransfer", "TransFerMoney", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
