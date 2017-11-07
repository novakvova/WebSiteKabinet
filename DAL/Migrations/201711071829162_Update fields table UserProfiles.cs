namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatefieldstableUserProfiles : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.UserProfiles", "FirstName", c => c.String(maxLength: 256));
            AlterColumn("dbo.UserProfiles", "SecondName", c => c.String(maxLength: 256));
            AlterColumn("dbo.UserProfiles", "Image", c => c.String(maxLength: 256));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UserProfiles", "Image", c => c.String());
            AlterColumn("dbo.UserProfiles", "SecondName", c => c.String());
            AlterColumn("dbo.UserProfiles", "FirstName", c => c.String());
        }
    }
}
