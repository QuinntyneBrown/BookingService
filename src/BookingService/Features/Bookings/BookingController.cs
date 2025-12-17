using MediatR;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BookingService.Features.Core;
using static BookingService.Features.Bookings.AddOrUpdateBookingCommand;
using static BookingService.Features.Bookings.GetBookingsQuery;
using static BookingService.Features.Bookings.GetBookingByIdQuery;
using static BookingService.Features.Bookings.RemoveBookingCommand;

namespace BookingService.Features.Bookings
{
    [Authorize]
    [RoutePrefix("api/booking")]
    public class BookingController : ControllerBase
    {
        public BookingController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Route("add")]
        [HttpPost]
        [ProducesResponseType(typeof(AddOrUpdateBookingResponse), 200)]
        public async Task<IHttpActionResult> Add(AddOrUpdateBookingRequest request)
        {
            request.TenantUniqueId = Request.GetTenantUniqueId();
            return Ok(await _mediator.Send(request));
        }

        [Route("update")]
        [HttpPut]
        [ProducesResponseType(typeof(AddOrUpdateBookingResponse), 200)]
        public async Task<IHttpActionResult> Update(AddOrUpdateBookingRequest request)
        {
            request.TenantUniqueId = Request.GetTenantUniqueId();
            return Ok(await _mediator.Send(request));
        }
        
        [Route("get")]
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(typeof(GetBookingsResponse), 200)]
        public async Task<IHttpActionResult> Get()
        {
            var request = new GetBookingsRequest();
            request.TenantUniqueId = Request.GetTenantUniqueId();
            return Ok(await _mediator.Send(request));
        }

        [Route("getById")]
        [HttpGet]
        [ProducesResponseType(typeof(GetBookingByIdResponse), 200)]
        public async Task<IHttpActionResult> GetById([FromQuery]GetBookingByIdRequest request)
        {
            request.TenantUniqueId = Request.GetTenantUniqueId();
            return Ok(await _mediator.Send(request));
        }

        [Route("remove")]
        [HttpDelete]
        [ProducesResponseType(typeof(RemoveBookingResponse), 200)]
        public async Task<IHttpActionResult> Remove([FromQuery]RemoveBookingRequest request)
        {
            request.TenantUniqueId = Request.GetTenantUniqueId();
            return Ok(await _mediator.Send(request));
        }

        protected readonly IMediator _mediator;
    }
}
