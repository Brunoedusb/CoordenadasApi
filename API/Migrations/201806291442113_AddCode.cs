namespace API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCode : DbMigration
    {
        public override void Up()
        {
            CreateTable(
            "dbo.ValidCode",
            c => new
            {
                Id = c.Int(nullable: false, identity: true),
                UserId = c.String(nullable: false, maxLength: 128),
                Code = c.String(),
            })
            .PrimaryKey(t => t.Id)
            .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true);
        }

        public override void Down()
        {
            DropForeignKey("dbo.ValidCode", "UserId", "dbo.AspNetUsers");
            DropTable("dbo.ValidCode");
        }
    }
}
