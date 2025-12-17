using MediatR;
using BookingService.Data;
using BookingService.Data.Model;
using BookingService.Features.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Features.Resources
{
    public class AddOrUpdateResourceCommand
    {
        public class AddOrUpdateResourceRequest : IRequest<AddOrUpdateResourceResponse>
        {
            public ResourceApiModel Resource { get; set; }
            public Guid TenantUniqueId { get; set; }
        }

        public class AddOrUpdateResourceResponse { }

        public class AddOrUpdateResourceHandler : IAsyncRequestHandler<AddOrUpdateResourceRequest, AddOrUpdateResourceResponse>
        {
            public AddOrUpdateResourceHandler(BookingServiceContext context, ICache cache)
            {
                _context = context;
                _cache = cache;
            }

            public async Task<AddOrUpdateResourceResponse> Handle(AddOrUpdateResourceRequest request)
            {
                var entity = await _context.Resources
                    .Include(x => x.Tenant)
                    .SingleOrDefaultAsync(x => x.Id == request.Resource.Id && x.Tenant.UniqueId == request.TenantUniqueId);
                
                if (entity == null) {
                    var tenant = await _context.Tenants.SingleAsync(x => x.UniqueId == request.TenantUniqueId);
                    _context.Resources.Add(entity = new Resource() { TenantId = tenant.Id });
                }

                entity.Name = request.Resource.Name;
                
                await _context.SaveChangesAsync();

                return new AddOrUpdateResourceResponse();
            }

            private readonly BookingServiceContext _context;
            private readonly ICache _cache;
        }

    }

}
