namespace Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class G9 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ErrorUrl",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Url = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ErrorUrl");
        }
    }
}
