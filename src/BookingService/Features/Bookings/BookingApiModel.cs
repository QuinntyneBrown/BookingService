using BookingService.Data.Model;
using BookingService.Features.Resources;
using System;

namespace BookingService.Features.Bookings
{
    public class BookingApiModel
    {        
        public int Id { get; set; }

        public int? TenantId { get; set; }

        public string Name { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public int? ResourceId { get; set; }

        public string Description { get; set; }

        public ResourceApiModel Resource { get; set; }

        public static TModel FromBooking<TModel>(Booking booking) where
            TModel : BookingApiModel, new()
        {
            var model = new TModel();

            model.Id = booking.Id;

            model.TenantId = booking.TenantId;

            model.Name = booking.Name;

            model.Start = booking.Start;

            model.End = booking.End;

            model.Description = booking.Description;

            model.ResourceId = booking.ResourceId;

            model.Resource = ResourceApiModel.FromResource(booking.Resource);

            return model;
        }

        public static BookingApiModel FromBooking(Booking booking)
            => FromBooking<BookingApiModel>(booking);

    }
}
