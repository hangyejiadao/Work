namespace Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class G10 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ErrorUrl", "UrlType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ErrorUrl", "UrlType");
        }
    }
}
