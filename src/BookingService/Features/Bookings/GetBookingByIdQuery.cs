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
    public class GetBookingByIdQuery
    {
        public class GetBookingByIdRequest : IRequest<GetBookingByIdResponse> { 
            public int Id { get; set; }
            public Guid TenantUniqueId { get; set; }
        }

        public class GetBookingByIdResponse
        {
            public BookingApiModel Booking { get; set; } 
        }

        public class GetBookingByIdHandler : IAsyncRequestHandler<GetBookingByIdRequest, GetBookingByIdResponse>
        {
            public GetBookingByIdHandler(BookingServiceContext context, ICache cache)
            {
                _context = context;
                _cache = cache;
            }

            public async Task<GetBookingByIdResponse> Handle(GetBookingByIdRequest request)
            {                
                return new GetBookingByIdResponse()
                {
                    Booking = BookingApiModel.FromBooking(await _context.Bookings
                    .Include(x => x.Tenant)				
					.SingleAsync(x=>x.Id == request.Id &&  x.Tenant.UniqueId == request.TenantUniqueId))
                };
            }

            private readonly BookingServiceContext _context;
            private readonly ICache _cache;
        }

    }

}
