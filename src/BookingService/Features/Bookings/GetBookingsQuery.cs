using MediatR;
using BookingService.Data;
using BookingService.Features.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Data.Entity;

namespace BookingService.Features.Bookings
{
    public class GetBookingsQuery
    {
        public class GetBookingsRequest : IRequest<GetBookingsResponse> { 
            public Guid TenantUniqueId { get; set; }       
        }

        public class GetBookingsResponse
        {
            public ICollection<BookingApiModel> Bookings { get; set; } = new HashSet<BookingApiModel>();
        }

        public class GetBookingsHandler : IAsyncRequestHandler<GetBookingsRequest, GetBookingsResponse>
        {
            public GetBookingsHandler(BookingServiceContext context, ICache cache)
            {
                _context = context;
                _cache = cache;
            }

            public async Task<GetBookingsResponse> Handle(GetBookingsRequest request)
            {
                var bookings = await _context.Bookings
                    .Include(x => x.Tenant)
                    .Where(x => x.Tenant.UniqueId == request.TenantUniqueId )
                    .ToListAsync();

                return new GetBookingsResponse()
                {
                    Bookings = bookings.Select(x => BookingApiModel.FromBooking(x)).ToList()
                };
            }

            private readonly BookingServiceContext _context;
            private readonly ICache _cache;
        }

    }

}
