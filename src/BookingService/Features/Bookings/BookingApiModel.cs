using BookingService.Data.Model;

namespace BookingService.Features.Bookings
{
    public class BookingApiModel
    {        
        public int Id { get; set; }
        public int? TenantId { get; set; }
        public string Name { get; set; }

        public static TModel FromBooking<TModel>(Booking booking) where
            TModel : BookingApiModel, new()
        {
            var model = new TModel();
            model.Id = booking.Id;
            model.TenantId = booking.TenantId;
            model.Name = booking.Name;
            return model;
        }

        public static BookingApiModel FromBooking(Booking booking)
            => FromBooking<BookingApiModel>(booking);

    }
}
