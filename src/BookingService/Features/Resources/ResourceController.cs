using MediatR;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BookingService.Features.Core;
using static BookingService.Features.Resources.AddOrUpdateResourceCommand;
using static BookingService.Features.Resources.GetResourcesQuery;
using static BookingService.Features.Resources.GetResourceByIdQuery;
using static BookingService.Features.Resources.RemoveResourceCommand;

namespace BookingService.Features.Resources
{
    [Authorize]
    [Route("api/resource")]
    public class ResourceController : ControllerBase
    {
        public ResourceController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Route("add")]
        [HttpPost]
        [ProducesResponseType(typeof(AddOrUpdateResourceResponse), 200)]
        public async Task<IActionResult> Add(AddOrUpdateResourceRequest request)
        {
            request.TenantUniqueId = Request.GetTenantUniqueId();
            return Ok(await _mediator.Send(request));
        }

        [Route("update")]
        [HttpPut]
        [ProducesResponseType(typeof(AddOrUpdateResourceResponse), 200)]
        public async Task<IActionResult> Update(AddOrUpdateResourceRequest request)
        {
            request.TenantUniqueId = Request.GetTenantUniqueId();
            return Ok(await _mediator.Send(request));
        }
        
        [Route("get")]
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(typeof(GetResourcesResponse), 200)]
        public async Task<IActionResult> Get()
        {
            var request = new GetResourcesRequest();
            request.TenantUniqueId = Request.GetTenantUniqueId();
            return Ok(await _mediator.Send(request));
        }

        [Route("getById")]
        [HttpGet]
        [ProducesResponseType(typeof(GetResourceByIdResponse), 200)]
        public async Task<IActionResult> GetById([FromQuery]GetResourceByIdRequest request)
        {
            request.TenantUniqueId = Request.GetTenantUniqueId();
            return Ok(await _mediator.Send(request));
        }

        [Route("remove")]
        [HttpDelete]
        [ProducesResponseType(typeof(RemoveResourceResponse), 200)]
        public async Task<IActionResult> Remove([FromQuery]RemoveResourceRequest request)
        {
            request.TenantUniqueId = Request.GetTenantUniqueId();
            return Ok(await _mediator.Send(request));
        }

        protected readonly IMediator _mediator;
    }
}
