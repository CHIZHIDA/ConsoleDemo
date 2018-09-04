namespace EFDemo.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<EFDemo.EFCodeFirstDbContext>
    {
        public Configuration()
        {
            //将AutomaticMigrationEnable设置为true，表示启用自动迁移技术
            AutomaticMigrationsEnabled = true;//false;
            //将AutomaticMigrationDataLossAllowed设置为true，表示在更新数据表结构是，允许丢失数据。
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(EFDemo.EFCodeFirstDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}
