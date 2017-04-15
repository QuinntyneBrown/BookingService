using MediatR;
using BookingService.Data;
using BookingService.Data.Model;
using BookingService.Features.Core;
using System;
using System.Threading.Tasks;
using System.Data.Entity;

namespace BookingService.Features.Bookings
{
    public class AddOrUpdateBookingCommand
    {
        public class AddOrUpdateBookingRequest : IRequest<AddOrUpdateBookingResponse>
        {
            public BookingApiModel Booking { get; set; }
            public Guid TenantUniqueId { get; set; }
        }

        public class AddOrUpdateBookingResponse { }

        public class AddOrUpdateBookingHandler : IAsyncRequestHandler<AddOrUpdateBookingRequest, AddOrUpdateBookingResponse>
        {
            public AddOrUpdateBookingHandler(BookingServiceContext context, ICache cache)
            {
                _context = context;
                _cache = cache;
            }

            public async Task<AddOrUpdateBookingResponse> Handle(AddOrUpdateBookingRequest request)
            {
                var entity = await _context.Bookings
                    .Include(x => x.Tenant)
                    .SingleOrDefaultAsync(x => x.Id == request.Booking.Id && x.Tenant.UniqueId == request.TenantUniqueId);
                
                if (entity == null) {
                    var tenant = await _context.Tenants.SingleAsync(x => x.UniqueId == request.TenantUniqueId);
                    _context.Bookings.Add(entity = new Booking() { TenantId = tenant.Id });
                }

                entity.Name = request.Booking.Name;

                entity.Start = request.Booking.Start;

                entity.End = request.Booking.End;

                entity.Description = request.Booking.Description;

                entity.ResourceId = request.Booking.ResourceId;
                
                await _context.SaveChangesAsync();

                return new AddOrUpdateBookingResponse();
            }

            private readonly BookingServiceContext _context;
            private readonly ICache _cache;
        }

    }

}
