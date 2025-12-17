using MediatR;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using BookingService.Features.Core;
using static BookingService.Features.Accounts.AddOrUpdateProfileCommand;
using static BookingService.Features.Accounts.GetProfilesQuery;
using static BookingService.Features.Accounts.GetProfileByIdQuery;
using static BookingService.Features.Accounts.RemoveProfileCommand;

namespace BookingService.Features.Accounts
{
    [Authorize]
    [Route("api/profile")]
    public class ProfileController : ControllerBase
    {
        public ProfileController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Route("add")]
        [HttpPost]
        [ProducesResponseType(typeof(AddOrUpdateProfileResponse), 200)]
        public async Task<IActionResult> Add(AddOrUpdateProfileRequest request)
        {
            request.TenantUniqueId = Request.GetTenantUniqueId();
            return Ok(await _mediator.Send(request));
        }

        [Route("update")]
        [HttpPut]
        [ProducesResponseType(typeof(AddOrUpdateProfileResponse), 200)]
        public async Task<IActionResult> Update(AddOrUpdateProfileRequest request)
        {
            request.TenantUniqueId = Request.GetTenantUniqueId();
            return Ok(await _mediator.Send(request));
        }
        
        [Route("get")]
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(typeof(GetProfilesResponse), 200)]
        public async Task<IActionResult> Get()
        {
            var request = new GetProfilesRequest();
            request.TenantUniqueId = Request.GetTenantUniqueId();
            return Ok(await _mediator.Send(request));
        }

        [Route("getById")]
        [HttpGet]
        [ProducesResponseType(typeof(GetProfileByIdResponse), 200)]
        public async Task<IActionResult> GetById([FromQuery]GetProfileByIdRequest request)
        {
            request.TenantUniqueId = Request.GetTenantUniqueId();
            return Ok(await _mediator.Send(request));
        }

        [Route("remove")]
        [HttpDelete]
        [ProducesResponseType(typeof(RemoveProfileResponse), 200)]
        public async Task<IActionResult> Remove([FromQuery]RemoveProfileRequest request)
        {
            request.TenantUniqueId = Request.GetTenantUniqueId();
            return Ok(await _mediator.Send(request));
        }

        protected readonly IMediator _mediator;
    }
}
