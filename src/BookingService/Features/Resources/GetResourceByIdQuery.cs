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
    public class GetResourceByIdQuery
    {
        public class GetResourceByIdRequest : IRequest<GetResourceByIdResponse> { 
            public int Id { get; set; }
            public Guid TenantUniqueId { get; set; }
        }

        public class GetResourceByIdResponse
        {
            public ResourceApiModel Resource { get; set; } 
        }

        public class GetResourceByIdHandler : IRequestHandler<GetResourceByIdRequest, GetResourceByIdResponse>
        {
            public GetResourceByIdHandler(BookingServiceContext context, ICache cache)
            {
                _context = context;
                _cache = cache;
            }

            public async Task<GetResourceByIdResponse> Handle(GetResourceByIdRequest request, CancellationToken cancellationToken)
            {                
                return new GetResourceByIdResponse()
                {
                    Resource = ResourceApiModel.FromResource(await _context.Resources
                    .Include(x => x.Tenant)				
					.SingleAsync(x=>x.Id == request.Id &&  x.Tenant.UniqueId == request.TenantUniqueId))
                };
            }

            private readonly BookingServiceContext _context;
            private readonly ICache _cache;
        }

    }

}
