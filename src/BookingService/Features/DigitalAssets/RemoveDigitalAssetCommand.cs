using System.Threading;
using MediatR;
using BookingService.Data;
using System.Threading.Tasks;
using BookingService.Features.Core;

namespace BookingService.Features.DigitalAssets
{
    public class RemoveDigitalAssetCommand
    {
        public class RemoveDigitalAssetRequest : IRequest<RemoveDigitalAssetResponse>
        {
            public int Id { get; set; }
        }

        public class RemoveDigitalAssetResponse { }

        public class RemoveDigitalAssetHandler : IRequestHandler<RemoveDigitalAssetRequest, RemoveDigitalAssetResponse>
        {
            public RemoveDigitalAssetHandler(IBookingServiceContext context, ICache cache)
            {
                _context = context;
                _cache = cache;
            }

            public async Task<RemoveDigitalAssetResponse> Handle(RemoveDigitalAssetRequest request, CancellationToken cancellationToken)
            {
                var digitalAsset = await _context.DigitalAssets.FindAsync(request.Id);
                digitalAsset.IsDeleted = true;
                await _context.SaveChangesAsync();
                return new RemoveDigitalAssetResponse();
            }

            private readonly IBookingServiceContext _context;
            private readonly ICache _cache;
        }
    }
}
