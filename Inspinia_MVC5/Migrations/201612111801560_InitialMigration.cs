namespace Inspinia_MVC5.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
              "dbo.Tournaments",
              c => new
              {
                  Id = c.Int(nullable: false, identity: true),
                  Name = c.String(),
                  DateFrom = c.DateTime(nullable: false),
                  DateTo = c.DateTime(nullable: false),
                  RoundOfSixteen_Id = c.Int(),
                  QuarterFinal_Id = c.Int(),
                  SemiFinal_Id = c.Int(),
                  Final_Id = c.Int(),
              })
              .PrimaryKey(t => t.Id)
              
              .Index(t => t.RoundOfSixteen_Id)
              .Index(t => t.QuarterFinal_Id)
              .Index(t => t.SemiFinal_Id)
              .Index(t => t.Final_Id);
        }
        
        public override void Down()
        {
        }
    }
}
