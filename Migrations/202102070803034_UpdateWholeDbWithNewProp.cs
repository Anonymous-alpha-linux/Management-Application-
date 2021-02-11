namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateWholeDbWithNewProp : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ChangeTraineeAccountAndInfors",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(),
                        Password = c.String(nullable: false, maxLength: 100),
                        ConfirmPassword = c.String(),
                        Trainee_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Trainee_Id)
                .Index(t => t.Trainee_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ChangeTraineeAccountAndInfors", "Trainee_Id", "dbo.AspNetUsers");
            DropIndex("dbo.ChangeTraineeAccountAndInfors", new[] { "Trainee_Id" });
            DropTable("dbo.ChangeTraineeAccountAndInfors");
        }
    }
}
