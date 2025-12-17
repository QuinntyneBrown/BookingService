using MediatR;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BookingService.Features.Core;
using static BookingService.Features.Accounts.AddOrUpdateAccountCommand;
using static BookingService.Features.Accounts.GetAccountsQuery;
using static BookingService.Features.Accounts.GetAccountByIdQuery;
using static BookingService.Features.Accounts.RemoveAccountCommand;

namespace BookingService.Features.Accounts
{
    [Authorize]
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Route("add")]
        [HttpPost]
        [ProducesResponseType(typeof(AddOrUpdateAccountResponse), 200)]
        public async Task<IHttpActionResult> Add(AddOrUpdateAccountRequest request)
        {
            request.TenantUniqueId = Request.GetTenantUniqueId();
            return Ok(await _mediator.Send(request));
        }

        [Route("update")]
        [HttpPut]
        [ProducesResponseType(typeof(AddOrUpdateAccountResponse), 200)]
        public async Task<IHttpActionResult> Update(AddOrUpdateAccountRequest request)
        {
            request.TenantUniqueId = Request.GetTenantUniqueId();
            return Ok(await _mediator.Send(request));
        }
        
        [Route("get")]
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(typeof(GetAccountsResponse), 200)]
        public async Task<IHttpActionResult> Get()
        {
            var request = new GetAccountsRequest();
            request.TenantUniqueId = Request.GetTenantUniqueId();
            return Ok(await _mediator.Send(request));
        }

        [Route("getById")]
        [HttpGet]
        [ProducesResponseType(typeof(GetAccountByIdResponse), 200)]
        public async Task<IHttpActionResult> GetById([FromQuery]GetAccountByIdRequest request)
        {
            request.TenantUniqueId = Request.GetTenantUniqueId();
            return Ok(await _mediator.Send(request));
        }

        [Route("remove")]
        [HttpDelete]
        [ProducesResponseType(typeof(RemoveAccountResponse), 200)]
        public async Task<IHttpActionResult> Remove([FromQuery]RemoveAccountRequest request)
        {
            request.TenantUniqueId = Request.GetTenantUniqueId();
            return Ok(await _mediator.Send(request));
        }

        protected readonly IMediator _mediator;
    }
}
