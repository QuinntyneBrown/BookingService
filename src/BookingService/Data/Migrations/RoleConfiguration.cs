using System.Data.Entity.Migrations;
using BookingService.Data;
using BookingService.Data.Model;
using BookingService.Features.Users;

namespace BookingService.Migrations
{
    public class RoleConfiguration
    {
        public static void Seed(BookingServiceContext context) {

            context.Roles.AddOrUpdate(x => x.Name, new Role()
            {
                Name = Roles.SYSTEM
            });

            context.Roles.AddOrUpdate(x => x.Name, new Role()
            {
                Name = Roles.ACCOUNT_HOLDER
            });

            context.Roles.AddOrUpdate(x => x.Name, new Role()
            {
                Name = Roles.DEVELOPMENT
            });

            context.SaveChanges();
        }
    }
}
