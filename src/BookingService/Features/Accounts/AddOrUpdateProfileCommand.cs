using MediatR;
using BookingService.Data;
using BookingService.Data.Model;
using BookingService.Features.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Data.Entity;

namespace BookingService.Features.Accounts
{
    public class AddOrUpdateProfileCommand
    {
        public class AddOrUpdateProfileRequest : IRequest<AddOrUpdateProfileResponse>
        {
            public ProfileApiModel Profile { get; set; }
            public Guid TenantUniqueId { get; set; }
        }

        public class AddOrUpdateProfileResponse { }

        public class AddOrUpdateProfileHandler : IAsyncRequestHandler<AddOrUpdateProfileRequest, AddOrUpdateProfileResponse>
        {
            public AddOrUpdateProfileHandler(BookingServiceContext context, ICache cache)
            {
                _context = context;
                _cache = cache;
            }

            public async Task<AddOrUpdateProfileResponse> Handle(AddOrUpdateProfileRequest request)
            {
                var entity = await _context.Profiles
                    .Include(x => x.Tenant)
                    .SingleOrDefaultAsync(x => x.Id == request.Profile.Id && x.Tenant.UniqueId == request.TenantUniqueId);
                
                if (entity == null) {
                    var tenant = await _context.Tenants.SingleAsync(x => x.UniqueId == request.TenantUniqueId);
                    _context.Profiles.Add(entity = new Profile() { TenantId = tenant.Id });
                }

                entity.Name = request.Profile.Name;
                
                await _context.SaveChangesAsync();

                return new AddOrUpdateProfileResponse();
            }

            private readonly BookingServiceContext _context;
            private readonly ICache _cache;
        }

    }

}
