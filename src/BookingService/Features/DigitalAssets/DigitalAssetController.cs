using BookingService.Features.DigitalAssets.UploadHandlers;
using BookingService.Security;
using MediatR;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http.Headers;

using static BookingService.Features.DigitalAssets.GetDigitalAssetByUniqueIdQuery;
using static BookingService.Features.DigitalAssets.AzureBlobStorageDigitalAssetCommand;

namespace BookingService.Features.DigitalAssets
{
    [Authorize]
    [RoutePrefix("api/digitalasset")]
    public class DigitalAssetController : ControllerBase
    {        
        public DigitalAssetController(IMediator mediator, IUserManager userManager)
        {
            _mediator = mediator;
            _userManager = userManager;
        }

        [Route("add")]
        [HttpPost]
        [ProducesResponseType(typeof(AddOrUpdateDigitalAssetCommand.AddOrUpdateDigitalAssetResponse), 200)]
        public async Task<IActionResult> Add(AddOrUpdateDigitalAssetCommand.AddOrUpdateDigitalAssetRequest request)
            => Ok(await _mediator.Send(request));

        [Route("update")]
        [HttpPut]
        [ProducesResponseType(typeof(AddOrUpdateDigitalAssetCommand.AddOrUpdateDigitalAssetResponse), 200)]
        public async Task<IActionResult> Update(AddOrUpdateDigitalAssetCommand.AddOrUpdateDigitalAssetRequest request)
            => Ok(await _mediator.Send(request));
        
        [Route("get")]
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(typeof(GetDigitalAssetsQuery.GetDigitalAssetsResponse), 200)]
        public async Task<IActionResult> Get()
            => Ok(await _mediator.Send(new GetDigitalAssetsQuery.GetDigitalAssetsRequest()));

        [Route("getById")]
        [HttpGet]
        [ProducesResponseType(typeof(GetDigitalAssetByIdQuery.GetDigitalAssetByIdResponse), 200)]
        public async Task<IActionResult> GetById([FromQuery]GetDigitalAssetByIdQuery.GetDigitalAssetByIdRequest request)
            => Ok(await _mediator.Send(request));

        [Route("remove")]
        [HttpDelete]
        [ProducesResponseType(typeof(RemoveDigitalAssetCommand.RemoveDigitalAssetResponse), 200)]
        public async Task<IActionResult> Remove([FromQuery]RemoveDigitalAssetCommand.RemoveDigitalAssetRequest request)
            => Ok(await _mediator.Send(request));

        [Route("serve")]
        [HttpGet]
        [ProducesResponseType(typeof(GetDigitalAssetByUniqueIdResponse), 200)]
        [AllowAnonymous]
        public async Task<HttpResponseMessage> Serve([FromQuery]GetDigitalAssetByUniqueIdRequest request)
        {
            var response = await _mediator.Send(request);
            var result = new HttpResponseMessage(HttpStatusCode.OK);
            result.Content = new ByteArrayContent(response.DigitalAsset.Bytes);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue(response.DigitalAsset.ContentType);
            return result;
        }

        [Route("upload")]
        [HttpPost]
        public async Task<IActionResult> Upload(HttpRequestMessage request)
        {
            if (!Request.Content.IsMimeMultipartContent("form-data"))
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            var user = await _userManager.GetUserAsync(User);            
            var provider = await Request.Content.ReadAsMultipartAsync(new InMemoryMultipartFormDataStreamProvider());            
            return Ok(await _mediator.Send(new AzureBlobStorageDigitalAssetRequest() { Provider = provider, Folder = $"{user.Tenant.UniqueId}" }));
        }

        protected readonly IMediator _mediator;
        protected readonly IUserManager _userManager;
    }
}