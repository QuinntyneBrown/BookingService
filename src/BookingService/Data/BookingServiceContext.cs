using BookingService.Data.Helpers;
using BookingService.Data.Model;
using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BookingService.Data
{
    public interface IBookingServiceContext
    {
        DbSet<User> Users { get; set; }
        DbSet<Role> Roles { get; set; }
        DbSet<Tenant> Tenants { get; set; }
        DbSet<DigitalAsset> DigitalAssets { get; set; }        
        DbSet<Account> Accounts { get; set; }
        DbSet<Profile> Profiles { get; set; }
        DbSet<Booking> Bookings { get; set; }
        DbSet<Resource> Resources { get; set; }
        Task<int> SaveChangesAsync();
    }

    public class BookingServiceContext: DbContext, IBookingServiceContext
    {
        public BookingServiceContext(DbContextOptions<BookingServiceContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<DigitalAsset> DigitalAssets { get; set; }        
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Resource> Resources { get; set; }

        public override int SaveChanges()
        {
            UpdateLoggableEntries();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync()
        {
            UpdateLoggableEntries();
            return base.SaveChangesAsync();
        }

        public void UpdateLoggableEntries()
        {
            foreach (var entity in ChangeTracker.Entries()
                .Where(e => e.Entity is ILoggable && ((e.State == EntityState.Added || (e.State == EntityState.Modified))))
                .Select(x => x.Entity as ILoggable))
            {
                entity.CreatedOn = entity.CreatedOn == default(DateTime) ? DateTime.UtcNow : entity.CreatedOn;
                entity.LastModifiedOn = DateTime.UtcNow;
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(u => u.Roles)
                .WithMany(r => r.Users)
                .UsingEntity(j => j.ToTable("UserRoles"));

            // Note: Soft delete convention needs to be reimplemented as a global query filter
            // Example: modelBuilder.Entity<YourEntity>().HasQueryFilter(e => !e.IsDeleted);
        }
    }
}