using MediatR;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using BookingService.Features.Core;
using static BookingService.Features.Bookings.AddOrUpdateBookingCommand;
using static BookingService.Features.Bookings.GetBookingsQuery;
using static BookingService.Features.Bookings.GetBookingByIdQuery;
using static BookingService.Features.Bookings.RemoveBookingCommand;

namespace BookingService.Features.Bookings
{
    [Authorize]
    [RoutePrefix("api/booking")]
    public class BookingController : ApiController
    {
        public BookingController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Route("add")]
        [HttpPost]
        [ResponseType(typeof(AddOrUpdateBookingResponse))]
        public async Task<IHttpActionResult> Add(AddOrUpdateBookingRequest request)
        {
            request.TenantUniqueId = Request.GetTenantUniqueId();
            return Ok(await _mediator.Send(request));
        }

        [Route("update")]
        [HttpPut]
        [ResponseType(typeof(AddOrUpdateBookingResponse))]
        public async Task<IHttpActionResult> Update(AddOrUpdateBookingRequest request)
        {
            request.TenantUniqueId = Request.GetTenantUniqueId();
            return Ok(await _mediator.Send(request));
        }
        
        [Route("get")]
        [AllowAnonymous]
        [HttpGet]
        [ResponseType(typeof(GetBookingsResponse))]
        public async Task<IHttpActionResult> Get()
        {
            var request = new GetBookingsRequest();
            request.TenantUniqueId = Request.GetTenantUniqueId();
            return Ok(await _mediator.Send(request));
        }

        [Route("getById")]
        [HttpGet]
        [ResponseType(typeof(GetBookingByIdResponse))]
        public async Task<IHttpActionResult> GetById([FromUri]GetBookingByIdRequest request)
        {
            request.TenantUniqueId = Request.GetTenantUniqueId();
            return Ok(await _mediator.Send(request));
        }

        [Route("remove")]
        [HttpDelete]
        [ResponseType(typeof(RemoveBookingResponse))]
        public async Task<IHttpActionResult> Remove([FromUri]RemoveBookingRequest request)
        {
            request.TenantUniqueId = Request.GetTenantUniqueId();
            return Ok(await _mediator.Send(request));
        }

        protected readonly IMediator _mediator;
    }
}
