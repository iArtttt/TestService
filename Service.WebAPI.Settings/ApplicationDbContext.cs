using Microsoft.EntityFrameworkCore;
using Service.WebAPI.Settings.Entities;

namespace Service.WebAPI.Settings
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<ApplicationLog> Logs { get; set; }

        public DbSet<ApplicationSetting> Settings { get; set; }

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationSetting>()
                .Property(x => x.Name).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<ApplicationSetting>()
                .Property(x => x.Value).IsRequired().HasDefaultValue(string.Empty);

            modelBuilder.Entity<ApplicationSetting>()
                .HasIndex(x => x.Name).IsUnique().IncludeProperties(p => p.Value);


            modelBuilder.Entity<ApplicationLog>()
                .Property(p => p.DateTime).HasColumnType("datetime2");


            modelBuilder.Entity<ApplicationSetting>().HasData(
                new ApplicationSetting { Id = 1, Name = "LogToDatabase", Value = "true" },
                new ApplicationSetting { Id = 2, Name = "LogOrdersChanges", Value = "false" },
                new ApplicationSetting { Id = 3, Name = "LogLevel", Value = "0" }
            );



            base.OnModelCreating(modelBuilder);
        }

    }
}