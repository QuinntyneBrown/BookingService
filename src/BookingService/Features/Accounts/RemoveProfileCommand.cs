using System.Threading;
using MediatR;
using BookingService.Data;
using BookingService.Data.Model;
using BookingService.Features.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Features.Accounts
{
    public class RemoveProfileCommand
    {
        public class RemoveProfileRequest : IRequest<RemoveProfileResponse>
        {
            public int Id { get; set; }
            public Guid TenantUniqueId { get; set; } 
        }

        public class RemoveProfileResponse { }

        public class RemoveProfileHandler : IRequestHandler<RemoveProfileRequest, RemoveProfileResponse>
        {
            public RemoveProfileHandler(BookingServiceContext context, ICache cache)
            {
                _context = context;
                _cache = cache;
            }

            public async Task<RemoveProfileResponse> Handle(RemoveProfileRequest request, CancellationToken cancellationToken)
            {
                var profile = await _context.Profiles.SingleAsync(x=>x.Id == request.Id && x.Tenant.UniqueId == request.TenantUniqueId);
                profile.IsDeleted = true;
                await _context.SaveChangesAsync();
                return new RemoveProfileResponse();
            }

            private readonly BookingServiceContext _context;
            private readonly ICache _cache;
        }
    }
}
