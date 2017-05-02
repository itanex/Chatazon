using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Chatazon.Models;

using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Chatazon.Data
{

    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            // http://stackoverflow.com/questions/35907509/choose-a-specific-entity-framework-7-core-1-migration-in-code
            Microsoft.EntityFrameworkCore.Migrations.IMigrator mig =
                (Microsoft.EntityFrameworkCore.Migrations.IMigrator)
                context.GetInfrastructure().GetService(
                typeof(Microsoft.EntityFrameworkCore.Migrations.IMigrator)
            );

            // http://stackoverflow.com/questions/38408213/entity-framework-core-1-0-code-first-migrations-using-code

            // mig.Migrate(nameof(Chatazon.Migrations.Initial));

            // var migrator = db.GetInfrastructure().GetRequiredService<IMigrator>();
            // migrator.Migrate("NameOfMyMigration");

            // Microsoft.EntityFrameworkCore.Migrations.IHistoryRepository hist;
            // hist.GetAppliedMigrations


            // http://www.dotnetjalps.com/2016/06/entity-framework-core-migration.html
            // http://www.learnentityframeworkcore.com/migrations
            // http://stackoverflow.com/questions/35144784/how-to-use-migration-programmatically-in-entityframework-codefirst
            // https://romiller.com/2012/02/09/running-scripting-migrations-from-code/
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public DbSet<Message> Message { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            builder.Entity<Message>()
                .ToTable("Message")
                .Property(m => m.DateCreated)
                .HasDefaultValueSql("strftime('%Y-%m-%d %H:%M:%S')")
                ;
        }
    }
}
