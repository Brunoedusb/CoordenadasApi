namespace API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class address : DbMigration
    {
        public override void Up()
        {
            CreateTable(
            "dbo.Address",
            c => new
            {
                AddressId = c.Int(nullable: false, identity: true),
                UserId = c.String(nullable: false, maxLength: 128),
                Lat = c.String(),
                Lng = c.String(),
                FormatedAddress = c.String(),
            })
            .PrimaryKey(t => t.AddressId)
            .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Address", "UserId", "dbo.AspNetUsers");
            DropTable("dbo.Address");
        }
    }
}
