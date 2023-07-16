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
                .Entity<TodoItem>() // це ми говоримо шо ми будемо працювати з тудуайтем
                .Property(ti => ti.Id) // це ми говоримо шо ми будемо налаштовувати проперті айді з ентітібейз
                .UseIdentityColumn(); // метод який говорить шо айді буде автоматично інкрементуватись
            modelBuilder
               .Entity<TodoItem>()
               .Property(ti => ti.CreatedAt)
               .HasDefaultValueSql("GETDATE()"); // виклик методу який автоматично записує дату на створенні тудуайтему
            modelBuilder
               .Entity<TodoItem>()
               .Property(ti => ti.IsDone)
               .HasDefaultValue(false); // виклик методу який сетить дефолтне значення на фолс
            modelBuilder
               .Entity<Day>()
               .Property(ti => ti.Id)
               .UseIdentityColumn();
           modelBuilder
               .Entity<TodoItem>()
               .HasOne(s => s.Day) // звʼязує DayId з полем primary Id в Day
               .WithMany(s => s.Items) //
               .OnDelete(DeleteBehavior.Cascade) // цей метод викликається на момент видалення тудуайтему. Коли видаляється тудуайтем , разом з ним видаляється дей. 
               .HasForeignKey(t => t.DayId);
           modelBuilder
               .Entity<User>()
               .Metadata.SetIsTableExcludedFromMigrations(true);
           modelBuilder
               .Entity<TodoItem>()
               .HasOne(s => s.User); //adds foreignkey
            base.OnModelCreating(modelBuilder);
        }
    }
}
