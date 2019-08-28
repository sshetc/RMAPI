namespace RehabMakerAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCompanyMigration : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Params", "IdDevice", "dbo.Devices");
            DropForeignKey("dbo.Statistics", "IdDevice", "dbo.Devices");
            DropPrimaryKey("dbo.Devices");
            AlterColumn("dbo.Devices", "IdDevice", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Devices", "IdDevice");
            AddForeignKey("dbo.Params", "IdDevice", "dbo.Devices", "IdDevice", cascadeDelete: true);
            AddForeignKey("dbo.Statistics", "IdDevice", "dbo.Devices", "IdDevice", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Statistics", "IdDevice", "dbo.Devices");
            DropForeignKey("dbo.Params", "IdDevice", "dbo.Devices");
            DropPrimaryKey("dbo.Devices");
            AlterColumn("dbo.Devices", "IdDevice", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Devices", "IdDevice");
            AddForeignKey("dbo.Statistics", "IdDevice", "dbo.Devices", "IdDevice", cascadeDelete: true);
            AddForeignKey("dbo.Params", "IdDevice", "dbo.Devices", "IdDevice", cascadeDelete: true);
        }
    }
}
