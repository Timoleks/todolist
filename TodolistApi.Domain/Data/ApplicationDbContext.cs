using Microsoft.EntityFrameworkCore;
using TodolistApi.Domain.Models;

namespace TodolistApi.Domain.Data
{
    public sealed class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options) {}

        public DbSet<TodoItem> TodoItems => Set<TodoItem>();
        public DbSet<Day> Days => Set<Day>();
        public DbSet<User> Users => Set<User>(); 



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<TodoItem>()
                .Property(ti => ti.Id)
                .UseIdentityColumn();
            modelBuilder
               .Entity<TodoItem>()
               .Property(ti => ti.CreatedAt)
               .HasDefaultValueSql("GETDATE()");
            modelBuilder
               .Entity<TodoItem>()
               .Property(ti => ti.IsDone)
               .HasDefaultValue(false);
            modelBuilder
               .Entity<Day>()
               .Property(ti => ti.Id)
               .UseIdentityColumn();
           modelBuilder
               .Entity<TodoItem>()
               .HasOne(s => s.Day)
               .WithMany(s => s.Items)
               .OnDelete(DeleteBehavior.Cascade)
               .HasForeignKey(t => t.DayId);
           modelBuilder
               .Entity<User>()
               .Metadata.SetIsTableExcludedFromMigrations(true);
           modelBuilder
               .Entity<TodoItem>()
               .HasOne(s => s.User);
            base.OnModelCreating(modelBuilder);
        }
    }
}
