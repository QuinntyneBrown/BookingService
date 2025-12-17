using BookingService.Security;
using MediatR;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using static BookingService.Features.Users.AddOrUpdateUserCommand;
using static BookingService.Features.Users.GetUsersQuery;
using static BookingService.Features.Users.GetUserByIdQuery;
using static BookingService.Features.Users.RemoveUserCommand;
using static BookingService.Features.Users.GetUserByUsernameQuery;

namespace BookingService.Features.Users
{
    [Authorize]
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public UserController(IMediator mediator, IUserManager userManager)
        {
            _mediator = mediator;
            _userManager = userManager;
        }

        [HttpPost("add")]
        [ProducesResponseType(typeof(AddOrUpdateUserResponse), 200)]
        public async Task<IActionResult> Add(AddOrUpdateUserRequest request)
        {
            request.TenantId = (await _userManager.GetUserAsync(User)).TenantId;
            return Ok(await _mediator.Send(request));
        }

        [HttpPut("update")]
        [ProducesResponseType(typeof(AddOrUpdateUserResponse), 200)]
        public async Task<IActionResult> Update(AddOrUpdateUserRequest request)
        {
            request.TenantId = (await _userManager.GetUserAsync(User)).TenantId;
            return Ok(await _mediator.Send(request));
        }
        
        [HttpGet("get")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GetUsersResponse), 200)]
        public async Task<IActionResult> Get([FromQuery]GetUsersQuery.GetUsersRequest request)
        {
            request.TenantId = (await _userManager.GetUserAsync(User)).TenantId;
            return Ok(await _mediator.Send(request));
        }

        [HttpGet("getById")]
        [ProducesResponseType(typeof(GetUserByIdResponse), 200)]
        public async Task<IActionResult> GetById([FromQuery]GetUserByIdRequest request)
        {
            request.TenantId = (await _userManager.GetUserAsync(User)).TenantId;
            return Ok(await _mediator.Send(request));
        }

        [HttpDelete("remove")]
        [ProducesResponseType(typeof(RemoveUserResponse), 200)]
        public async Task<IActionResult> Remove([FromQuery]RemoveUserRequest request)
        {
            request.TenantId = (await _userManager.GetUserAsync(User)).TenantId;
            return Ok(await _mediator.Send(request));
        }

        [HttpGet("current")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GetUserByUsernameResponse), 200)]
        public async Task<IActionResult> Current()
        {            
            if (!User.Identity.IsAuthenticated)
                return Ok();
            var request = new GetUserByUsernameRequest();
            request.Username = User.Identity.Name;
            var user = await _userManager.GetUserAsync(User);
            request.TenantId = user.TenantId;
            
            return Ok(await _mediator.Send(request));
        }

        protected readonly IMediator _mediator;
        protected readonly IUserManager _userManager;
    }
}
