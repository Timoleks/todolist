using Microsoft.EntityFrameworkCore;
using TodolistApi.Domain.Models;

namespace TodolistApi.Domain.Data
{
    public sealed class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}

        public DbSet<TodoItem> TodoItems => Set<TodoItem>();

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

            base.OnModelCreating(modelBuilder);
        }
    }
}
