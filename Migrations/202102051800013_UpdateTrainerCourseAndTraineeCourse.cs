namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTrainerCourseAndTraineeCourse : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.TrainerCourses", name: "TraineeId", newName: "TrainerId");
            RenameIndex(table: "dbo.TrainerCourses", name: "IX_TraineeId", newName: "IX_TrainerId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.TrainerCourses", name: "IX_TrainerId", newName: "IX_TraineeId");
            RenameColumn(table: "dbo.TrainerCourses", name: "TrainerId", newName: "TraineeId");
        }
    }
}
