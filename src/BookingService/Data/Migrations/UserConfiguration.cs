using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Linq;
using BookingService.Data;
using BookingService.Data.Model;
using BookingService.Security;

namespace BookingService.Migrations
{
    public class UserConfiguration
    {
        public static void Seed(BookingServiceContext context) {

            var systemRole = context.Roles.First(x => x.Name == Roles.SYSTEM);
            var roles = new List<Role>();
            var tenant = context.Tenants.Single(x => x.Name == "Default");

            roles.Add(systemRole);

            context.Users.AddOrUpdate(x => x.Username, new User()
            {
                Username = "system",
                Password = new EncryptionService().TransformPassword("system"),
                Roles = roles,
                TenantId = tenant.Id
            });
                        
            context.SaveChanges();
        }
    }
}
