namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatefieldSecondNametoLastNametableUserProfiles : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserProfiles", "LastName", c => c.String(maxLength: 256));
            DropColumn("dbo.UserProfiles", "SecondName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserProfiles", "SecondName", c => c.String(maxLength: 256));
            DropColumn("dbo.UserProfiles", "LastName");
        }
    }
}
