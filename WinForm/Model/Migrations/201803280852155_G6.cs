namespace Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class G6 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ShopBegRent",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        InfoTitle = c.String(),
                        InfoContent = c.String(),
                        Customer = c.String(),
                        Phone = c.String(),
                        MinArea = c.Double(nullable: false),
                        MaxArea = c.Double(nullable: false),
                        MinRentMoney = c.Double(nullable: false),
                        MaxRentMoney = c.Double(nullable: false),
                        AreaId = c.String(),
                        UpdateTime = c.DateTime(nullable: false),
                        AreaName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.ShopRentOrTransfer", "UpdateTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ShopRentOrTransfer", "UpdateTime");
            DropTable("dbo.ShopBegRent");
        }
    }
}
