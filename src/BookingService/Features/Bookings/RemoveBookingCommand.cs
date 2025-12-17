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

namespace BookingService.Features.Bookings
{
    public class RemoveBookingCommand
    {
        public class RemoveBookingRequest : IRequest<RemoveBookingResponse>
        {
            public int Id { get; set; }
            public Guid TenantUniqueId { get; set; } 
        }

        public class RemoveBookingResponse { }

        public class RemoveBookingHandler : IRequestHandler<RemoveBookingRequest, RemoveBookingResponse>
        {
            public RemoveBookingHandler(BookingServiceContext context, ICache cache)
            {
                _context = context;
                _cache = cache;
            }

            public async Task<RemoveBookingResponse> Handle(RemoveBookingRequest request, CancellationToken cancellationToken)
            {
                var booking = await _context.Bookings.SingleAsync(x=>x.Id == request.Id && x.Tenant.UniqueId == request.TenantUniqueId);
                booking.IsDeleted = true;
                await _context.SaveChangesAsync();
                return new RemoveBookingResponse();
            }

            private readonly BookingServiceContext _context;
            private readonly ICache _cache;
        }
    }
}
