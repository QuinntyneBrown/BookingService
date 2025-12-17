using System.Threading;
using MediatR;
using BookingService.Data;
using BookingService.Features.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Features.Resources
{
    public class GetResourcesQuery
    {
        public class GetResourcesRequest : IRequest<GetResourcesResponse> { 
            public Guid TenantUniqueId { get; set; }       
        }

        public class GetResourcesResponse
        {
            public ICollection<ResourceApiModel> Resources { get; set; } = new HashSet<ResourceApiModel>();
        }

        public class GetResourcesHandler : IRequestHandler<GetResourcesRequest, GetResourcesResponse>
        {
            public GetResourcesHandler(BookingServiceContext context, ICache cache)
            {
                _context = context;
                _cache = cache;
            }

            public async Task<GetResourcesResponse> Handle(GetResourcesRequest request, CancellationToken cancellationToken)
            {
                var resources = await _context.Resources
                    .Include(x => x.Tenant)
                    .Where(x => x.Tenant.UniqueId == request.TenantUniqueId )
                    .ToListAsync();

                return new GetResourcesResponse()
                {
                    Resources = resources.Select(x => ResourceApiModel.FromResource(x)).ToList()
                };
            }

            private readonly BookingServiceContext _context;
            private readonly ICache _cache;
        }

    }

}
