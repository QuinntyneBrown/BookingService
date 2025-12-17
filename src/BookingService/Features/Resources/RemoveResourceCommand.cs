using MediatR;
using BookingService.Data;
using BookingService.Data.Model;
using BookingService.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Data.Entity;

namespace BookingService.Features.Resources
{
    public class RemoveResourceCommand
    {
        public class RemoveResourceRequest : IRequest<RemoveResourceResponse>
        {
            public int Id { get; set; }
            public Guid TenantUniqueId { get; set; } 
        }

        public class RemoveResourceResponse { }

        public class RemoveResourceHandler : IAsyncRequestHandler<RemoveResourceRequest, RemoveResourceResponse>
        {
            public RemoveResourceHandler(BookingServiceContext context, ICache cache)
            {
                _context = context;
                _cache = cache;
            }

            public async Task<RemoveResourceResponse> Handle(RemoveResourceRequest request)
            {
                var resource = await _context.Resources.SingleAsync(x=>x.Id == request.Id && x.Tenant.UniqueId == request.TenantUniqueId);
                resource.IsDeleted = true;
                await _context.SaveChangesAsync();
                return new RemoveResourceResponse();
            }

            private readonly BookingServiceContext _context;
            private readonly ICache _cache;
        }
    }
}
