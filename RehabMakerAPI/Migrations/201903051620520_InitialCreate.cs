namespace RehabMakerAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Devices",
                c => new
                    {
                        IdDevice = c.Int(nullable: false, identity: true),
                        Number = c.String(),
                    })
                .PrimaryKey(t => t.IdDevice);
            
            CreateTable(
                "dbo.Params",
                c => new
                    {
                        IdParams = c.Int(nullable: false, identity: true),
                        Speed = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Distance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Сalories = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Date = c.DateTime(nullable: false),
                        IdDevice = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdParams)
                .ForeignKey("dbo.Devices", t => t.IdDevice, cascadeDelete: true)
                .Index(t => t.IdDevice);
            
            CreateTable(
                "dbo.Statistics",
                c => new
                    {
                        IdStatitics = c.Int(nullable: false, identity: true),
                        AverageSpeed = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalDistance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalCalories = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Date = c.DateTime(nullable: false),
                        IdDevice = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdStatitics)
                .ForeignKey("dbo.Devices", t => t.IdDevice, cascadeDelete: true)
                .Index(t => t.IdDevice);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Statistics", "IdDevice", "dbo.Devices");
            DropForeignKey("dbo.Params", "IdDevice", "dbo.Devices");
            DropIndex("dbo.Statistics", new[] { "IdDevice" });
            DropIndex("dbo.Params", new[] { "IdDevice" });
            DropTable("dbo.Statistics");
            DropTable("dbo.Params");
            DropTable("dbo.Devices");
        }
    }
}
