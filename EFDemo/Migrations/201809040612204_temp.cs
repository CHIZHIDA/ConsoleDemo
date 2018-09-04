namespace EFDemo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class temp : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Blogs", newName: "Blog");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.Blog", newName: "Blogs");
        }
    }
}
