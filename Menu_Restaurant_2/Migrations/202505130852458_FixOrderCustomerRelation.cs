namespace Menu_Restaurant_2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixOrderCustomerRelation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Food",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Portion_Size = c.String(nullable: false, maxLength: 50),
                        Category = c.String(nullable: false, maxLength: 50),
                        Stock = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Info = c.String(maxLength: 255),
                        Image = c.String(maxLength: 255),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.FoodMenu",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FoodId = c.Int(nullable: false),
                        MenuId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Food", t => t.FoodId, cascadeDelete: true)
                .ForeignKey("dbo.Menu", t => t.MenuId, cascadeDelete: true)
                .Index(t => t.FoodId)
                .Index(t => t.MenuId);
            
            CreateTable(
                "dbo.Menu",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Portion_Size = c.String(nullable: false, maxLength: 50),
                        Category = c.String(nullable: false, maxLength: 50),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Stock = c.Int(nullable: false),
                        Image = c.String(maxLength: 255),
                        Info = c.String(maxLength: 1000),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerId = c.Int(nullable: false),
                        OrderNumber = c.String(nullable: false, maxLength: 50),
                        Date = c.DateTime(nullable: false),
                        Status = c.String(nullable: false, maxLength: 50),
                        FoodPrice = c.Decimal(nullable: false, precision: 10, scale: 2),
                        TransportPrice = c.Decimal(nullable: false, precision: 10, scale: 2),
                        TotalPrice = c.Decimal(nullable: false, precision: 10, scale: 2),
                        EstimatedTime = c.DateTime(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                        Phone = c.String(maxLength: 20),
                        Address = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Person", t => t.CustomerId, cascadeDelete: true)
                .Index(t => t.CustomerId);
            
            CreateTable(
                "dbo.Person",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Type = c.String(nullable: false, maxLength: 50),
                        Email = c.String(nullable: false, maxLength: 100),
                        Parola = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "CustomerId", "dbo.Person");
            DropForeignKey("dbo.FoodMenu", "MenuId", "dbo.Menu");
            DropForeignKey("dbo.FoodMenu", "FoodId", "dbo.Food");
            DropIndex("dbo.Orders", new[] { "CustomerId" });
            DropIndex("dbo.FoodMenu", new[] { "MenuId" });
            DropIndex("dbo.FoodMenu", new[] { "FoodId" });
            DropTable("dbo.Person");
            DropTable("dbo.Orders");
            DropTable("dbo.Menu");
            DropTable("dbo.FoodMenu");
            DropTable("dbo.Food");
        }
    }
}
