namespace Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Areas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Url = c.String(),
                        ParentId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Images",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Guid = c.Guid(nullable: false),
                        InfoType = c.Int(nullable: false),
                        ImageUrl = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ShopTransfers",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        InfoType = c.Int(nullable: false),
                        InfoTitle = c.String(),
                        InfoContent = c.String(),
                        DetailAddress = c.String(),
                        ShopArea = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TransFerMoney = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Customer = c.String(),
                        Phone = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ShopTransfers");
            DropTable("dbo.Images");
            DropTable("dbo.Areas");
        }
    }
}
